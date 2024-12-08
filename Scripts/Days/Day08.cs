using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day08 : AbstractDay {
   protected override int Day => 8;

   public override string Part1() {
      ParseInput(out var antennasPerCoordinates, out var dimensions);
      var antiNodes = GetPart1AntiNodeCoordinates(antennasPerCoordinates.Values.Distinct().ToDictionary(c => c, c => antennasPerCoordinates.Where(t => t.Value == c).Select(t => t.Key).ToHashSet()),
         dimensions);
      return $"{antiNodes.Count}";
   }

   public override string Part2() {
      ParseInput(out var antennasPerCoordinates, out var dimensions);
      var antiNodes = GetPart2AntiNodeCoordinates(antennasPerCoordinates.Values.Distinct().ToDictionary(c => c, c => antennasPerCoordinates.Where(t => t.Value == c).Select(t => t.Key).ToHashSet()),
         dimensions);
      return $"{antiNodes.Count}";
   }

   private static HashSet<Vector2Int> GetPart1AntiNodeCoordinates(Dictionary<char, HashSet<Vector2Int>> allCoordinatesPerAntennaType, Vector2Int dimensions) {
      return (from antennaGroup in allCoordinatesPerAntennaType
              from firstAntennaCoords in antennaGroup.Value
              from secondAntennaCoords in antennaGroup.Value.Except([firstAntennaCoords])
              select 2 * secondAntennaCoords - firstAntennaCoords
              into antiNodeCoords
              where IsInGrid(antiNodeCoords, dimensions)
              select antiNodeCoords).ToHashSet();
   }

   private static HashSet<Vector2Int> GetPart2AntiNodeCoordinates(Dictionary<char, HashSet<Vector2Int>> allCoordinatesPerAntennaType, Vector2Int dimensions) {
      var antiNodeLines = from antennaGroup in allCoordinatesPerAntennaType
                          from firstAntennaCoords in antennaGroup.Value
                          from secondAntennaCoords in antennaGroup.Value.Except([firstAntennaCoords])
                          select (origin: firstAntennaCoords, delta: secondAntennaCoords - firstAntennaCoords);

      var result = new HashSet<Vector2Int>();
      foreach (var antiNodeLine in antiNodeLines) {
         var antiNodeCoords = antiNodeLine.origin + antiNodeLine.delta;
         while (IsInGrid(antiNodeCoords, dimensions)) {
            result.Add(antiNodeCoords);
            antiNodeCoords += antiNodeLine.delta;
         }
      }

      return result;
   }

   private static bool IsInGrid(Vector2Int coordinates, Vector2Int dimensions) => coordinates.X >= 0 && coordinates.X < dimensions.X && coordinates.Y >= 0 && coordinates.Y < dimensions.Y;

   private void ParseInput(out Dictionary<Vector2Int, char> antennasPerCoordinates, out Vector2Int dimensions) {
      var validLines = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
      dimensions = (validLines.First().Trim().Length, validLines.Length);
      antennasPerCoordinates = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), antenna: c)))
         .Where(t => t.antenna != '.').ToDictionary(t => t.coordinates, t => t.antenna);
   }
}