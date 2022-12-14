namespace AoC2022;

internal class Day11 : BaseDay
{
	public override int DayNumber => 11;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(11)).Chunk(7);

		List<Monkey> monkeys = ParseMonkeys(lines);

		for (ulong i = 0; i < 20; i++)
		{
			foreach (var m in monkeys)
			{
				m.PassItems(monkeys, true);
			}
		}

		var orderedMonkeys = monkeys.OrderByDescending(x => x.InspectionCount).ToList();
		var monkeyBuisness = orderedMonkeys[0].InspectionCount * orderedMonkeys[1].InspectionCount;

		return monkeyBuisness.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(11)).Chunk(7);
		//var lines = new List<string>
		//{
		//	"Monkey 0:",
		//	"  Starting items: 79, 98",
		//	"  Operation: new = old * 19",
		//	"  Test: divisible by 23",
		//	"    If true: throw to monkey 2",
		//	"    If false: throw to monkey 3",
		//	"",
		//	"Monkey 1:",
		//	"  Starting items: 54, 65, 75, 74",
		//	"  Operation: new = old + 6",
		//	"  Test: divisible by 19",
		//	"    If true: throw to monkey 2",
		//	"    If false: throw to monkey 0",
		//	"",
		//	"Monkey 2:",
		//	"  Starting items: 79, 60, 97",
		//	"  Operation: new = old * old",
		//	"  Test: divisible by 13",
		//	"    If true: throw to monkey 1",
		//	"    If false: throw to monkey 3",
		//	"",
		//	"Monkey 3:",
		//	"  Starting items: 74",
		//	"  Operation: new = old + 3",
		//	"  Test: divisible by 17",
		//	"    If true: throw to monkey 0",
		//	"    If false: throw to monkey 1",
		//	"",
		//}.Chunk(7);

		List<Monkey> monkeys = ParseMonkeys(lines);

		var divValue = monkeys.Select(x => x.Div).Aggregate((ulong)1, (a, b) => a * b);

		for (ulong i = 0; i < 10000; i++)
		{
			foreach (var m in monkeys)
			{
				m.PassItems(monkeys, false, divValue);
			}
		}

		var orderedMonkeys = monkeys.OrderByDescending(x => x.InspectionCount).ToList();
		var monkeyBuisness = orderedMonkeys[0].InspectionCount * orderedMonkeys[1].InspectionCount;

		return monkeyBuisness.ToString();
	}

	private static List<Monkey> ParseMonkeys(IEnumerable<string[]> lines)
	{
		List<Monkey> monkeys = new List<Monkey>();
		foreach (var line in lines)
		{
			string monkeyNumberTemp = line[0].Split(' ')[1];
			ulong monkeyNumber = ulong.Parse(monkeyNumberTemp.Remove(monkeyNumberTemp.Length - 1, 1));

			List<ulong> items = line[1]["  Starting items: ".Length..].Split(", ").Select(x => ulong.Parse(x)).ToList();
			var operation = line[2]["  Operation: new = ".Length..].Split(" ").ToList();
			Func<ulong, ulong> op =
				(input) =>
				{
					var val1 = operation[0] == "old" ? input : ulong.Parse(operation[0]);
					var val2 = operation[2] == "old" ? input : ulong.Parse(operation[2]);

					if (operation[1] == "*")
					{
						return val1 * val2;
					}
					else
					{
						return val1 + val2;
					}
				};

			var divTestVal = ulong.Parse(line[3]["  Test: divisible by ".Length..]);
			Func<ulong, bool> divTest =
				(input) =>
				{
					return input % divTestVal == 0;
				};

			int trueMonkey = int.Parse(line[4]["    If true: throw to monkey ".Length..]);
			int falseMonkey = int.Parse(line[5]["    If false: throw to monkey ".Length..]);

			monkeys.Add(new Monkey(items, op, divTest, trueMonkey, falseMonkey, divTestVal));
		}
		return monkeys;
	}
}

public class Monkey
{
	private Func<ulong, ulong> _operation;
	private Func<ulong, bool> _test;
	private int _trueMonkey;
	private int _falseMonkey;

	Queue<ulong> Items { get; init; }

	public ulong InspectionCount { get; private set; }

	public ulong Div { get; }

	public Monkey(
		IList<ulong> startingItems,
		Func<ulong, ulong> operation,
		Func<ulong, bool> test,
		int trueMonkey,
		int falseMoneky,
		ulong div)
	{
		Items = new Queue<ulong>(startingItems);
		_operation = operation;
		_test = test;
		_trueMonkey = trueMonkey;
		_falseMonkey = falseMoneky;
		Div = div;
	}

	public void PassItems(List<Monkey> monkeys, bool isChill, ulong divValue = 0)
	{
		if (!Items.Any())
		{
			return;
		}

		ulong item;

		do
		{
			item = Items.Dequeue();
			InspectionCount++;

			ulong newWorryLevel = _operation(item);
			if (isChill)
			{
				newWorryLevel /= 3;
			}
			if (divValue != 0)
			{
				newWorryLevel %= divValue;
			}

			int monkeyToPass = _test(newWorryLevel) ? _trueMonkey : _falseMonkey;

			monkeys[monkeyToPass].Items.Enqueue(newWorryLevel);

		} while (Items.Any());
	}

	public override string ToString()
	{
		return $"M[{InspectionCount}]: ({string.Join(", ", Items)})";
	}
}

