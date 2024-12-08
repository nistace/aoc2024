namespace AdventOfCode2024.Scripts.Days;

public class Day06 : AbstractDay {
   protected override int Day => 6;

   public override string Part1() {
      ParseInput(out var obstacles, out var startPosition, out var dimensions);
      var simulated = Simulate(obstacles, startPosition, dimensions);

      return $"{simulated.visited.Count}";
   }

   public override string Part2() {
      ParseInput(out var obstacles, out var startPosition, out var dimensions);
      var defaultVisited = Simulate(obstacles, startPosition, dimensions);

      var possibleObstacles = defaultVisited.visited.Keys.Except(obstacles).Except([startPosition]);

      return $"{possibleObstacles.Count(newObstacle => Simulate(obstacles.Union([newObstacle]).ToHashSet(), startPosition, dimensions).loop)}";
   }

   private static (bool loop, Dictionary<(int x, int y), HashSet<Heading>> visited) Simulate(HashSet<(int x, int y)> obstacles, (int x, int y) startPosition, (int width, int height) dimensions) {
      var visited = new Dictionary<(int x, int y), HashSet<Heading>>();
      var heading = Heading.Up;
      for (var currentPosition = startPosition; InGrid(currentPosition, dimensions); currentPosition = heading.MoveFrom(currentPosition)) {
         if (!visited.ContainsKey(currentPosition)) visited.Add(currentPosition, []);
         if (visited[currentPosition].Contains(heading)) {
            return (true, visited);
         }

         visited[currentPosition].Add(heading);

         while (obstacles.Contains(heading.MoveFrom(currentPosition))) {
            heading = heading.RotateRight();
         }
      }

      return (false, visited);
   }

   private static void Debug(HashSet<(int x, int y)> obstacles, (int x, int y) startPosition, (int width, int height) dimensions, Dictionary<(int x, int y), HashSet<Heading>> visited) {
      for (var y = 0; y < dimensions.height; y++) {
         for (var x = 0; x < dimensions.width; x++) {
            if ((x, y) == startPosition) Console.Write("^");
            else if (obstacles.Contains((x, y))) Console.Write("#");
            else if (visited.ContainsKey((x, y))) Console.Write($"{visited[(x, y)].Count}");
            else Console.Write(".");
         }
         Console.WriteLine();
      }
   }

   private static bool InGrid((int x, int y) currentPosition, (int width, int height) dimensions) {
      if (currentPosition.x < 0) return false;
      if (currentPosition.y < 0) return false;
      if (currentPosition.x >= dimensions.width) return false;
      if (currentPosition.y >= dimensions.height) return false;
      return true;
   }

   private void ParseInput(out HashSet<(int, int)> obstacles, out (int, int) startPosition, out (int width, int height) dimensions) {
      var allCharsCoords = GetInputLines().Select((chars, y) => (chars, y)).SelectMany(line => line.chars.Select((c, x) => (c, coords: (x, line.y)))).ToDictionary(t => t.coords, t => t.c);
      obstacles = allCharsCoords.Where(t => t.Value == '#').Select(t => t.Key).ToHashSet();
      startPosition = allCharsCoords.Single(t => t.Value == '^').Key;
      dimensions = (allCharsCoords.Max(t => t.Key.x + 1), allCharsCoords.Max(t => t.Key.y + 1));
   }

   private readonly struct Heading : IEquatable<Heading> {
      public static Heading Up { get; } = new Heading(0, -1);
      public static Heading Right { get; } = new Heading(1, 0);
      public static Heading Down { get; } = new Heading(0, 1);
      public static Heading Left { get; } = new Heading(-1, 0);

      private static Dictionary<Heading, Heading> RightRotations { get; } = new Dictionary<Heading, Heading> { { Up, Right }, { Right, Down }, { Down, Left }, { Left, Up } };

      private readonly int moveDeltaX;
      private readonly int moveDeltaY;

      private Heading(int moveDeltaX, int moveDeltaY) {
         this.moveDeltaX = moveDeltaX;
         this.moveDeltaY = moveDeltaY;
      }

      public (int x, int y) MoveFrom((int x, int y) origin) => (origin.x + moveDeltaX, origin.y + moveDeltaY);
      public Heading RotateRight() => RightRotations[this];

      public bool Equals(Heading other) => moveDeltaX == other.moveDeltaX && moveDeltaY == other.moveDeltaY;
      public override bool Equals(object? obj) => obj is Heading other && Equals(other);
      public override int GetHashCode() => HashCode.Combine(moveDeltaX, moveDeltaY);
   }
}