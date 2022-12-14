namespace AoC2022;

internal class Day04 : BaseDay
{
	public override int DayNumber => 4;

	public override async Task<string> Solve1Async()
	{
		IList<string> list = await InputFetcher.GetAllInputLinesAsync(4);
		//IList<string> list = new List<string>
		//{
		//	"2-4,6-8",
		//	"2-3,4-5",
		//	"5-7,7-9",
		//	"2-8,3-7",
		//	"6-6,4-6",
		//	"2-6,4-8",
		//};

		var cleaningPairs = list
			.Take(list.Count-1)
			.SelectMany(x => x.Split(","))
			.Select(x =>
		{
			var rangeVals = x.Split("-");
			return new CleaningPairItem(int.Parse(rangeVals[0]), int.Parse(rangeVals[1]));
		})
			.ToList();

		long count = 0;

		foreach (var pairs in cleaningPairs.Chunk(2))
		{
			if (pairs[0].Surrounds(pairs[1]) || pairs[1].Surrounds(pairs[0]))
			{
				count++;
			}
		}

		return count.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		IList<string> list = await InputFetcher.GetAllInputLinesAsync(4);
		//IList<string> list = new List<string>
		//{
		//	"2-4,6-8",
		//	"2-3,4-5",
		//	"5-7,7-9",
		//	"2-8,3-7",
		//	"6-6,4-6",
		//	"2-6,4-8",
		//};

		var cleaningPairs = list
			.Take(list.Count - 1)
			.Select(x =>
			{
				var item1and2 = x.Split(",");

				var rangeVals1 = item1and2[0].Split("-");
				var item1 = new CleaningPairItem(int.Parse(rangeVals1[0]), int.Parse(rangeVals1[1]));

				var rangeVals2 = item1and2[1].Split("-");
				var item2 = new CleaningPairItem(int.Parse(rangeVals2[0]), int.Parse(rangeVals2[1]));

				return new CleaningPair(item1, item2);
			})
			.ToList();


		return cleaningPairs.Where(x => x.Overlaps).Count().ToString();
	}
}


internal struct CleaningPair
{
	public CleaningPairItem Item1;

	public CleaningPairItem Item2;

	public CleaningPair(CleaningPairItem item1, CleaningPairItem item2)
	{
		Item1 = item1;
		Item2 = item2;
	}

	public bool HasCompleteOverlap => Item1.Surrounds(Item2) || Item2.Surrounds(Item1);

	public bool Overlaps => Item1.Overlaps(Item2) || Item2.Overlaps(Item1);
}

internal struct CleaningPairItem
{
	public long Min;

	public long Max;

	public long RangeLength => Max - Min;

	public CleaningPairItem(long min, long max)
	{
		Min = min;
		Max = max; 
	}

	public bool Surrounds(CleaningPairItem other)
	{
		return Min <= other.Min && Max >= other.Max;
	}

	public bool Overlaps(CleaningPairItem other)
	{
		return Min <= other.Max && Max >= other.Min;
	}

	public override string ToString()
	{
		return $"{Min}-{Max}";
	}
}
