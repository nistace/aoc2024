namespace AdventOfCode2024.Scripts.Days;

public class Day22 : AbstractDay {
   protected override int Day => 22;

   private const long Prune = 0xffffff;

   public override string Part1() {
      ParseInput(out var numbers);
      var sum = numbers.Sum(number => Enumerable.Range(0, 2000).Aggregate(number, (current, _) => GetNextSecret(current)));
      return $"{sum}";
   }

   public override string Part2() {
      return "";
   }

   private static long GetNextSecret(long secret) {
      secret = (secret ^ (secret << 6)) & Prune;
      secret = (secret ^ (secret >> 5)) & Prune;
      secret = (secret ^ (secret << 11)) & Prune;
      return secret;
   }

   private void ParseInput(out long[] numbers) {
      numbers = GetInputLines().Where(t => !string.IsNullOrEmpty(t)).Select(long.Parse).ToArray();
   }
}