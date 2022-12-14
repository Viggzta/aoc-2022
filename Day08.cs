namespace AoC2022;

internal class Day08 : BaseDay
{
	public override int DayNumber => 8;

	public override async Task<string> Solve1Async()
	{
		List<string> lines = (await InputFetcher.GetAllInputLinesAsync(8)).Where(x => !string.IsNullOrEmpty(x)).ToList();
		//List<string> lines = new List<string>
		//{
		//	"30373",
		//	"25512",
		//	"65332",
		//	"33549",
		//	"35390",
		//};

		int width = lines.First().Length;
		int height = lines.Count();

		int[,] treeMap = new int[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				treeMap[x, y] = int.Parse(lines[y][x].ToString());
			}
		}

		var visible = GetVisiblilityMap(treeMap);


		////for (int y = 0; y < height; y++)
		////{
		////	for (int x = 0; x < width; x++)
		////	{
		////		Console.Write($"{(visible[x, y] ? 1 : 0)}");
		////	}
		////	Console.WriteLine();
		////}
		

		var visibleTrees = Flatten(visible).Count(x => x);

		return visibleTrees.ToString();
	}

	private bool[,] GetVisiblilityMap(int[,] treeMap)
	{
		var width = treeMap.GetLength(0);
		var height = treeMap.GetLength(1);

		bool[,] visibleMap = new bool[width, height];

		// LeftToRight
		for (int y = 0; y < height; y++)
		{
			int highestSeen = -1;
			for (int x = 0; x < width; x++)
			{
				if (treeMap[x, y] <= highestSeen)
				{
					continue;
				}

				highestSeen = treeMap[x, y];
				visibleMap[x, y] = true;
			}
		}

		// RightToLeft
		for (int y = 0; y < height; y++)
		{
			int highestSeen = -1;
			for (int x = width-1; x >= 0; x--)
			{
				if (treeMap[x, y] <= highestSeen)
				{
					continue;
				}

				highestSeen = treeMap[x, y];
				visibleMap[x, y] = true;
			}
		}

		// BotToTop
		for (int x = 0; x < width; x++)
		{
			int highestSeen = -1;
			for (int y = height - 1; y >= 0; y--)
			{
				if (treeMap[x, y] <= highestSeen)
				{
					continue;
				}

				highestSeen = treeMap[x, y];
				visibleMap[x, y] = true;
			}
		}

		// TopToBot
		for (int x = 0; x < width; x++)
		{
			int highestSeen = -1;
			for (int y = 0; y < height; y++)
			{
				if (treeMap[x, y] <= highestSeen)
				{
					continue;
				}

				highestSeen = treeMap[x, y];
				visibleMap[x, y] = true;
			}
		}

		return visibleMap;
	}

	private T[] Flatten<T>(T[,] array)
	{
		var width = array.GetLength(0);
		var height = array.GetLength(1);
		T[] outArr = new T[width * height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				outArr[x * width + y] = array[x, y];
			}
		}

		return outArr;
	}

	private List<long> GetScenicScores(int[,] treeMap)
	{
		var width = treeMap.GetLength(0);
		var height = treeMap.GetLength(1);

		List<long> scenicScores = new List<long>();

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				scenicScores.Add(GetScenicScoreOneTree(treeMap, x, y));
			}
		}

		return scenicScores;
	}

	private long GetScenicScoreOneTree(int[,] treeMap, int cX, int cY)
	{
		var currentTree = treeMap[cX, cY];
		var left = 0;
		for (int x = cX - 1; x >= 0; x--)
		{
			left++;

			if (treeMap[x, cY] >= currentTree)
			{
				break;
			}
		}

		var right = 0;
		for (int x = cX + 1; x < treeMap.GetLength(0); x++)
		{
			right++;

			if (treeMap[x, cY] >= currentTree)
			{
				break;
			}
		}

		var up = 0;
		for (int y = cY - 1; y >= 0; y--)
		{
			up++;

			if (treeMap[cX, y] >= currentTree)
			{
				break;
			}
		}

		var down = 0;
		for (int y = cY + 1; y < treeMap.GetLength(1); y++)
		{
			down++;

			if (treeMap[cX, y] >= currentTree)
			{
				break;
			}
		}

		return left * right * up * down;
	}

	public override async Task<string> Solve2Async()
	{
		List<string> lines = (await InputFetcher.GetAllInputLinesAsync(8)).Where(x => !string.IsNullOrEmpty(x)).ToList();
		//List<string> lines = new List<string>
		//{
		//	"30373",
		//	"25512",
		//	"65332",
		//	"33549",
		//	"35390",
		//};

		int width = lines.First().Length;
		int height = lines.Count();

		int[,] treeMap = new int[width, height];
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				treeMap[x, y] = int.Parse(lines[y][x].ToString());
			}
		}

		var scores = GetScenicScores(treeMap);
		var highscore = scores.Max(x => x);

		return highscore.ToString();
	}
}

