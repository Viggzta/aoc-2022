namespace AoC2022;

internal class Day07 : BaseDay
{
	public override int DayNumber => 7;

	public override async Task<string> Solve1Async()
	{
		var list = (await InputFetcher.GetAllInputLinesAsync(7)).Where(x => !string.IsNullOrEmpty(x)).ToList();

		var (rootNode, directories) = GetFileSystem(list);

		long result = directories.Where(x => x.Size <= 100000).Sum(x => x.Size);
		return result.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var list = (await InputFetcher.GetAllInputLinesAsync(7)).Where(x => !string.IsNullOrEmpty(x)).ToList();

		var (rootNode, directories) = GetFileSystem(list);

		var availableDiskSpace = 70000000 - rootNode.Size;
		var cleanupRequired = 30000000 - availableDiskSpace;

		long result = directories.Where(x => x.Size >= cleanupRequired).OrderBy(x => x.Size).First().Size;
		return result.ToString();
	}

	private (Node rootNode, List<Node> directories) GetFileSystem(List<string> list)
	{
		Node currentNode = new Node(null, "/");
		Node rootNode = currentNode;

		List<Node> directories = new List<Node>();
		directories.Add(currentNode);

		foreach (var line in list)
		{
			var data = line.Split(" ");
			// $ CD
			if (data[0] == "$")
			{
				var currentCommand = data[1];

				if (currentCommand == "cd")
				{
					var data2 = data[2];
					if (data2 == "/")
					{
						continue; // This is only done initially
					}
					if (data2 == "..")
					{
						currentNode = currentNode.Parent;
						continue;
					}
					var isNew = currentNode.AddIfNotExist(new Node(currentNode, data2), out var newNode);
					currentNode = newNode;
					if (isNew)
					{
						directories.Add(newNode);
					}
				}

				continue;
			}

			// $ LS
			if (data[0] == "dir")
			{
				var isNew = currentNode.AddIfNotExist(new Node(currentNode, data[1]), out var newNode);
				if (isNew)
				{
					directories.Add(newNode);
				}
			}
			else
			{
				currentNode.AddIfNotExist(new FileNode(currentNode, data[1], long.Parse(data[0])), out _);
			}
		}

		return (rootNode, directories);
	}
}

internal class Node
{
	private long? internalSize = null;

	public List<Node> Children { get; } = new List<Node>();

	public Node? Parent { get; }

	public string Name { get; }

	public virtual long Size
	{
		get
		{
			if (internalSize != null)
			{
				return internalSize.Value;
			}

			internalSize = Children.Sum(x => x.Size);
			return internalSize.Value;
		}
	}

	public Node(Node? parent, string name)
	{
		Parent = parent;
		Name = name;
	}

	public bool AddIfNotExist(Node n, out Node resultNode)
	{
		bool isNew = false;

		var childNode = Children.Where(x => x.Name == n.Name).FirstOrDefault();
		if (childNode == null)
		{
			isNew = true;
			childNode = n;
			Children.Add(childNode);
		}

		resultNode = childNode;
		return isNew;
	}
}

internal class FileNode : Node
{
	private long _size;

	public override long Size => _size;

	public FileNode(Node? parent, string name, long size) : base(parent, name)
	{
		_size = size;
	}
}

