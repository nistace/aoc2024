namespace AdventOfCode2024.Scripts.Days;

public class Day22 : AbstractDay {
   protected override int Day => 22;

   private const int Prune = 0xffffff;

   public override string Part1() {
      ParseInput(out var numbers);
      var sum = numbers.Sum(number => Enumerable.Range(0, 2000).Aggregate(number, (current, _) => GetNextSecret(current)));
      return $"{sum}";
   }

   public override string Part2() {
      ParseInput(out var numbers);
      var sellValuePerSequence = numbers.ToDictionary(t => t, GetSellValuesPerSequenceHash);
      var distinctSequenceHashes = sellValuePerSequence.SelectMany(t => t.Value.Keys).Distinct();
      var bestSellValue = distinctSequenceHashes.Select(sequenceHash => sellValuePerSequence.Values.Aggregate(0L, (current, sellValues) => current + sellValues.GetValueOrDefault(sequenceHash))).Max();
      return $"{bestSellValue}";
   }

   public static Dictionary<int, long> GetSellValuesPerSequenceHash(long number) {
      var sellValuesPerSequenceHash = new Dictionary<int, long>();
      var sequenceDeltas = new Queue<int>();
      var secret = number;
      var previousPrice = (int)number % 10;
      for (var i = 0; i < 2000; i++) {
         secret = GetNextSecret(secret);
         var price = (int)secret % 10;
         sequenceDeltas.Enqueue(price - previousPrice);
         if (sequenceDeltas.Count > 4) sequenceDeltas.Dequeue();
         if (sequenceDeltas.Count == 4) {
            var sequenceHash = GetSequenceHash(sequenceDeltas);
            sellValuesPerSequenceHash.TryAdd(sequenceHash, price);
         }
         previousPrice = price;
      }
      return sellValuesPerSequenceHash;
   }

   private static int GetSequenceHash(IEnumerable<int> sequence) {
      var hash = 0;
      foreach (var value in sequence) {
         hash <<= 5;
         hash += value + 10;
      }
      return hash;
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