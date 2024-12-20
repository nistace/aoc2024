using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day20 : AbstractDay {
   protected override int Day => 20;

	public override string Part1() {
		ParseInput(out var startPosition, out var endPosition, out var walls);
		var paths = GetPaths(startPosition, walls);
		var cheats = GetCheats(walls, paths);

		return $"{cheats.Count(t => t.Value >= 100)}";
	}
	
	private static Dictionary<Vector2Int, int> GetCheats(HashSet<Vector2Int> walls, IReadOnlyDictionary<Vector2Int, int> costs) {
		return (from cheatedWall in walls
		        select (wall: cheatedWall, neighbouringPaths: cheatedWall.FourNeighbours.Where(costs.ContainsKey).ToArray())
		        into cheat
		        where cheat.neighbouringPaths.Length >= 2
		        select (cheat.wall, savedCost: cheat.neighbouringPaths.Max(t => costs[t]) - cheat.neighbouringPaths.Min(t => costs[t]) - 2))
			.ToDictionary(t => t.wall, t => t.savedCost);
	}

   private IReadOnlyDictionary<Vector2Int, int> GetPaths(Vector2Int startPosition, HashSet<Vector2Int> walls) {
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

   public override string Part2() {
      return "";
   }

   public void ParseInput(out Vector2Int startPosition, out Vector2Int endPosition, out HashSet<Vector2Int> walls) {
      var charsAtCoords = GetInputLines().SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), c))).ToDictionary(t => t.coordinates, t => t.c);

      startPosition = charsAtCoords.Single(t => t.Value == 'S').Key;
      endPosition = charsAtCoords.Single(t => t.Value == 'E').Key;
      walls = charsAtCoords.Where(t => t.Value == '#').Select(t => t.Key).ToHashSet();
   }
}