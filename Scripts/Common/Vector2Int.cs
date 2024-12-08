namespace AdventOfCode2024.Scripts.Common;

public class Vector2Int(int x, int y) : IEquatable<Vector2Int> {
	public int X { get; } = x;
	public int Y { get; } = y;

	public static implicit operator Vector2Int((int x, int y) t) => new Vector2Int(t.x, t.y);

	public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
	public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
	public static Vector2Int operator *(Vector2Int a, int scalar) => new Vector2Int(a.X * scalar, a.Y * scalar);
	public static Vector2Int operator *(int scalar, Vector2Int a) => a * scalar;

	public bool Equals(Vector2Int? other) => other is not null && X == other.X && Y == other.Y;
	public override bool Equals(object? obj) => obj is Vector2Int other && Equals(other);
	public override int GetHashCode() => HashCode.Combine(X, Y);

	public override string ToString() => $"({X}, {Y})";
}