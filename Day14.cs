namespace AoC2022;

internal class Day14 : BaseDay
{
	public override int DayNumber => 14;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(14)).Where(x => !string.IsNullOrEmpty(x));
		//var lines = new List<string>
		//{
		//	"498,4 -> 498,6 -> 496,6",
		//	"503,4 -> 502,4 -> 502,9 -> 494,9",
		//};

		Dictionary<(int x, int y), bool> map = ParseMap(lines);

		int minX = map.Min(x => x.Key.x);
		int maxX = map.Max(x => x.Key.x);
		int maxY = map.Max(x => x.Key.y);

		bool hasFallenIntoVoid = false;

		int restingSand = 0;

		while (!hasFallenIntoVoid)
		{
			(int x, int y) sand = (500, 0);
			bool falling = true;
			while (falling)
			{
				// Just 4 fun
				//PrintWindow(sand, map);
				//Thread.Sleep(2);

				if (!IsInside(sand, minX, maxX, maxY))
				{
					hasFallenIntoVoid = true;
					break;
				}

				if (!map.ContainsKey((sand.x, sand.y + 1)))
				{
					sand = (sand.x, sand.y + 1);
					continue;
				}
				if (!map.ContainsKey((sand.x - 1, sand.y + 1)))
				{
					sand = (sand.x - 1, sand.y + 1);
					continue;
				}
				if (!map.ContainsKey((sand.x + 1, sand.y + 1)))
				{
					sand = (sand.x + 1, sand.y + 1);
					continue;
				}

				restingSand++;
				map[sand] = true;
				falling = false;
			}
		}

		return restingSand.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(14)).Where(x => !string.IsNullOrEmpty(x));
		//var lines = new List<string>
		//{
		//	"498,4 -> 498,6 -> 496,6",
		//	"503,4 -> 502,4 -> 502,9 -> 494,9",
		//};

		Dictionary<(int x, int y), bool> map = ParseMap(lines);

		int floorY = map.Max(x => x.Key.y) + 2;

		bool isAtTop = false;

		int restingSand = 1;

		while (!isAtTop)
		{
			(int x, int y) sand = (500, 0);
			bool falling = true;
			while (falling)
			{
				var isOnFloor = (sand.y + 1) == floorY;

				if (!map.ContainsKey((sand.x, sand.y + 1)) && !isOnFloor)
				{
					sand = (sand.x, sand.y + 1);
					continue;
				}
				if (!map.ContainsKey((sand.x - 1, sand.y + 1)) && !isOnFloor)
				{
					sand = (sand.x - 1, sand.y + 1);
					continue;
				}
				if (!map.ContainsKey((sand.x + 1, sand.y + 1)) && !isOnFloor)
				{
					sand = (sand.x + 1, sand.y + 1);
					continue;
				}

				if (sand == (500, 0))
				{
					isAtTop = true;
					break;
				}

				restingSand++;
				map[sand] = true;
				falling = false;
			}
		}

		return restingSand.ToString();
	}

	private bool IsInside((int x, int y) c, int minX, int maxX, int maxY)
	{
		if (c.x < minX) { return false; }
		if (c.x > maxX) { return false; }
		if (c.y > maxY) { return false; }

		return true;
	}

	private void PrintWindow((int x, int y) c, Dictionary<(int, int), bool> map)
	{
		Console.CursorTop = 0;
		Console.CursorLeft = 0;
		int windowWidth = 50;
		int windowHeight = 16;
		string window = "";
		for (int y = (c.y - windowHeight); y < (c.y + windowHeight); y++)
		{
			for (int x = (c.x - windowWidth); x < (c.x + windowWidth); x++)
			{
				if ((x, y) == c)
				{
					window += 'O';
					continue;
				}

				if (map.ContainsKey((x, y)))
				{
					window += '█';
				}
				else
				{
					window += ' ';
				}
			}
			window += '\n';
		}

		Console.WriteLine(window);
	}

	private static Dictionary<(int, int), bool> ParseMap(IEnumerable<string> lines)
	{
		Dictionary<(int, int), bool> map = new Dictionary<(int, int), bool>();

		foreach (var line in lines)
		{
			List<(int x, int y)> points = line
				.Split(" -> ")
				.Select(x =>
				{
					var temp = x.Split(",");
					return (int.Parse(temp[0]), int.Parse(temp[1]));
				})
				.ToList();

			for (int i = 0; i < points.Count - 1; i++)
			{
				var a = points[i];
				var b = points[i + 1];

				var hasSameX = a.x == b.x;
				var from = hasSameX ? a.y : a.x;
				var to = hasSameX ? b.y : b.x;
				var dir = to < from ? -1 : 1;

				for (int j = from; (j - dir) != to; j += dir)
				{
					if (hasSameX)
					{
						map[(a.x, j)] = true;
					}
					else
					{
						map[(j, a.y)] = true;
					}
				}
			}
		}

		return map;
	}
}
