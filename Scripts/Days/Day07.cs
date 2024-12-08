namespace AdventOfCode2024.Scripts.Days;

public class Day07 : AbstractDay {
   protected override int Day => 7;
   private readonly HashSet<Func<long, long, long>> part1Operators = [(a, b) => a + b, (a, b) => a * b];
   private readonly HashSet<Func<long, long, long>> part2Operators = [(a, b) => long.Parse($"{a}{b}"), (a, b) => a + b, (a, b) => a * b];

   public override string Part1() => $"{ParseInput().Where(t => HasSolution(t.result, t.operands, part1Operators)).Sum(t => t.result)}";
   public override string Part2() => $"{ParseInput().Where(t => HasSolution(t.result, t.operands, part2Operators)).Sum(t => t.result)}";

   private static bool HasSolution(long result, long[] operands, HashSet<Func<long, long, long>> operators) {
      if (operands.Length == 1) return result == operands[0];
      if (operands[0] > result) return false;
      return operators.Any(o => HasSolution(result, operands.Skip(2).Prepend(o(operands[0], operands[1])).ToArray(), operators));
   }

   private (long result, long[] operands)[] ParseInput() {
      return GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Split(": ")).Select(t => (long.Parse(t[0]), t[1].Split(" ").Select(long.Parse).ToArray())).ToArray();
   }
}