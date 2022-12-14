namespace AoC2022;

internal class ConsoleHelper
{
	public static void WriteLineColored(string input)
	{
		var lines = input.Split("~");

		foreach (var line in lines)
		{
			if (line.StartsWith("?"))
			{
				ConsoleColor? color = CharToColor(line[1]);
				PrettyWrite(line.Substring(2), color);
				continue;
			}

			Console.Write(line);
		}

		Console.WriteLine();
	}

	private static ConsoleColor? CharToColor(char c)
	{
		switch (c)
		{
			case 'g':
				return ConsoleColor.Green;
			case 'r':
				return ConsoleColor.Red;
			case 'y':
				return ConsoleColor.Yellow;
		}

		return null;
	}

	private static void PrettyWrite(string s, ConsoleColor? color = null)
	{
		if (color == null)
		{
			Console.Write(s);
			return;
		}

		var beforeColor = Console.ForegroundColor;
		Console.ForegroundColor = color.Value;
		Console.Write(s);
		Console.ForegroundColor = beforeColor;
	}
}
