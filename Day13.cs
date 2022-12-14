namespace AoC2022;

internal class Day13 : BaseDay
{
	public override int DayNumber => 13;

	public override async Task<string> Solve1Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(13)).Chunk(3);
		//var lines = new List<string>
		//{
		//	"[1,1,3,1,1]",
		//	"[1,1,5,1,1]",
		//	"",
		//	"[[1],[2,3,4]]",
		//	"[[1],4]",
		//	"",
		//	"[9]",
		//	"[[8,7,6]]",
		//	"",
		//	"[[4,4],4,4]",
		//	"[[4,4],4,4,4]",
		//	"",
		//	"[7,7,7,7]",
		//	"[7,7,7]",
		//	"",
		//	"[]",
		//	"[3]",
		//	"",
		//	"[[[]]]",
		//	"[[]]",
		//	"",
		//	"[1,[2,[3,[4,[5,6,7]]]],8,9]",
		//	"[1,[2,[3,[4,[5,6,0]]]],8,9]",
		//	"",
		//}.Chunk(3);

		int index = 0;
		int sum = 0;

		foreach (var line in lines)
		{
			index++;

			var pair = line.Take(2).ToList();

			var left = GetNode(pair.First());
			var right = GetNode(pair.Last());

			bool? correct = Compare(left, right);
			if (correct == true)
			{
				sum += index;
			}
		}

		return sum.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = (await InputFetcher.GetAllInputLinesAsync(13)).Where(x => !string.IsNullOrEmpty(x));
		//var lines = new List<string>
		//{
		//	"[1,1,3,1,1]",
		//	"[1,1,5,1,1]",
		//	"",
		//	"[[1],[2,3,4]]",
		//	"[[1],4]",
		//	"",
		//	"[9]",
		//	"[[8,7,6]]",
		//	"",
		//	"[[4,4],4,4]",
		//	"[[4,4],4,4,4]",
		//	"",
		//	"[7,7,7,7]",
		//	"[7,7,7]",
		//	"",
		//	"[]",
		//	"[3]",
		//	"",
		//	"[[[]]]",
		//	"[[]]",
		//	"",
		//	"[1,[2,[3,[4,[5,6,7]]]],8,9]",
		//	"[1,[2,[3,[4,[5,6,0]]]],8,9]",
		//	"",
		//}.Where(x => !string.IsNullOrEmpty(x));

		List<DecodeNode> allNodes = new List<DecodeNode>();

		foreach (var line in lines)
		{
			allNodes.Add(GetNode(line));
		}

		var divPacket1 = GetNode("[[2]]");
		var divPacket2 = GetNode("[[6]]");

		allNodes.Add(divPacket1);
		allNodes.Add(divPacket2);

		for (int i = 0; i < allNodes.Count - 1; i++)
		{
			var a = allNodes[i];
			var b = allNodes[i + 1];
			if (Compare(a, b) == false)
			{
				var aTemp = a;
				var bTemp = b;

				allNodes.RemoveAt(i);
				allNodes.RemoveAt(i);

				allNodes.Insert(i, bTemp);
				allNodes.Insert(i + 1, aTemp);

				i = -1; // for loops adds 1 so we get to 0
			}
		}

		var divPacket1Index = allNodes.IndexOf(divPacket1) + 1;
		var divPacket2Index = allNodes.IndexOf(divPacket2) + 1;

		return (divPacket1Index * divPacket2Index).ToString();
	}

	private bool? Compare(DecodeNode left, DecodeNode right)
	{
		var currentLeft = left;
		var currentRight = right;

		if (currentLeft is DecodeVal l && currentRight is DecodeVal r)
		{
			if (l.Value < r.Value) return true;
			if (l.Value > r.Value) return false;
			return null;
		}

		int longestChildren = Math.Max(currentLeft.Children.Count, currentRight.Children.Count);
		for (int i = 0; i < longestChildren; i++)
		{
			if (i >= currentLeft.Children.Count)
			{
				return true;
			}

			if (i >= currentRight.Children.Count)
			{
				return false;
			}

			bool? compareChildren;
			if (currentLeft.Children[i] is not DecodeVal && currentRight.Children[i] is DecodeVal)
			{
				var temp = new DecodeNode(null);
				temp.Children.Add(currentRight.Children[i]);
				compareChildren = Compare(currentLeft.Children[i], temp);
			}
			else if (currentLeft.Children[i] is DecodeVal && currentRight.Children[i] is not DecodeVal)
			{
				var temp = new DecodeNode(null);
				temp.Children.Add(currentLeft.Children[i]);
				compareChildren = Compare(temp, currentRight.Children[i]);
			}
			else
			{
				compareChildren = Compare(currentLeft.Children[i], currentRight.Children[i]);
			}

			if (compareChildren != null)
			{
				return compareChildren;
			}
		}

		return null;
	}

	private static DecodeNode GetNode(string line)
	{
		DecodeNode? currentNode = null;
		string tempString = "";
		char prevC = ' ';
		foreach (var c in line)
		{
			if (c == ',' && prevC != ']')
			{
				currentNode.Children.Add(GetChildNode(currentNode, tempString));
				tempString = "";
			}
			else if (c == '[')
			{
				currentNode = new DecodeNode(currentNode);
				if (currentNode.Parent != null)
				{
					currentNode.Parent.Children.Add(currentNode);
				}
			}
			else if (c == ']')
			{
				if (!string.IsNullOrEmpty(tempString))
				{
					currentNode.Children.Add(GetChildNode(currentNode, tempString));
					tempString = "";
				}

				currentNode = currentNode.Parent != null ? currentNode.Parent : currentNode;
			}
			else if (c != ',')
			{
				tempString += c;
			}

			prevC = c;
		}

		if (currentNode == null)
		{
			throw new NotImplementedException();
		}

		return currentNode;
	}

	private static DecodeNode GetChildNode(DecodeNode? currentNode, string tempString)
	{
		if (string.IsNullOrEmpty(tempString))
		{
			return new DecodeNode(currentNode);
		}
		else
		{
			var value = int.Parse(tempString);
			return new DecodeVal(currentNode, value);
		}
	}
}

internal class DecodeNode
{
	public DecodeNode? Parent { get; set; }

	public List<DecodeNode> Children { get; set; }

	public DecodeNode(DecodeNode? parent)
	{
		Children = new List<DecodeNode>();
		Parent = parent;
	}

	public override string ToString()
	{
		return Children.Any() ? $"[{string.Join(",", Children)}]" : " ";
	}
}

internal class DecodeVal : DecodeNode
{
	public DecodeVal(DecodeNode? parent, int value) : base(parent)
	{
		Value = value;
	}

	public int Value { get; set; }

	public override string ToString()
	{
		return Value.ToString();
	}
}
