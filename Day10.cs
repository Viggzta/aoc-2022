namespace AoC2022;

internal class Day10 : BaseDay
{
	public override int DayNumber => 10;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(10)).Where(x => !string.IsNullOrEmpty(x));

		List<Operation> operations = ParseToOperations(lines);

		long sum = 0;
		for (int i = 20; i <= 220; i += 40)
		{
			long regX = 1;
			List<Operation> ops = GetOperationsUntil(operations, i);
			ops.ForEach(x => x.Operate(ref regX));
			sum += regX * i;
		}

		return sum.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(10)).Where(x => !string.IsNullOrEmpty(x));

		List<Operation> operations = ParseToOperations(lines);
		bool[] renderBuffer = new bool[240];

		for (int i = 1; i <= renderBuffer.Length; i++)
		{
			long regX = 1;
			List<Operation> ops = GetOperationsUntil(operations, i);
			ops.ForEach(x => x.Operate(ref regX));

			var spritePos = regX;
			var currentCrtPos = ((i - 1) % 40);
			if (spritePos == currentCrtPos || spritePos - 1 == currentCrtPos || spritePos + 1 == currentCrtPos)
			{
				renderBuffer[i - 1] = true;
			}
		}


		string screen = "";
		for (int i = 0; i < renderBuffer.Length; i++)
		{
			if (i % 40 == 0)
			{
				screen += "\n";
			}

			screen += renderBuffer[i] ? "X" : " ";
		}

		return AsciiHelper.ToString(screen.Split("\n").Skip(1).ToList());
	}

	private static List<Operation> ParseToOperations(IEnumerable<string> lines)
	{
		List<Operation> operations = new List<Operation>();
		foreach (var line in lines)
		{
			var instrction = line.Split(' ');

			switch (instrction[0])
			{
				case "noop":
					operations.Add(new NoOp());
					break;
				case "addx":
					operations.Add(new AddOp(long.Parse(instrction[1])));
					break;
			}
		}

		return operations;
	}

	private static List<Operation> GetOperationsUntil(List<Operation> operations, long targetOp)
	{
		long sum = 0;

		List<Operation> internalOperations = new List<Operation>();
		while (internalOperations.Sum(x => x.CycleCost) < targetOp)
		{
			internalOperations.AddRange(operations);
		}

		return internalOperations.TakeWhile(x =>
		{
			sum += x.CycleCost;
			return sum < targetOp;
		}).ToList();
	}
}


public abstract class Operation
{
	public abstract long CycleCost { get; }

	public abstract void Operate(ref long register);
}

public class NoOp : Operation
{
	public override long CycleCost => 1;

	public override void Operate(ref long register)
	{
	}

	public override string ToString()
	{
		return "noop";
	}
}

public class AddOp : Operation
{
	public override long CycleCost => 2;

	private long _addAmount;

	public AddOp(long addAmount)
	{
		_addAmount = addAmount;
	}

	public override void Operate(ref long register)
	{
		register += _addAmount;
	}

	public override string ToString()
	{
		return $"addx {_addAmount}";
	}
}
