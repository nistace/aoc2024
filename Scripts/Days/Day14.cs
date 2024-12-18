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

      var robotFinalPositions = SimulateRobotsMovement(robotInfos, seconds, dimensions);

      var countPerQuadrant = new[] {
         robotFinalPositions.Count(t => t.X < dimensions.X / 2f - 1 && t.Y < dimensions.Y / 2f - 1),
         robotFinalPositions.Count(t => t.X > dimensions.X / 2f && t.Y < dimensions.Y / 2f - 1),
         robotFinalPositions.Count(t => t.X < dimensions.X / 2f - 1 && t.Y > dimensions.Y / 2f),
         robotFinalPositions.Count(t => t.X > dimensions.X / 2f && t.Y > dimensions.Y / 2f)
      };

      return $"{countPerQuadrant.Aggregate(1L, (current, quadrant) => current * quadrant)}";
   }

   public override string Part2() {
      ParseInput(out var robotInfos);

      var dimensions = new Vector2Int(101, 103);

      var bestScore = -1;
      var bestSecond = -1;
      for (var second = 0; second < 20000; ++second) {
         var secondScore = GetChristmasTreeScore(SimulateRobotsMovement(robotInfos, second, dimensions));
         if (secondScore > bestScore) {
            bestScore = secondScore;
            bestSecond = second;
         }
      }

      return $"{bestSecond}";
   }

   private static int GetChristmasTreeScore(IReadOnlyList<Vector2Int> robotFinalPositions) {
      var distinctPositions = robotFinalPositions.Distinct().ToHashSet();
      return distinctPositions.Sum(coordinates => coordinates.FourNeighbours.Count(t => distinctPositions.Contains(t)));
   }

   private IReadOnlyList<Vector2Int> SimulateRobotsMovement(IReadOnlyList<RobotInfo> robotInfos, int seconds, Vector2Int dimensions) {
      return robotInfos.Select(t => t.Position + (t.Velocity * seconds))
         .Select(t => new Vector2Int((t.X % dimensions.X + dimensions.X) % dimensions.X, (t.Y % dimensions.Y + dimensions.Y) % dimensions.Y)).ToArray();
   }

   private void ParseInput(out IReadOnlyList<RobotInfo> robotInfos) {
      robotInfos = GetInputLines().Select(t => inputLineRegex.Match(t)).Where(t => t.Success).Select(t =>
         new RobotInfo((int.Parse(t.Groups[1].Value), int.Parse(t.Groups[2].Value)), (int.Parse(t.Groups[3].Value), int.Parse(t.Groups[4].Value)))).ToArray();
   }

   private readonly struct RobotInfo(Vector2Int position, Vector2Int velocity) {
      public Vector2Int Position { get; } = position;
      public Vector2Int Velocity { get; } = velocity;
   }
}