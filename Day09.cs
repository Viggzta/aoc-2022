namespace AoC2022;

internal class Day09 : BaseDay
{
	public override int DayNumber => 9;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(9)).Where(x => !string.IsNullOrEmpty(x));
		//var lines = new List<string>
		//{
		//	"R 4",
		//	"U 4",
		//	"L 3",
		//	"D 1",
		//	"R 4",
		//	"D 1",
		//	"L 5",
		//	"R 2",
		//};

		Coordinate head = new(0, 0);
		Coordinate tail = new(0, 0);

		List<Coordinate> tailPositions = new List<Coordinate>();

		foreach (var line in lines)
		{
			var instruction = line.Split(" ");
			var dir = GetDirection(instruction[0]);
			int moves = int.Parse(instruction[1]);

			for (int i = 0; i < moves; i++)
			{
				int headPrevX = head.X;
				int headPrevY = head.Y;
				head.Add(dir.X, dir.Y);

				int distX = GetDistance(head.X, tail.X);
				int distY = GetDistance(head.Y, tail.Y);
				bool moveDiag = distX + distY > 2;

				if (distX > 1 || distY > 1 || moveDiag)
				{
					tail.X = headPrevX;
					tail.Y = headPrevY;
				}
				tailPositions.Add(new Coordinate(tail.X, tail.Y));
			}
		}

		return tailPositions.Distinct().Count().ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(9)).Where(x => !string.IsNullOrEmpty(x));
		//var lines = new List<string>
		//{
		//	"R 5",
		//	"U 8",
		//	"L 8",
		//	"D 3",
		//	"R 17",
		//	"D 10",
		//	"L 25",
		//	"U 20",
		//};

		Coordinate head = new(0, 0);
		Coordinate tail = new(0, 0);
		List<Coordinate> betweens = new List<Coordinate>();
		for (int i = 0; i < 8; i++)
		{
			betweens.Add(new Coordinate(0, 0));
		}

		List<Coordinate> tailPositions = new List<Coordinate>();
		tailPositions.Add(new Coordinate(0, 0));

		foreach (var line in lines)
		{
			var instruction = line.Split(" ");
			var dir = GetDirection(instruction[0]);
			int moves = int.Parse(instruction[1]);

			for (int i = 0; i < moves; i++)
			{
				head.Add(dir.X, dir.Y);

				Coordinate prev = head;
				foreach (Coordinate c in betweens)
				{
					c.Follow(prev);
					prev = c;
				}

				tail.Follow(betweens.Last(), tailPositions);
			}
		}

		return tailPositions.Distinct().Count().ToString();
	}

	private int GetDistance(int a, int b)
	{
		return Math.Abs(b - a);
	}

	private (int X, int Y) GetDirection(string dir)
	{
		switch (dir)
		{
			case "U":
				return (0, -1);
			case "D":
				return (0, 1);
			case "L":
				return (-1, 0);
			case "R":
				return (1, 0);
		}

		throw new NotImplementedException();
	}
}

class Coordinate : IEquatable<Coordinate>
{
	public int X { get; set; }
	public int Y { get; set; }

	public Coordinate(int x, int y)
	{
		X = x;
		Y = y;
	}

	public void Add(int x, int y)
	{
		X += x;
		Y += y;
	}

	public void Follow(Coordinate other, List<Coordinate> passedNodes = null)
	{
		var xDist = XDistance(other, out var dirX);
		var yDist = YDistance(other, out var dirY);

		if (passedNodes != null)
		{
			while (xDist > 1 || yDist > 1)
			{
				X += -dirX;
				Y += -dirY;

				xDist = XDistance(other, out dirX);
				yDist = YDistance(other, out dirY);
				passedNodes.Add(new Coordinate(X, Y));
			}
			return;
		}

		if (xDist <= 1 && yDist <= 1)
		{
			return;
		}

		if (xDist == yDist)
		{
			X = other.X + dirX;
			Y = other.Y + dirY;
		}
		else if (xDist > yDist)
		{
			X = other.X + dirX;
			Y = other.Y;
		}
		else if (xDist < yDist)
		{
			X = other.X;
			Y = other.Y + dirY;
		}
	}

	private int LargestDistance(Coordinate other, out (int x, int y) dir)
	{
		var xDist = X - other.X;
		var yDist = Y - other.Y;
		var xDist2 = Math.Abs(xDist);
		var yDist2 = Math.Abs(yDist);

		var dirX = xDist != 0 ? xDist / xDist2 : 0;
		var dirY = yDist != 0 ? yDist / yDist2 : 0;

		dir = (dirX, dirY);

		return Math.Max(xDist2, yDist2);
	}

	private int XDistance(Coordinate other, out int dir)
	{
		var xDist = X - other.X;
		var xDist2 = Math.Abs(xDist);
		var dirX = xDist != 0 ? xDist / xDist2 : 0;
		dir = dirX;
		return xDist2;
	}

	private int YDistance(Coordinate other, out int dir)
	{
		var yDist = Y - other.Y;
		var yDist2 = Math.Abs(yDist);
		var dirY = yDist != 0 ? yDist / yDist2 : 0;
		dir = dirY;
		return yDist2;
	}

	public override bool Equals(object? obj)
	{
		if (obj is not Coordinate other)
		{
			return false;
		}

		return X == other.X && Y == other.Y;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(X, Y);
	}

	public bool Equals(Coordinate? other)
	{
		if (other == null)
		{
			return false;
		}

		return X == other.X && Y == other.Y;
	}
}
