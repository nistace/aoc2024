using System.Text.RegularExpressions;

namespace AdventOfCode2024.Scripts.Days;

public class Day03 : AbstractDay {
	protected override int Day => 3;
	private readonly Regex mulRegex = new(@"mul\((\d+),(\d+)\)");

	public override string Part1() {
		return $"{mulRegex.Matches(GetInputText()).Sum(t => int.Parse(t.Groups[1].Value) * int.Parse(t.Groups[2].Value))}";
	}

	public override string Part2() {
		return "";
	}
}