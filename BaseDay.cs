namespace AoC2022;

public abstract class BaseDay
{
	public abstract int DayNumber { get; }

	public abstract Task<string> Solve1Async();

	public abstract Task<string> Solve2Async();
}
