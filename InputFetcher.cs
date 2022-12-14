using System.Net;

namespace AoC2022;

public static class InputFetcher
{
	public static async Task<IList<string>> GetAllInputLinesAsync(int day)
	{
		Uri uri = new Uri($"https://adventofcode.com/2022/day/{day}/input");
		var cookieContainer = new CookieContainer();
		using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
		using (var client = new HttpClient(handler))
		{
			cookieContainer.Add(uri, new Cookie(
				"session",
				Secret.SessionKey));
			string result = await client.GetStringAsync(uri);

			return result.Split('\n').ToList();
		}
	}
}
