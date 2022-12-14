namespace AoC2022;

internal class Day05 : BaseDay
{
	public override int DayNumber => 5;

	public override async Task<string> Solve1Async()
	{
		var list = await InputFetcher.GetAllInputLinesAsync(5);
		//var list = new List<string>
		//{
		//	"    [D]    ",
		//	"[N] [C]    ",
		//	"[Z] [M] [P]",
		//	" 1   2   3 ",
		//	"",
		//	"move 1 from 2 to 1",
		//	"move 3 from 1 to 3",
		//	"move 2 from 2 to 1",
		//	"move 1 from 1 to 2",
		//};

		var boxesEnd = list.IndexOf("");

		var boxesList = list.Take(boxesEnd).ToList();
		IDictionary<int, Stack<char>> boxes = ParseBoxes(boxesList);

		var moves = list.Skip(boxesEnd).Where(x => !string.IsNullOrEmpty(x)).ToList();

		foreach (var moveString in moves)
		{
			var values = moveString
				.Split(" ")
				.Where(x => int.TryParse(x, out _))
				.Select(x => int.Parse(x))
				.ToList();

			for (int i = 0; i < values[0]; i++)
			{
				var topBox = boxes[values[1]].Pop();
				boxes[values[2]].Push(topBox);
			}
		}

		string output = "";
		foreach (var box in boxes)
		{
			output += box.Value.Peek();
		}

		return output;
	}

	public override async Task<string> Solve2Async()
	{
		var list = await InputFetcher.GetAllInputLinesAsync(5);
		//var list = new List<string>
		//{
		//	"    [D]    ",
		//	"[N] [C]    ",
		//	"[Z] [M] [P]",
		//	" 1   2   3 ",
		//	"",
		//	"move 1 from 2 to 1",
		//	"move 3 from 1 to 3",
		//	"move 2 from 2 to 1",
		//	"move 1 from 1 to 2",
		//};

		var boxesEnd = list.IndexOf("");

		var boxesList = list.Take(boxesEnd).ToList();
		IDictionary<int, Stack<char>> boxes = ParseBoxes(boxesList);

		var moves = list.Skip(boxesEnd).Where(x => !string.IsNullOrEmpty(x)).ToList();

		foreach (var moveString in moves)
		{
			var values = moveString
				.Split(" ")
				.Where(x => int.TryParse(x, out _))
				.Select(x => int.Parse(x))
				.ToList();

			List<char> crane = new List<char>();

			for (int i = 0; i < values[0]; i++)
			{
				crane.Add(boxes[values[1]].Pop());
			}

			crane.Reverse();

			foreach (var item in crane)
			{
				boxes[values[2]].Push(item);
			}
		}

		string output = "";
		foreach (var box in boxes)
		{
			output += box.Value.Peek();
		}


		return output;
	}

	private IDictionary<int, Stack<char>> ParseBoxes(IList<string> boxesList)
	{
		var boxes = new Dictionary<int, Stack<char>>();

		foreach (string numberString in boxesList.Last().Split(" ").Where(x => !string.IsNullOrEmpty(x)))
		{
			var number = int.Parse(numberString);
			boxes.Add(number, new Stack<char>());
		}

		foreach (string boxRow in boxesList.Take(boxesList.Count - 1).Reverse())
		{
			foreach (var boxStack in boxes)
			{
				var boxLocation = 1 + 4 * (boxStack.Key - 1);
				var boxChar = boxRow[boxLocation];

				if (boxChar != ' ')
				{
					boxStack.Value.Push(boxChar);
				}
			}
		}

		return boxes;
	}
}

