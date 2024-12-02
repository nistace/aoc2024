namespace AdventOfCode2024.Scripts.Days;

public class Day02 : AbstractDay {
	protected override int Day => 2;

	public override string Part1() {
		return $"{GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Split(" ").Select(int.Parse).ToArray()).Count(IsSafe)}";
	}

	public override string Part2() {
		return $"{GetInputLines().Where(t => !string.IsNullOrWhiteSpace(t)).Select(t => t.Split(" ").Select(int.Parse).ToArray()).Count(IsSafeAllowingRemovals)}";
	}

	private static bool IsSafe(int[] array) {
		float sign = array[1] - array[0];
		if (sign == 0) return false;
		for (var i = 0; i < array.Length - 1; ++i) {
			if ((array[i + 1] - array[i]) / sign <= 0) return false;
			if (Math.Abs(array[i + 1] - array[i]) > 3) return false;
		}
		return true;
	}

	private static bool IsSafeAllowingRemovals(int[] array) {
		if (IsSafe(array)) return true;
		for (var i = 0; i < array.Length; ++i) {
			var list = array.ToList();
			list.RemoveAt(i);
			if (IsSafe(list.ToArray())) return true;
		}
		return false;
	}
}