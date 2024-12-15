using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day15 : AbstractDay {
   protected override int Day => 15;

   public override string Part1() {
      ParseInput(out var robotInitialPosition, out var walls, out var boxes, out var moveInstructions);

      // ReSharper disable once UnusedVariable
      var robotPosition = moveInstructions.Aggregate(robotInitialPosition, (current, moveInstruction) => MoveRobot(current, moveInstruction, ref walls, ref boxes));

      return $"{boxes.Sum(t => (long)t.X + t.Y * 100)}";
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
      ParseInput(out var robotInitialPosition, out var walls, out var boxes, out var moveInstructions);
      EnlargeInput(ref robotInitialPosition, ref walls, ref boxes);

      // ReSharper disable once UnusedVariable
      var robotPosition = moveInstructions.Aggregate(robotInitialPosition, (current, moveInstruction) => MoveRobotLarge(current, moveInstruction, ref walls, ref boxes));

      return $"{boxes.Sum(t => (long)t.X + t.Y * 100)}";
   }

   private static Vector2Int MoveRobotLarge(Vector2Int robotPosition, Vector2Int moveInstruction, ref HashSet<Vector2Int> walls, ref HashSet<Vector2Int> boxes) {
      var robotTargetPosition = robotPosition + moveInstruction;
      if (walls.Contains(robotTargetPosition)) return robotPosition;
      if (boxes.Contains(robotTargetPosition) || boxes.Contains(robotTargetPosition + Vector2Int.Left)) {
         var boxPosition = boxes.Contains(robotTargetPosition) ? robotTargetPosition : robotTargetPosition + Vector2Int.Left;
         var boxesToMove = new Dictionary<Vector2Int, Vector2Int>();
         if (!IsLargeBoxMovementAllowed(boxPosition, moveInstruction, walls, boxes, ref boxesToMove)) {
            return robotPosition;
         }

         foreach (var newPosition in boxesToMove.Values.Except(boxesToMove.Keys)) {
            boxes.Add(newPosition);
         }
         foreach (var newPosition in boxesToMove.Keys.Except(boxesToMove.Values)) {
            boxes.Remove(newPosition);
         }

         return robotTargetPosition;
      }

      return robotTargetPosition;
   }

   private static bool IsLargeBoxMovementAllowed(Vector2Int box, Vector2Int movement, HashSet<Vector2Int> walls, HashSet<Vector2Int> boxes,
      ref Dictionary<Vector2Int, Vector2Int> requiredBoxMovements) {
      var boxTargetPosition = box + movement;

      if (walls.Contains(boxTargetPosition) || walls.Contains(boxTargetPosition + Vector2Int.Right)) return false;

      foreach (var requiredClearPosition in new[] { boxTargetPosition + Vector2Int.Left, boxTargetPosition, boxTargetPosition + Vector2Int.Right }) {
         if (requiredClearPosition != box
             && !requiredBoxMovements.ContainsKey(requiredClearPosition)
             && boxes.Contains(requiredClearPosition)
             && !IsLargeBoxMovementAllowed(requiredClearPosition, movement, walls, boxes, ref requiredBoxMovements)) {
            return false;
         }
      }

      requiredBoxMovements.Add(box, boxTargetPosition);

      return true;
   }

   private static void EnlargeInput(ref Vector2Int robotInitialPosition, ref HashSet<Vector2Int> walls, ref HashSet<Vector2Int> boxes) {
      robotInitialPosition = new Vector2Int(robotInitialPosition.X * 2, robotInitialPosition.Y);
      walls = walls.SelectMany(t => new[] { new Vector2Int(t.X * 2, t.Y), new Vector2Int(t.X * 2 + 1, t.Y) }).ToHashSet();
      boxes = boxes.Select(t => new Vector2Int(t.X * 2, t.Y)).ToHashSet();
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

   // ReSharper disable once UnusedMember.Local
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

   // ReSharper disable once UnusedMember.Local
   private static void DebugLarge(Vector2Int robotPosition, HashSet<Vector2Int> walls, HashSet<Vector2Int> boxes) {
      var dimensions = new Vector2Int(walls.Max(t => t.X), walls.Max(t => t.Y));
      for (var y = 0; y <= dimensions.Y; y++) {
         for (var x = 0; x <= dimensions.X; x++) {
            var position = new Vector2Int(x, y);
            if (walls.Contains(position)) Console.Write('#');
            else if (boxes.Contains(position)) Console.Write('[');
            else if (boxes.Contains(position + Vector2Int.Left)) Console.Write(']');
            else if (robotPosition == position) Console.Write('@');
            else Console.Write(' ');
         }
         Console.WriteLine();
      }
   }
}