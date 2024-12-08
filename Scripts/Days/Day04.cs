using System.Text.RegularExpressions;

namespace AdventOfCode2024.Scripts.Days;

public class Day04 : AbstractDay {
   protected override int Day => 4;
   protected Regex xmasRegex = new("XMAS");
   protected Regex masAsAnXRegex = new("(MMASS|MSAMS|SSAMM|SMASM)");

   public override string Part1() {
      var input = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Select(u => $"{u}").ToList()).ToList();
      var linesInAllDirections = new List<string>();
      linesInAllDirections.AddRange(input.Select(t => string.Concat(t)));
      linesInAllDirections.AddRange(Enumerable.Range(0, input[0].Count).Select(y => string.Concat(input.Select(line => line[y]))));
      linesInAllDirections.AddRange(Enumerable.Range(-input[0].Count, 2 * input[0].Count).Select(y => string.Concat(input.Select((line, x) => line.ElementAtOrDefault(x + y)))));
      linesInAllDirections.AddRange(Enumerable.Range(0, 2 * input[0].Count).Select(y => string.Concat(input.Select((line, x) => line.ElementAtOrDefault(y - x)))));
      linesInAllDirections.AddRange(linesInAllDirections.ToArray().Select(t => new string(t.Reverse().ToArray())));

      return $"{xmasRegex.Matches(string.Join(" ", linesInAllDirections)).Count}";
   }

   public override string Part2() {
      var input = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Select(u => $"{u}").ToArray()).ToArray();
      return $"{Enumerable.Range(0, input.Length - 2).Sum(x => Enumerable.Range(0, input[0].Length - 2).Count(y => CheckMasInXShape(input, y, x)))}";
   }

   private bool CheckMasInXShape(string[][] input, int y, int x) => masAsAnXRegex.Match(string.Concat(input[y][x], input[y][x + 2], input[y + 1][x + 1], input[y + 2][x], input[y + 2][x + 2])).Success;
}