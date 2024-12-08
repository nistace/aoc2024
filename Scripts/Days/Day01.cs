using System.Text.RegularExpressions;

namespace AdventOfCode2024.Scripts.Days;

public class Day01 : AbstractDay {
   protected override int Day => 1;
   private readonly Regex inputRegex = new(@"^(\d+) +(\d+)$");

   public override string Part1() {
      var input = GetInputLines().Select(t => inputRegex.Match(t)).Where(t => t.Success).Select(t => (int.Parse(t.Groups[1].Value), int.Parse(t.Groups[2].Value))).ToArray();
      var left = input.Select(t => t.Item1).OrderBy(t => t).ToArray();
      var right = input.Select(t => t.Item2).OrderBy(t => t).ToArray();
      return $"{Enumerable.Range(0, left.Length).Select(i => Math.Abs(left[i] - right[i])).Sum()}";
   }

   public override string Part2() {
      var input = GetInputLines().Select(t => inputRegex.Match(t)).Where(t => t.Success).Select(t => (int.Parse(t.Groups[1].Value), int.Parse(t.Groups[2].Value))).ToArray();
      var left = input.Select(t => t.Item1).ToArray();
      var right = input.Select(t => t.Item2).Distinct().ToDictionary(t => t, t => input.Count(u => t == u.Item2));
      return $"{left.Sum(t => t * right.GetValueOrDefault(t, 0))}";
   }
}