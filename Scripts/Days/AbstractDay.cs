namespace AdventOfCode2024.Scripts.Days;

public abstract class AbstractDay {
	protected abstract int Day { get; }
	private string InputPath => $"../../../Input/{Day:00}.txt";

	public abstract string Part1();
	public abstract string Part2();

	protected string[] GetInputLines() => File.ReadAllLines(InputPath);
	protected string GetInputText() => File.ReadAllText(InputPath);
}