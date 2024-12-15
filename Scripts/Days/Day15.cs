using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day15 : AbstractDay {
   protected override int Day => 15;

   public override string Part1() {
      ParseInput(out var robotInitialPosition, out var walls, out var boxes, out var moveInstructions);

      var robotPosition = moveInstructions.Aggregate(robotInitialPosition, (current, moveInstruction) => MoveRobot(current, moveInstruction, ref walls, ref boxes));

      Debug(robotPosition, walls, boxes);

      return $"{boxes.Sum(t => (long)t.X + t.Y * 100)}";
   }

   private static void Debug(Vector2Int robotPosition, HashSet<Vector2Int> walls, HashSet<Vector2Int> boxes) {
      var dimensions = new Vector2Int(walls.Max(t => t.X), walls.Max(t => t.Y));
      for (var y = 0; y <= dimensions.Y; y++) {
         for (var x = 0; x <= dimensions.X; x++) {
            var position = new Vector2Int(x, y);
            if (walls.Contains(position)) Console.Write('#');
            else if (boxes.Contains(position)) Console.Write('O');
            else if (robotPosition == position) Console.Write('@');
            else Console.Write(' ');
         }
         Console.WriteLine();
      }
   }

   private static Vector2Int MoveRobot(Vector2Int robotPosition, Vector2Int moveInstruction, ref HashSet<Vector2Int> walls, ref HashSet<Vector2Int> boxes) {
      var robotTargetPosition = robotPosition + moveInstruction;
      if (walls.Contains(robotTargetPosition)) return robotPosition;
      if (boxes.Contains(robotTargetPosition)) {
         var boxTargetPosition = robotTargetPosition + moveInstruction;
         while (!walls.Contains(boxTargetPosition) && boxes.Contains(boxTargetPosition)) {
            boxTargetPosition += moveInstruction;
         }

         if (walls.Contains(boxTargetPosition)) return robotPosition;

         boxes.Add(boxTargetPosition);
         boxes.Remove(robotTargetPosition);
         return robotTargetPosition;
      }

      return robotTargetPosition;
   }

   public override string Part2() {
      return "";
   }

   private void ParseInput(out Vector2Int robotInitialPosition, out HashSet<Vector2Int> walls, out HashSet<Vector2Int> boxes, out IReadOnlyList<Vector2Int> moveInstructions) {
      var inputLines = GetInputLines();
      var blankLineIndex = inputLines.Select((t, i) => (t, i)).First(t => string.IsNullOrEmpty(t.t)).i;

      var gridChars = inputLines.Take(blankLineIndex).SelectMany((line, y) => line.Select((c, x) => (coordinates: new Vector2Int(x, y), c))).ToDictionary(t => t.coordinates, t => t.c);

      robotInitialPosition = gridChars.First(t => t.Value == '@').Key;
      walls = gridChars.Where(t => t.Value == '#').Select(t => t.Key).ToHashSet();
      boxes = gridChars.Where(t => t.Value == 'O').Select(t => t.Key).ToHashSet();

      var moveInstructionChars = new Dictionary<char, Vector2Int> { { '<', Vector2Int.Left }, { '>', Vector2Int.Right }, { '^', Vector2Int.Up }, { 'v', Vector2Int.Down } };

      moveInstructions = string.Concat(inputLines.Skip(blankLineIndex)).Select(t => moveInstructionChars[t]).ToList();
   }
}