using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day08 : AbstractDay {
   protected override int Day => 8;
   private readonly HashSet<Func<long, long, long>> part1Operators = [(a, b) => a + b, (a, b) => a * b];
   private readonly HashSet<Func<long, long, long>> part2Operators = [(a, b) => long.Parse($"{a}{b}"), (a, b) => a + b, (a, b) => a * b];

   public override string Part1() {
      ParseInput(out var antennasPerCoordinates, out var dimensions);
      var antiNodes = GetAllAntiNodeCoordinates(antennasPerCoordinates.Values.Distinct().ToDictionary(c => c, c => antennasPerCoordinates.Where(t => t.Value == c).Select(t => t.Key).ToHashSet()),
         dimensions);
      return $"{antiNodes.Count}";
   }

   public override string Part2() => "";

   private static HashSet<Vector2Int> GetAllAntiNodeCoordinates(Dictionary<char, HashSet<Vector2Int>> allCoordinatesPerAntennaType, Vector2Int dimensions) {
      return (from antennaGroup in allCoordinatesPerAntennaType
              from firstAntennaCoords in antennaGroup.Value
              from secondAntennaCoords in antennaGroup.Value.Except([firstAntennaCoords])
              select 2 * secondAntennaCoords - firstAntennaCoords
              into antiNodeCoords
              where antiNodeCoords.X >= 0 && antiNodeCoords.X < dimensions.X && antiNodeCoords.Y >= 0 && antiNodeCoords.Y < dimensions.Y
              select antiNodeCoords).ToHashSet();
   }

   private void ParseInput(out Dictionary<Vector2Int, char> antennasPerCoordinates, out Vector2Int dimensions) {
      var validLines = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
      dimensions = (validLines.First().Trim().Length, validLines.Length);
      antennasPerCoordinates = GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), antenna: c)))
         .Where(t => t.antenna != '.').ToDictionary(t => t.coordinates, t => t.antenna);
   }
}