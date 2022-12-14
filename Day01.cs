namespace AoC2022;

internal class Day01 : BaseDay
{
	public override int DayNumber => 1;

	public override async Task<string> Solve1Async()
	{
		var maxCal = (await GetNiceInputAsync()).Max(x => x.TotalCalories);
		return maxCal.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var maxCalGroup = (await GetNiceInputAsync()).OrderByDescending(x => x.TotalCalories).Take(3).Sum(x => x.TotalCalories);
		return maxCalGroup.ToString();
	}

	private static async Task<IReadOnlyList<Elf>> GetNiceInputAsync()
	{
		var inputLines = await InputFetcher.GetAllInputLinesAsync(1);

		List<Elf> elves = new();
		var currentElf = new Elf();

		foreach (var line in inputLines)
		{
			if (string.IsNullOrEmpty(line))
			{
				elves.Add(currentElf);
				currentElf = new Elf();
				continue;
			}

			currentElf.FoodItems.Add(int.Parse(line));
		}
		return elves;
	}
}

internal class Elf
{
	public List<int> FoodItems { get; set; }

	public int TotalCalories => FoodItems.Sum();

	public Elf()
	{
		FoodItems = new List<int>();
	}
}
