namespace AoC2022;

internal class Day06 : BaseDay
{
	public override int DayNumber => 6;

	public override async Task<string> Solve1Async()
	{
		var lines = await InputFetcher.GetAllInputLinesAsync(6);
		var line = lines.First();
		//var line = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg";

		return GetStartOfMessage(4, line).ToString();
	}

	public override async Task<string> Solve2Async()
	{
		var lines = await InputFetcher.GetAllInputLinesAsync(6);
		var line = lines.First();
		//var line = "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg";

		return GetStartOfMessage(14, line).ToString();
	}

	private long GetStartOfMessage(long bufferSize, string message)
	{
		Queue<char> buffer = new Queue<char>();

		for (int i = 0; i < message.Length; i++)
		{
			if (buffer.Count == bufferSize)
			{
				buffer.Dequeue();
			}

			buffer.Enqueue(message[i]);

			if (buffer.Distinct().Count() == bufferSize)
			{
				return i + 1;
			}
		}

		return -1;
	}
}

