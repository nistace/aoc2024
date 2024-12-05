namespace AdventOfCode2024.Scripts.Days;

public class Day05 : AbstractDay
{
	protected override int Day => 5;
	public override string Part1()
	{
		ParseInput(out var rules, out var updates);

		return $"{updates.Where(t => CheckRules(t, rules)).Sum(t => t.Skip(t.Length / 2).First())}";
	}
	private static bool CheckRules(int[] updatePages, Dictionary<int, HashSet<int>> rules) =>
		!updatePages.Where((value, index) => rules.ContainsKey(value) && rules[value].Intersect(updatePages.Take(index)).Any()).Any();

	public override string Part2() => "";

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
