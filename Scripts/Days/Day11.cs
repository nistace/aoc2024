using System.Numerics;

namespace AdventOfCode2024.Scripts.Days;

public class Day11 : AbstractDay {
   protected override int Day => 11;
   protected Dictionary<(BigInteger value, int blinks), long> CachedSplits { get; } = [];
   protected Dictionary<BigInteger, BigInteger[]> CachedNextValues { get; } = [];

   public override string Part1() => $"{Simulate(25)}";
   public override string Part2() => $"{Simulate(75)}";

   private long CountSubdivisions(BigInteger fromValue, int blinks) {
      if (blinks == 0) return 1;
      if (!CachedSplits.ContainsKey((fromValue, blinks))) {
         if (!CachedNextValues.ContainsKey(fromValue)) {
            var fromValueStr = $"{fromValue}";
            if (fromValueStr == "0") CachedNextValues.Add(fromValue, [1]);
            else if (fromValueStr.Length % 2 == 0) CachedNextValues.Add(fromValue, [long.Parse(fromValueStr[..(fromValueStr.Length / 2)]), long.Parse(fromValueStr[(fromValueStr.Length / 2)..])]);
            else CachedNextValues.Add(fromValue, [fromValue * 2024]);
         }

         CachedSplits.Add((fromValue, blinks), CachedNextValues[fromValue].Sum(t => CountSubdivisions(t, blinks - 1)));
      }

      return CachedSplits[(fromValue, blinks)];
   }

   private long Simulate(int blinks) {
      ParseInput(out var initialValues);

      for (var subBlinkCount = 1; subBlinkCount < blinks; ++subBlinkCount) {
         initialValues.ForEach(t => CountSubdivisions(t, subBlinkCount));
      }

      return initialValues.Sum(t => CountSubdivisions(t, blinks));
   }

   private void ParseInput(out List<long> initialValues) {
      initialValues = GetInputText().Trim().Split(' ').Select(long.Parse).ToList();
   }
}