namespace AoC2022;

internal class Day03 : BaseDay
{
	public override int DayNumber => 3;

	public override async Task<string> Solve1Async()
	{
		var lines = await InputFetcher.GetAllInputLinesAsync(3);
		//var lines = new List<string>
		//{
		//	"vJrwpWtwJgWrhcsFMMfFFhFp",
		//	"jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
		//	"PmmdzqPrVvPwwTWBwg",
		//	"wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
		//	"ttgJtRGJQctTZtZT",
		//	"CrZsJsPPZsGzwwsLwLmpwMDw",
		//};

		return lines
			.Where(x => !string.IsNullOrEmpty(x))
			.Select(x => new Backpack(x))
			.Select(x => x.SameCharInCompartments())
			.Sum(x => Backpack.GetItemValue(x))
			.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(3)).Where(x => !string.IsNullOrEmpty(x)).ToList();
		//var lines = new List<string>
		//{
		//	"vJrwpWtwJgWrhcsFMMfFFhFp",
		//	"jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
		//	"PmmdzqPrVvPwwTWBwg",
		//	"wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
		//	"ttgJtRGJQctTZtZT",
		//	"CrZsJsPPZsGzwwsLwLmpwMDw",
		//};

		long sum = 0;

		for (int i = 0; i < lines.Count; i += 3)
		{
			List<Backpack> group = new List<Backpack>
			{
				new Backpack(lines[i]),
				new Backpack(lines[i+1]),
				new Backpack(lines[i+2]),
			};

			List<char> intersectAll =
				group[0].AllItemsList
					.Intersect(group[1].AllItemsList
						.Intersect(group[2].AllItemsList))
					.ToList();

			if (intersectAll.Count() != 1)
			{
				throw new ArgumentException("Too many backpacks");
			}

			sum += Backpack.GetItemValue(intersectAll[0]);
		}

		return sum.ToString();
	}
}

internal class Backpack
{
	public string AllItems { get; }

	public List<char> AllItemsList { get; }

	public List<char> _compartment1 = new List<char>();
	public List<char> _compartment2 = new List<char>();

	public Backpack(string allItems)
	{
		AllItems = allItems;

		AllItemsList = allItems.ToList();

		int mid = allItems.Length / 2;
		_compartment1 = allItems.Substring(0, mid).ToList();
		_compartment2 = allItems.Substring(mid).ToList();
	}

	public char SameCharInCompartments()
	{
		var union =_compartment1.Intersect(_compartment2).ToList();
		if (union.Count > 1)
		{
			throw new ArgumentOutOfRangeException("Too many items found!");
		}

		return union[0];
	}

	public static long GetItemValue(char c)
	{
		var charInt = (int)c;
		return charInt > 96
			? charInt - 96 // Lowercase
			: (charInt - 64) + 26; // Uppercase
	}
}
