namespace AdventOfCode2024.Scripts.Days;

public class Day19 : AbstractDay {
   protected override int Day => 19;

   public override string Part1() {
      ParseInput(out var patterns, out var desiredDesigns);

      return $"{CountDoableDesigns(desiredDesigns, patterns)}";
   }

   private static int CountDoableDesigns(List<string> desiredDesigns, HashSet<string> patterns) {
      var possibleDesigns = patterns.ToHashSet();
      var impossibleDesigns = new HashSet<string>();
      return desiredDesigns.Count(t => IsDesignPossible(t, possibleDesigns, impossibleDesigns));
   }

   private static bool IsDesignPossible(string design, HashSet<string> possibleDesigns, HashSet<string> impossibleDesigns) {
      if (possibleDesigns.Contains(design)) return true;
      if (impossibleDesigns.Contains(design)) return false;

      if (possibleDesigns.Where(design.StartsWith).Any(possibleStart => IsDesignPossible(design[possibleStart.Length..], possibleDesigns, impossibleDesigns))) {
         possibleDesigns.Add(design);
         return true;
      }

      impossibleDesigns.Add(design);
      return false;
   }

   public override string Part2() {
      return "";
   }

   private void ParseInput(out HashSet<string> patterns, out List<string> desiredDesigns) {
      var inputLines = GetInputLines();
      patterns = inputLines[0].Split(",").Select(t => t.Trim()).ToHashSet();
      desiredDesigns = inputLines.Skip(1).Where(t => !string.IsNullOrWhiteSpace(t)).ToList();
   }
}