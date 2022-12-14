namespace AoC2022;
internal class Day02 : BaseDay
{
	public override int DayNumber => 2;

	public override async Task<string> Solve1Async()
	{
		IList<string> lines = await InputFetcher.GetAllInputLinesAsync(2);

		long totalScore = 0;
		foreach (string line in lines)
		{
			if (string.IsNullOrEmpty(line))
			{
				break;
			}

			var inOutValues = line.Split(" ");
			Round round = new Round(inOutValues[0][0], inOutValues[1][0]);

			totalScore += round.GetScore();
		}

		return totalScore.ToString();
	}

	public override async Task<string> Solve2Async()
	{
		IList<string> lines = await InputFetcher.GetAllInputLinesAsync(2);

		long totalScore = 0;
		foreach (string line in lines)
		{
			if (string.IsNullOrEmpty(line))
			{
				break;
			}

			var inOutValues = line.Split(" ");
			Round round = new Round(inOutValues[0][0], inOutValues[1][0]);

			totalScore += round.GetScore2();
		}

		return totalScore.ToString();
	}
}

internal class Round
{
	private char _opponentMove;

	private char _myMove;

	private static Dictionary<char, char> _winningOutcome = new()
	{
		{ 'A', 'B' },
		{ 'B', 'C' },
		{ 'C', 'A' },
	};

	private static Dictionary<char, char> _loseOutcome = _winningOutcome.ToDictionary(x => x.Value, x=> x.Key);

	public Round(char opponentMove, char myMove)
	{
		_opponentMove = opponentMove;
		_myMove = myMove;
	}

	public long GetScore()
	{
		var winningMove = _winningOutcome[_opponentMove];
		var myMove = MyMoveNormalized;

		if (myMove == winningMove)
		{
			return 6 + GetScoreInternal(myMove);// Win
		}

		if (myMove == _opponentMove)
		{
			return 3 + GetScoreInternal(myMove); // draw
		}

		return GetScoreInternal(myMove); // Loss
	}

	public long GetScore2()
	{
		var winningMove = _winningOutcome[_opponentMove];
		var myMove = GetOutcome(_opponentMove, _myMove);

		if (myMove == winningMove)
		{
			return 6 + GetScoreInternal(myMove);// Win
		}

		if (myMove == _opponentMove)
		{
			return 3 + GetScoreInternal(myMove); // draw
		}

		return GetScoreInternal(myMove); // Loss
	}

	private long GetScoreInternal(char move)
	{
		switch (move)
		{
			case 'A':
				return 1;
			case 'B':
				return 2;
			case 'C':
				return 3;
			default:
				throw new NotImplementedException();
		}
	}

	private char GetOutcome(char input, char expectedOutcome)
	{
		switch (expectedOutcome)
		{
			case 'X': // Lose
				return _loseOutcome[input];
			case 'Y': // Draw
				return input;
			case 'Z': // Win
				return _winningOutcome[input];
			default:
				throw new NotImplementedException();
		}
	}

	private char MyMoveNormalized
	{
		get
		{
			switch (_myMove)
			{
				case 'X':
					return 'A';
				case 'Y':
					return 'B';
				case 'Z':
					return 'C';
				default:
					throw new NotImplementedException();
			}
		}
	}
}
