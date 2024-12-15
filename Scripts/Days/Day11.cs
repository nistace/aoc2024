namespace AdventOfCode2024.Scripts.Days;

public class Day11 : AbstractDay {
   protected override int Day => 11;

   public override string Part1() {
      ParseInput(out var initialValues);
      var nextValues = new Dictionary<long, long[]> { { 0, [1] } };

      return $"{initialValues.Sum(t => CountSubdivisions(t, 25, nextValues))}";
   }

   private static int CountSubdivisions(long fromValue, int overTimes, Dictionary<long, long[]> cachedNextValues) {
      if (overTimes == 0) return 1;

      if (!cachedNextValues.ContainsKey(fromValue)) {
         var fromValueStr = "" + fromValue;
         if (fromValueStr.Length % 2 == 0)
            cachedNextValues.Add(fromValue, [long.Parse(fromValueStr[..(fromValueStr.Length / 2)]), long.Parse(fromValueStr[(fromValueStr.Length / 2)..])]);
         else cachedNextValues.Add(fromValue, [fromValue * 2024]);
      }

      return cachedNextValues[fromValue].Sum(t => CountSubdivisions(t, overTimes - 1, cachedNextValues));
   }

   private void ParseInput(out List<long> initialValues) {
      initialValues = GetInputText().Trim().Split(' ').Select(long.Parse).ToList();
   }

   public override string Part2() => "";
}