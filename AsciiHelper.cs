using System.Text;

namespace AoC2022;

internal static class AsciiHelper
{

	public static string ToString(List<string> lines)
	{
		List<string> inputLetters = new List<string>();
		int letterWidth = 4;
		int amountOfLetters = ((lines.First().Length + 1) / (letterWidth + 1));


		for (int i = 0; i < amountOfLetters; i++)
		{
			inputLetters.Add("");
		}

		foreach (string line in lines)
		{
			for (int i = 0; i < amountOfLetters; i++)
			{
				inputLetters[i] += line.Substring(i * (letterWidth + 1), letterWidth);
			}
		}
		StringBuilder stringBuilder = new();

		foreach (string letter in inputLetters)
		{
			stringBuilder.Append(letterDictionary[letter]);
		}

		return stringBuilder.ToString();
	}

	private static Dictionary<string, char> letterDictionary = new Dictionary<string, char>
	{
		{
			" XX " +
			"X  X" +
			"X  X" +
			"XXXX" +
			"X  X" +
			"X  X"
			,
			'A'
		},

		{
			"XXX " +
			"X  X" +
			"XXX " +
			"X  X" +
			"X  X" +
			"XXX "
			,
			'B'
		},

		{
			" XX " +
			"X  X" +
			"X   " +
			"X   " +
			"X  X" +
			" XX "
			,
			'C'
		},

		{
			"XXX " +
			"X  X" +
			"X  X" +
			"X  X" +
			"X  X" +
			"XXX "
			,
			'D'
		},

		{
			"XXXX" +
			"X   " +
			"XXX " +
			"X   " +
			"X   " +
			"XXXX"
			,
			'E'
		},

		{
			"XXXX" +
			"X   " +
			"XXX " +
			"X   " +
			"X   " +
			"X   "
			,
			'F'
		},

		{
			" XX " +
			"X  X" +
			"X   " +
			"X XX" +
			"X  X" +
			" XXX"
			,
			'G'
		},

		{
			"X  X" +
			"X  X" +
			"XXXX" +
			"X  X" +
			"X  X" +
			"X  X"
			,
			'H'
		},

		{ // Unsure
			"XXXX" +
			" XX " +
			" XX " +
			" XX " +
			" XX " +
			"XXXX"
			,
			'I'
		},

		{
			"  XX" +
			"   X" +
			"   X" +
			"   X" +
			"X  X" +
			" XX "
			,
			'J'
		},

		{
			"X  X" +
			"X X " +
			"XX  " +
			"X X " +
			"X X " +
			"X  X"
			,
			'K'
		},

		{
			"X   " +
			"X   " +
			"X   " +
			"X   " +
			"X   " +
			"XXXX"
			,
			'L'
		},

		{
			"X  X" +
			"XXXX" +
			"X  X" +
			"X  X" +
			"X  X" +
			"X  X"
			,
			'M'
		},

		{
			"X  X" +
			"XX X" +
			"XX X" +
			"X XX" +
			"X XX" +
			"X  X"
			,
			'N'
		},

		{
			" XX " +
			"X  X" +
			"X  X" +
			"X  X" +
			"X  X" +
			" XX "
			,
			'O'
		},

		{
			"XXX " +
			"X  X" +
			"X  X" +
			"XXX " +
			"X   " +
			"X   "
			,
			'P'
		},

		{
			" XX " +
			"X  X" +
			"X  X" +
			"X  X" +
			"X XX" +
			" X X"
			,
			'Q'
		},

		{
			"XXX " +
			"X  X" +
			"X  X" +
			"XXX " +
			"X X " +
			"X  X"
			,
			'R'
		},

		{
			" XX " +
			"X  X" +
			"X   " +
			" XX " +
			"   X" +
			"XXX "
			,
			'S'
		},

		{
			"X  X" +
			"X  X" +
			"X  X" +
			"X  X" +
			"X  X" +
			" XX "
			,
			'U'
		},

		{
			"XXXX" +
			"   X" +
			"  X " +
			" X  " +
			"X   " +
			"XXXX"
			,
			'Z'
		},

		{
			"    " +
			"    " +
			"    " +
			"    " +
			"    " +
			"    "
			,
			' '
		},
	};
}

