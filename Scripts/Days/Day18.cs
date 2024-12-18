using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day18 : AbstractDay {
   protected override int Day => 18;

   public override string Part1() {
      ParseInput(out var fallingBytesCoordinates);
      var dimensions = new Vector2Int(71, 71);
      const int nanoseconds = 1024;

      var walls = GenerateSurroundingWalls(dimensions).Union(fallingBytesCoordinates.Take(nanoseconds)).ToHashSet();

      return $"{CountMinSteps((0, 0), dimensions - (1, 1), walls)}";
   }

   private static IEnumerable<Vector2Int> GenerateSurroundingWalls(Vector2Int dimensions) => Enumerable.Range(0, dimensions.X).SelectMany(x => new Vector2Int[] { (x, -1), (x, dimensions.Y) })
      .Union(Enumerable.Range(0, dimensions.Y).SelectMany(y => new Vector2Int[] { (-1, y), (dimensions.X, y) }));

   private static long CountMinSteps(Vector2Int start, Vector2Int end, HashSet<Vector2Int> walls) {
      var closedNodesAndScores = new Dictionary<Vector2Int, long>();
      var openNodesAndScores = new Dictionary<Vector2Int, (long score, long heuristic)> { { start, (0, start.GridDistance(end)) } };

      while (!closedNodesAndScores.ContainsKey(end)) {
         var next = openNodesAndScores.OrderBy(t => t.Value.heuristic).First();

         closedNodesAndScores.Add(next.Key, next.Value.score);
         openNodesAndScores.Remove(next.Key);

         foreach (var neighbour in next.Key.FourNeighbours.Where(t => !walls.Contains(t) && !closedNodesAndScores.ContainsKey(t))) {
            var neighbourScore = next.Value.score + 1;
            var neighbourHeuristic = neighbourScore + neighbour.GridDistance(end);
            if (!openNodesAndScores.TryGetValue(neighbour, out var openNeighbour) || openNeighbour.score > neighbourScore) {
               openNodesAndScores[neighbour] = (neighbourScore, neighbourHeuristic);
            }
         }
      }

      return closedNodesAndScores[end];
   }

   public override string Part2() => "";

   private void ParseInput(out List<Vector2Int> fallingBytesCoordinates) {
      fallingBytesCoordinates = GetInputLines().Where(t => !string.IsNullOrEmpty(t)).Select(t => new Vector2Int(int.Parse(t.Split(",")[0]), int.Parse(t.Split(",")[1]))).ToList();
   }
}