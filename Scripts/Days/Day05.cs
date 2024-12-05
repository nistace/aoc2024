namespace AdventOfCode2024.Scripts.Days;

public class Day05 : AbstractDay
{
	protected override int Day => 5;
	public override string Part1()
	{
		ParseInput(out var rules, out var updates);
		return $"{updates.Where(t => CheckRules(t, rules)).Sum(t => t.Skip(t.Length / 2).First())}";
	}

	public override string Part2()
	{
		ParseInput(out var rules, out var updates);
		return $"{updates.Where(t => !CheckRules(t, rules)).Select(t => FixWithRules(t, rules)).Sum(t => t.Skip(t.Length / 2).First())}";
	}

	private static bool CheckRules(int[] updatePages, Dictionary<int, HashSet<int>> rules) =>
		!updatePages.Where((value, index) => rules.ContainsKey(value) && rules[value].Intersect(updatePages.Take(index)).Any()).Any();

	private static int[] FixWithRules(int[] updatePages, Dictionary<int, HashSet<int>> rules)
	{
		var ordered = new List<int>();
		var remainingPages = new HashSet<int>(updatePages);

		while (remainingPages.Count != 0)
		{
			var nextItem = remainingPages.First(t => !rules.Any(r => remainingPages.Contains(r.Key) && r.Value.Contains(t)));
			remainingPages.Remove(nextItem);
			ordered.Add(nextItem);
		}

		return ordered.ToArray();
	}

	private void ParseInput(out Dictionary<int, HashSet<int>> rules, out IReadOnlyList<int[]> updates)
	{
		var input = GetInputLines();

		var rulesInput = input.TakeWhile(t => !string.IsNullOrWhiteSpace(t)).ToArray();
		rules = new Dictionary<int, HashSet<int>>();
		foreach (var rule in rulesInput.Select(t => (before: int.Parse(t.Split("|")[0]), after: int.Parse(t.Split("|")[1]))))
		{
			rules.TryAdd(rule.before, []);
			rules[rule.before].Add(rule.after);
		}

		updates = input.Skip(rulesInput.Length).Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Split(",").Select(int.Parse).ToArray()).ToArray();
	}

}
