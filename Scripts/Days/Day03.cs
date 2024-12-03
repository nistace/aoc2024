using System.Text.RegularExpressions;

namespace AdventOfCode2024.Scripts.Days;

public class Day03 : AbstractDay {
	protected override int Day => 3;
	private readonly Regex mulRegex = new(@"mul\((\d+),(\d+)\)");

	public override string Part1() => $"{SumAllMul(GetInputText())}";

	private int SumAllMul(string input) => mulRegex.Matches(input).Sum(t => int.Parse(t.Groups[1].Value) * int.Parse(t.Groups[2].Value));

	public override string Part2() {
		var sum = 0;
		var subInput = GetInputText();
		while (subInput.Contains("don't()"))
		{
			var indexOfNextDont = subInput.IndexOf("don't()", StringComparison.Ordinal);
			sum += SumAllMul(subInput[..indexOfNextDont]);
			subInput = subInput[indexOfNextDont..];
			subInput = subInput.Contains("do()") ? subInput[subInput.IndexOf("do()", StringComparison.Ordinal)..] : string.Empty;
		}
		sum += SumAllMul(subInput);
		return $"{sum}";
	}
}