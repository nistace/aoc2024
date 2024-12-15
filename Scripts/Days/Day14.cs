using System.Text.RegularExpressions;
using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day14 : AbstractDay {
   protected override int Day => 14;
   protected Regex inputLineRegex = new(@"p=(\-?\d+),(\-?\d+) v=(\-?\d+),(\-?\d+)");

   public override string Part1() {
      ParseInput(out var robotInfos);

      var dimensions = new Vector2Int(101, 103);
      const int seconds = 100;

      var robotFinalPositions = robotInfos.Select(t => t.Position + (t.Velocity * seconds))
         .Select(t => new Vector2Int((t.X % dimensions.X + dimensions.X) % dimensions.X, (t.Y % dimensions.Y + dimensions.Y) % dimensions.Y)).ToArray();

      var countPerQuadrant = new[] {
         robotFinalPositions.Count(t => t.X < dimensions.X / 2f - 1 && t.Y < dimensions.Y / 2f - 1),
         robotFinalPositions.Count(t => t.X > dimensions.X / 2f && t.Y < dimensions.Y / 2f - 1),
         robotFinalPositions.Count(t => t.X < dimensions.X / 2f - 1 && t.Y > dimensions.Y / 2f),
         robotFinalPositions.Count(t => t.X > dimensions.X / 2f && t.Y > dimensions.Y / 2f)
      };

      return $"{countPerQuadrant.Aggregate(1L, (current, quadrant) => current * quadrant)}";
   }

   public override string Part2() {
      return "";
   }

   private void ParseInput(out IReadOnlyList<RobotInfo> robotInfos) {
      robotInfos = GetInputLines().Select(t => inputLineRegex.Match(t)).Where(t => t.Success).Select(t =>
         new RobotInfo((int.Parse(t.Groups[1].Value), int.Parse(t.Groups[2].Value)), (int.Parse(t.Groups[3].Value), int.Parse(t.Groups[4].Value)))).ToArray();
   }

   // ReSharper disable once UnusedMember.Local
   private static void Debug(IReadOnlyList<Vector2Int> robotFinalPositions, Vector2Int dimensions) {
      for (var y = 0; y < dimensions.Y; y++) {
         for (var x = 0; x < dimensions.X; x++) {
            var robotCount = robotFinalPositions.Count(t => t.X == x && t.Y == y);
            Console.Write(robotCount == 0 ? "." : robotCount > 10 ? "+" : $"{robotCount}");
         }
         Console.WriteLine();
      }
   }

   private readonly struct RobotInfo(Vector2Int position, Vector2Int velocity) {
      public Vector2Int Position { get; } = position;
      public Vector2Int Velocity { get; } = velocity;
   }
}