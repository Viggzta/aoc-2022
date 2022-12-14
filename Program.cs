using AoC2022;

List<BaseDay> days = new()
{
	new Day01(),
	new Day02(),
	new Day03(),
	new Day04(),
	new Day05(),
	new Day06(),
	new Day07(),
	new Day08(),
	new Day09(),
	new Day10(),
	new Day11(),
	new Day12(),
	new Day13(),
	new Day14(),
};

ConsoleHelper.WriteLineColored(await PrintOutputAsync(days.Last()));

//var dayTasks = days.Select(x => PrintOutputAsync(x)).ToArray();

//Task.WaitAll(dayTasks);

//foreach (var task in dayTasks)
//{
//	ConsoleHelper.WriteLineColored(await task);
//}

async Task<string> PrintOutputAsync(BaseDay day)
{
	var ans1 = await day.Solve1Async();
	var ans2 = await day.Solve2Async();
	return $"The answer for day ~?g{day.DayNumber}~ is ~?y{ans1}~ and ~?y{ans2}~.";
}
