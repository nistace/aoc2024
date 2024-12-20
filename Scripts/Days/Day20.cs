using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day20 : AbstractDay {
   protected override int Day => 20;

   public override string Part1() {
      ParseInput(out var startPosition, out var walls);
      return $"{GetCheats(GetPaths(startPosition, walls), 2, 100)}";
   }

   public override string Part2() {
      ParseInput(out var startPosition, out var walls);
      return $"{GetCheats(GetPaths(startPosition, walls), 20, 100)}";
   }

   private static int GetCheats(IReadOnlyDictionary<Vector2Int, int> pathCosts, int allowedCheatDistance, int minCheatSavedCost) {
      var cheats = 0;
      foreach (var origin in pathCosts) {
         foreach (var destination in pathCosts) {
            if (destination.Value - origin.Value < minCheatSavedCost) continue;
            if (origin.Key.GridDistance(destination.Key) > allowedCheatDistance) continue;
            if (destination.Value - origin.Value - origin.Key.GridDistance(destination.Key) < minCheatSavedCost) continue;

            cheats++;
         }
      }
      return cheats;
   }

   private static Dictionary<Vector2Int, int> GetPaths(Vector2Int startPosition, HashSet<Vector2Int> walls) {
      var closedCosts = new Dictionary<Vector2Int, int>();
      var openCosts = new Dictionary<Vector2Int, int> { { startPosition, 0 } };

      while (openCosts.Count > 0) {
         (var node, var cost) = openCosts.OrderBy(t => t.Value).First();
         closedCosts.Add(node, cost);
         openCosts.Remove(node);

         foreach (var next in node.FourNeighbours.Where(t => !walls.Contains(t) && !closedCosts.ContainsKey(t))) {
            openCosts.Add(next, cost + 1);
         }
      }

      return closedCosts;
   }

   public void ParseInput(out Vector2Int startPosition, out HashSet<Vector2Int> walls) {
      var charsAtCoords = GetInputLines().SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), c))).ToDictionary(t => t.coordinates, t => t.c);

      startPosition = charsAtCoords.Single(t => t.Value == 'S').Key;
      walls = charsAtCoords.Where(t => t.Value == '#').Select(t => t.Key).ToHashSet();
   }
}
