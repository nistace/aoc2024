using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day10 : AbstractDay {
   protected override int Day => 10;
   public static readonly Dictionary<Vector2Int, int> nothingReachable = new Dictionary<Vector2Int, int>();

   public static Dictionary<Vector2Int, int> GetReachableNines(Vector2Int from, Dictionary<Vector2Int, int> map, Dictionary<Vector2Int, Dictionary<Vector2Int, int>> cachedReachableNines) {
      if (cachedReachableNines.TryGetValue(from, out var reachableNinesFrom)) return reachableNinesFrom;
      if (!map.TryGetValue(from, out var fromValue)) return nothingReachable;

      if (fromValue == 9) {
         cachedReachableNines.Add(from, new Dictionary<Vector2Int, int> { { from, 1 } });
      }
      else {
         cachedReachableNines.Add(from, new Dictionary<Vector2Int, int>());
         var cachedNinesFromThis = cachedReachableNines[from];
         foreach (var neighbour in from.FourNeighbours.Where(t => map.TryGetValue(t, out var neighbourValue) && neighbourValue == fromValue + 1)) {
            foreach (var reachableNine in GetReachableNines(neighbour, map, cachedReachableNines)) {
               cachedNinesFromThis.TryAdd(reachableNine.Key, 0);
               cachedNinesFromThis[reachableNine.Key] += reachableNine.Value;
            }
         }
      }

      return cachedReachableNines[from];
   }

   public override string Part1() {
      ParseInput(out var map);
      var reachableNines = new Dictionary<Vector2Int, Dictionary<Vector2Int, int>>();
      return $"{map.Where(t => t.Value == 0).Sum(t => GetReachableNines(t.Key, map, reachableNines).Count)}";
   }

   private void ParseInput(out Dictionary<Vector2Int, int> map) {
      map = GetInputLines().SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), value: int.Parse($"{c}")))).ToDictionary(t => t.coordinates, t => t.value);
   }

   public override string Part2() {
      ParseInput(out var map);
      var reachableNines = new Dictionary<Vector2Int, Dictionary<Vector2Int, int>>();
      return $"{map.Where(t => t.Value == 0).Sum(t => GetReachableNines(t.Key, map, reachableNines).Sum(u => u.Value))}";
   }
}