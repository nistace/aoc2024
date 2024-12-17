using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day12 : AbstractDay {
   protected override int Day => 12;

   public override string Part1() {
      ParseInput(out var grid);
      var areasAndPerimeters = EvaluateAllAreas(grid, EvaluateAreaAndPerimeter);
      return $"{areasAndPerimeters.Values.Sum(t => t.Sum(u => u.area * u.coefficient))}";
   }

   public override string Part2() {
      ParseInput(out var grid);
      var areasAndSides = EvaluateAllAreas(grid, EvaluateAreaAndSideCount);
      return $"{areasAndSides.Values.Sum(t => t.Sum(u => u.area * u.coefficient))}";
   }

   private static Dictionary<char, List<(int area, int coefficient)>> EvaluateAllAreas(IReadOnlyDictionary<Vector2Int, char> grid,
      Func<Vector2Int, IReadOnlyDictionary<Vector2Int, char>, HashSet<Vector2Int>, (int area, int coefficient)> evaluateFunction) {
      var dimensions = (width: grid.Max(x => x.Key.X) + 1, height: grid.Max(x => x.Key.Y) + 1);

      var result = grid.Values.Distinct().ToDictionary(t => t, _ => new List<(int area, int perimeter)>());
      var analyzedCells = new HashSet<Vector2Int>();

      for (var y = 0; y < dimensions.height; y++)
      for (var x = 0; x < dimensions.width; x++) {
         var coordinates = new Vector2Int(x, y);
         if (!analyzedCells.Contains(coordinates)) {
            var character = grid[coordinates];
            result[character].Add(evaluateFunction(coordinates, grid, analyzedCells));
         }
      }

      return result;
   }

   private static (int area, int perimeter) EvaluateAreaAndPerimeter(Vector2Int coordinates, IReadOnlyDictionary<Vector2Int, char> grid, HashSet<Vector2Int> analyzedCells) {
      AnalyzeAreaWithCell(coordinates, grid, analyzedCells, out var cellsInArea, out var fencesPerCell);
      return (cellsInArea.Count, fencesPerCell.Values.Sum(t => t.Count));
   }

   private static void AnalyzeAreaWithCell(Vector2Int coordinates, IReadOnlyDictionary<Vector2Int, char> grid, HashSet<Vector2Int> analyzedCells, out HashSet<Vector2Int> cellsInArea,
      out Dictionary<Vector2Int, HashSet<Vector2Int>> fencesPerCell) {
      cellsInArea = [];
      PopulateAllCellsInArea(coordinates, grid, cellsInArea);
      fencesPerCell = new Dictionary<Vector2Int, HashSet<Vector2Int>>();

      foreach (var cell in cellsInArea) {
         fencesPerCell.Add(cell, []);
         foreach (var direction in Vector2Int.FourDirections) {
            if (!cellsInArea.Contains(cell + direction)) {
               fencesPerCell[cell].Add(direction);
            }
         }
         analyzedCells.Add(cell);
      }
   }

   private static (int area, int sides) EvaluateAreaAndSideCount(Vector2Int coordinates, IReadOnlyDictionary<Vector2Int, char> grid, HashSet<Vector2Int> analyzedCells) {
      AnalyzeAreaWithCell(coordinates, grid, analyzedCells, out var cellsInArea, out var fencesPerCell);
      var treatedFenceChunks = new HashSet<(Vector2Int cell, Vector2Int direction)>();

      var sides = 0;
      foreach (var fenceChunk in fencesPerCell.SelectMany(t => t.Value.Select(direction => (cell: t.Key, direction)))) {
         if (treatedFenceChunks.Contains(fenceChunk)) continue;
         sides++;
         treatedFenceChunks.Add(fenceChunk);
         foreach (var neighbourDirection in new[] { fenceChunk.direction.AntiClockwise90, fenceChunk.direction.Clockwise90 }) {
            var neighbour = fenceChunk.cell + neighbourDirection;
            while (fencesPerCell.ContainsKey(neighbour) && fencesPerCell[neighbour].Contains(fenceChunk.direction)) {
               treatedFenceChunks.Add((neighbour, fenceChunk.direction));
               neighbour += neighbourDirection;
            }
         }
      }

      return (cellsInArea.Count, sides);
   }

   private static void PopulateAllCellsInArea(Vector2Int coordinates, IReadOnlyDictionary<Vector2Int, char> grid, HashSet<Vector2Int> cellsInArea) {
      if (!grid.TryGetValue(coordinates, out var character)) return;
      cellsInArea.Add(coordinates);
      foreach (var neighbour in coordinates.FourNeighbours) {
         if (!cellsInArea.Contains(neighbour) && grid.TryGetValue(neighbour, out var neighbourChar) && neighbourChar == character) {
            PopulateAllCellsInArea(neighbour, grid, cellsInArea);
         }
      }
   }

   private void ParseInput(out Dictionary<Vector2Int, char> grid) {
      grid = GetInputLines().SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), c))).ToDictionary(t => t.coordinates, t => t.c);
   }

   // ReSharper disable once UnusedMember.Local
   private static void Debug(Dictionary<char, List<(int area, int coefficient)>> areasAndCoefficients) {
      foreach (var charAreas in areasAndCoefficients) {
         foreach (var charArea in charAreas.Value) {
            Console.WriteLine($"{charAreas.Key}: {charArea.area:00000} * {charArea.coefficient:00000} = {charArea.area * charArea.coefficient:0000000}");
         }
      }
   }
}