using System.Diagnostics;
using AdventOfCode2024.Scripts.Days;

namespace AdventOfCode2024.Scripts;

public static class Program {
   public static void Main() {
      var day = new Day21();
      Console.WriteLine($"{day.GetType().Name}:");
      Stopwatch stopwatch = new();
      stopwatch.Start();
      Console.WriteLine($"PART 1:\t{day.Part1()}\n{stopwatch.ElapsedMilliseconds} ms");
      stopwatch.Restart();
      Console.WriteLine($"PART 2:\t{day.Part2()}\n{stopwatch.ElapsedMilliseconds} ms");
      stopwatch.Stop();
   }
}