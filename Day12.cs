namespace AoC2022;

internal class Day12 : BaseDay
{
	public override int DayNumber => 12;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(12)).Where(x => !string.IsNullOrEmpty(x)).ToList();
		//List<string> lines = new List<string>
		//{
		//	"Sabqponm",
		//	"abcryxxl",
		//	"accszExk",
		//	"acctuvwj",
		//	"abdefghi",
		//};

		PathNode? start = null;
		PathNode? goal = null;

		int width = lines.First().Length;
		int height = lines.Count;

		List<PathNode> allNodes = new List<PathNode>();

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				var currentChar = lines[y][x];
				PathNode n = new PathNode(x, y, ElevationFromChar(currentChar));
				allNodes.Add(n);

				if (currentChar == 'S') { start = n; }
				if (currentChar == 'E') { goal = n; }
			}
		}

		if (start == null || goal == null)
		{
			throw new NotImplementedException();
		}

		List<PathNode> evaluatedNodes = new List<PathNode>();
		List<PathNode> exploreSet = new List<PathNode> { start };
		start.DistanceToThis = 0;

		List<PathNode> finalPath = new List<PathNode>();

		while (exploreSet.Any())
		{
			var evaluationNode = exploreSet.OrderBy(x => x.DistanceToThis).First();

			if (evaluationNode == goal)
			{
				finalPath = ReconstructPath(start, goal);
				break;
			}

			exploreSet.Remove(evaluationNode);

			var neighbours = GetNeighbours(evaluationNode, allNodes).Where(x => evaluationNode.Distance(x) <= 1);
			foreach (var neighbour in neighbours)
			{
				var newDistance = evaluationNode.DistanceToThis + 1;
				if (newDistance < neighbour.DistanceToThis)
				{
					neighbour.CameFrom = evaluationNode;
					neighbour.DistanceToThis = newDistance;
					if (!exploreSet.Contains(neighbour))
					{
						exploreSet.Add(neighbour);
					}
				}
			}
		}

		return (finalPath.Count - 1).ToString();
	}

	private List<PathNode> ReconstructPath(PathNode start, PathNode goal)
	{
		List<PathNode> path = new List<PathNode>();
		var current = goal;
		while (current != null)
		{
			path.Add(current);
			current = current.CameFrom;
		}

		return path;
	}

	private List<PathNode> GetNeighbours(PathNode current, List<PathNode> allNodes)
	{
		return allNodes
			.Where(n =>
				(n.X == current.X - 1 && n.Y == current.Y) ||
				(n.X == current.X + 1 && n.Y == current.Y) ||
				(n.X == current.X && n.Y == current.Y - 1) ||
				(n.X == current.X && n.Y == current.Y + 1)
			)
			.ToList();

	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(12)).Where(x => !string.IsNullOrEmpty(x)).ToList();
		//List<string> lines = new List<string>
		//{
		//	"Sabqponm",
		//	"abcryxxl",
		//	"accszExk",
		//	"acctuvwj",
		//	"abdefghi",
		//};

		List<PathNode> allStarts = new List<PathNode>();
		PathNode? goal = null;

		int width = lines.First().Length;
		int height = lines.Count;

		List<PathNode> allNodes = new List<PathNode>();

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				var currentChar = lines[y][x];
				PathNode n = new PathNode(x, y, ElevationFromChar(currentChar));
				allNodes.Add(n);

				if (currentChar == 'S' || currentChar == 'a') { allStarts.Add(n); }
				if (currentChar == 'E') { goal = n; }
			}
		}

		List<Task<int>> tasks = new List<Task<int>>();

		foreach (var s in allStarts)
		{
			(List<PathNode> all, PathNode? localStart, PathNode? localGoal) = GetCopy(allNodes, s, goal);
			tasks.Add(Task.Run(() => { return GetPathLength(localStart, localGoal, all); }));
		}

		var lengths = await Task.WhenAll(tasks);
		var finalPathLength = lengths.Where(x => x > 0).OrderBy(x => x).First();

		return finalPathLength.ToString();
	}

	private (List<PathNode> all, PathNode? start, PathNode? goal) GetCopy(List<PathNode> allNodes, PathNode s, PathNode? g)
	{
		PathNode? start = null;
		PathNode? goal = null;
		List<PathNode> val = new List<PathNode>();
		foreach (var n in allNodes)
		{
			var newNode = new PathNode(
				n.X,
				n.Y,
				n.Elevation);

			if (n == s) { start = newNode; }
			if (n == g) { goal = newNode; }

			val.Add(newNode);
		}

		return (val, start, goal);
	}

	private int GetPathLength(PathNode? start, PathNode? goal, List<PathNode> allNodes)
	{
		if (start == null || goal == null)
		{
			throw new NotImplementedException();
		}

		List<PathNode> evaluatedNodes = new List<PathNode>();
		List<PathNode> exploreSet = new List<PathNode> { start };
		start.DistanceToThis = 0;

		List<PathNode> finalPath = new List<PathNode>();

		while (exploreSet.Any())
		{
			var evaluationNode = exploreSet.OrderBy(x => x.DistanceToThis).First();

			if (evaluationNode.SameAs(goal))
			{
				finalPath = ReconstructPath(start, goal);
				break;
			}

			exploreSet.Remove(evaluationNode);

			var neighbours = GetNeighbours(evaluationNode, allNodes).Where(x => evaluationNode.Distance(x) <= 1);
			foreach (var neighbour in neighbours)
			{
				var newDistance = evaluationNode.DistanceToThis + 1;
				if (newDistance < neighbour.DistanceToThis)
				{
					neighbour.CameFrom = evaluationNode;
					neighbour.DistanceToThis = newDistance;
					if (!exploreSet.Contains(neighbour))
					{
						exploreSet.Add(neighbour);
					}
				}
			}
		}

		var finalPathLength = (finalPath.Count - 1);
		return finalPathLength;
	}

	private int ElevationFromChar(char c)
	{
		if (c == 'S') { c = 'a'; }
		if (c == 'E') { c = 'z'; }

		return c - 97;
	}
}

internal class PathNode
{
	public int X { get; }

	public int Y { get; }

	public int DistanceToThis { get; set; } = int.MaxValue;

	public int Elevation { get; }

	public PathNode? CameFrom { get; set; }

	public PathNode(int x, int y, int elevation)
	{
		X = x;
		Y = y;
		Elevation = elevation;
	}

	public int Distance(PathNode other)
	{
		return other.Elevation - Elevation;
	}

	public bool SameAs(PathNode other)
	{
		return X == other.X && Y == other.Y;
	}

	public override string ToString()
	{
		return $"({X}, {Y}) {Elevation}";
	}
}
