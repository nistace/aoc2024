namespace AdventOfCode2024.Scripts.Days;

public class Day06 : AbstractDay {
	protected override int Day => 6;

	public override string Part1() {
		ParseInput(out var obstacles, out var startPosition, out var dimensions);
		var visited = new HashSet<(int x, int y)>();
		var heading = Heading.Up;
		for (var currentPosition = startPosition; InGrid(currentPosition, dimensions); currentPosition = heading.MoveFrom(currentPosition)) {
			visited.Add(currentPosition);
			if (obstacles.Contains(heading.MoveFrom(currentPosition))) {
				heading = heading.RotateRight();
			}
		}

		return $"{visited.Count}";
	}

	public override string Part2() => "";

	private static bool InGrid((int x, int y) currentPosition, (int width, int height) dimensions) {
		if (currentPosition.x < 0) return false;
		if (currentPosition.y < 0) return false;
		if (currentPosition.x >= dimensions.width) return false;
		if (currentPosition.y >= dimensions.height) return false;
		return true;
	}

	private void ParseInput(out HashSet<(int, int)> obstacles, out (int, int) startPosition, out (int, int) dimensions) {
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