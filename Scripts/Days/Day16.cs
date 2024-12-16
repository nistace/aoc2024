using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day16 : AbstractDay {
   protected override int Day => 16;

   public override string Part1() {
      ParseInput(out var startPosition, out var endPosition, out var walls);

      return $"{GetShortestPathScore(startPosition, endPosition, walls)}";
   }

   private static long GetShortestPathScore(Vector2Int startPosition, Vector2Int endPosition, HashSet<Vector2Int> walls) {
      var shortestPaths = new Dictionary<Vector2Int, (Vector2Int origin, long score, Vector2Int direction)>();

      var openedNodes = new Dictionary<Vector2Int, (Vector2Int origin, long score, Vector2Int direction)> { { startPosition, (startPosition, 0, Vector2Int.Right) } };

      while (!shortestPaths.ContainsKey(endPosition)) {
         (var nodeToLock, var nodeToLockInfo) = openedNodes.OrderBy(t => t.Value.score).First();
         shortestPaths.Add(nodeToLock, nodeToLockInfo);
         openedNodes.Remove(nodeToLock);

         foreach (var newOpenedNode in nodeToLock.FourNeighbours.Where(t => !shortestPaths.ContainsKey(t) && !walls.Contains(t))) {
            var direction = newOpenedNode - nodeToLock;
            var score = shortestPaths[nodeToLock].score + 1;
            if (direction != shortestPaths[nodeToLock].direction) score += 1000;

            if (!openedNodes.TryGetValue(newOpenedNode, out var value)) openedNodes.Add(newOpenedNode, (nodeToLock, score, direction));
            else if (value.score > score) openedNodes[newOpenedNode] = (nodeToLock, score, direction);
         }
      }

      return shortestPaths[endPosition].score;
   }


   public override string Part2() {
      return "";
   }

   // ReSharper disable once UnusedMember.Local
   private static void Debug(Dictionary<Vector2Int, (Vector2Int direction, long score)> resultNodes, HashSet<Vector2Int> walls) {
      var dimensions = new Vector2Int(walls.Max(t => t.X) + 1, walls.Max(t => t.Y) + 1);

      for (var y = 0; y < dimensions.Y; y++) {
         for (var x = 0; x < dimensions.X; x++) {
            if (walls.Contains(new Vector2Int(x, y))) Console.Write('#');
            else if (resultNodes.TryGetValue((x, y), out var resultAtPosition)) {
               if (resultAtPosition.direction == Vector2Int.Right) Console.Write('>');
               else if (resultAtPosition.direction == Vector2Int.Left) Console.Write('<');
               else if (resultAtPosition.direction == Vector2Int.Up) Console.Write('^');
               else if (resultAtPosition.direction == Vector2Int.Down) Console.Write('v');
               else Console.Write('?');
            }
            else Console.Write(' ');
         }
         Console.WriteLine();
      }
   }

   public void ParseInput(out Vector2Int startPosition, out Vector2Int endPosition, out HashSet<Vector2Int> walls) {
      var charsAtCoords = GetInputLines().SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), c))).ToDictionary(t => t.coordinates, t => t.c);

      startPosition = charsAtCoords.Single(t => t.Value == 'S').Key;
      endPosition = charsAtCoords.Single(t => t.Value == 'E').Key;
      walls = charsAtCoords.Where(t => t.Value == '#').Select(t => t.Key).ToHashSet();
   }
}