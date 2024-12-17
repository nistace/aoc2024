namespace AdventOfCode2024.Scripts.Common;

public readonly struct Vector2Int(int x, int y) : IEquatable<Vector2Int> {
   public static Vector2Int Zero { get; } = new(0, 0);
   public static Vector2Int Left { get; } = new(-1, 0);
   public static Vector2Int Down { get; } = new(0, 1);
   public static Vector2Int Up { get; } = new(0, -1);
   public static Vector2Int Right { get; } = new(1, 0);
   public static IReadOnlyList<Vector2Int> FourDirections { get; } = [Up, Right, Down, Left];

   public int X { get; } = x;
   public int Y { get; } = y;
   public Vector2Int Clockwise90 => new Vector2Int(-Y, X);
   public Vector2Int AntiClockwise90 => new Vector2Int(Y, -X);

   public Vector2Int[] FourNeighbours => [this + Left, this + Right, this + Up, this + Down];

   public static implicit operator Vector2Int((int x, int y) t) => new Vector2Int(t.x, t.y);

   public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
   public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
   public static Vector2Int operator *(Vector2Int a, int scalar) => new Vector2Int(a.X * scalar, a.Y * scalar);
   public static Vector2Int operator *(int scalar, Vector2Int a) => a * scalar;

   public static bool operator ==(Vector2Int a, Vector2Int b) => a.Equals(b);
   public static bool operator !=(Vector2Int a, Vector2Int b) => !a.Equals(b);

   public bool Equals(Vector2Int other) => X == other.X && Y == other.Y;
   public override bool Equals(object? obj) => obj is Vector2Int other && Equals(other);
   public override int GetHashCode() => HashCode.Combine(X, Y);

   public override string ToString() => $"({X}, {Y})";
}