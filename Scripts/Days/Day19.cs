namespace AdventOfCode2024.Scripts.Days;

public class Day19 : AbstractDay
{
	protected override int Day => 19;

	public override string Part1()
	{
		ParseInput(out var patterns, out var desiredDesigns);
		return $"{CountDoableDesigns(desiredDesigns, patterns)}";
	}

	public override string Part2()
	{
		ParseInput(out var patterns, out var desiredDesigns);
		return $"{CountDoableDesignsDifferentSolutions(desiredDesigns, patterns)}";
	}

	private static int CountDoableDesigns(List<string> desiredDesigns, HashSet<string> patterns)
	{
		var impossibleDesigns = new HashSet<string>();
		return desiredDesigns.Count(t => IsDesignPossible(t, patterns, impossibleDesigns));
	}

	private static long CountDoableDesignsDifferentSolutions(List<string> desiredDesigns, HashSet<string> patterns)
	{
		var impossibleDesigns = new HashSet<string>();
		var solutionsPerDesign = new Dictionary<string, long>();
		return desiredDesigns.Sum(t => CountDifferentSolutions(t, patterns, solutionsPerDesign, impossibleDesigns));

	}

	private static bool IsDesignPossible(string design, IReadOnlySet<string> patterns, HashSet<string> impossibleDesigns)
	{
		if (patterns.Contains(design)) return true;
		if (impossibleDesigns.Contains(design)) return false;

		if (patterns.Where(design.StartsWith).Any(possibleStart => IsDesignPossible(design[possibleStart.Length..], patterns, impossibleDesigns)))
		{
			return true;
		}

		impossibleDesigns.Add(design);
		return false;
	}

	private static long CountDifferentSolutions(string design, IReadOnlySet<string> patterns, Dictionary<string, long> solutionsPerDesign, HashSet<string> impossibleDesigns)
	{
		if (design == "") return 1;
		if (solutionsPerDesign.TryGetValue(design, out var solution)) return solution;
		if (impossibleDesigns.Contains(design)) return 0;

		var solutionCount = patterns.Where(design.StartsWith).Sum(start => CountDifferentSolutions(design[start.Length..], patterns, solutionsPerDesign, impossibleDesigns));

		if (solutionCount == 0) impossibleDesigns.Add(design);
		else solutionsPerDesign[design] = solutionCount;
		return solutionCount;
	}

	private void ParseInput(out HashSet<string> patterns, out List<string> desiredDesigns)
	{
		var inputLines = GetInputLines();
		patterns = inputLines[0].Split(",").Select(t => t.Trim()).ToHashSet();
		desiredDesigns = inputLines.Skip(1).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
	}
}
