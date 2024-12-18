using System.Text.RegularExpressions;
using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day13 : AbstractDay {
   protected override int Day => 13;
   private readonly Regex inputRegexButton = new Regex(@"Button \w: X\+(\d+), Y\+(\d+)");
   private readonly Regex inputRegexPrize = new Regex(@"Prize: X=(\d+), Y=(\d+)");
   private const int PressACost = 3;
   private const int PressBCost = 1;
   private const long Part2Offset = 10000000000000L;

   public override string Part1() {
      ParseInput(out var machineSetups);
      return $"{machineSetups.Select(t => t.Simulate()).Where(t => t.hasSolution).Sum(t => t.pressA * PressACost + t.pressB * PressBCost)}";
   }

   public override string Part2() {
      ParseInput(out var machineSetups);
      return $"{machineSetups.Select(t => t.Simulate(Part2Offset)).Where(t => t.hasSolution).Sum(t => t.pressA * PressACost + t.pressB * PressBCost)}";
   }

   private void ParseInput(out MachineSetup[] machineSetups) {
      var machineSetupsList = new List<MachineSetup>();
      var inputLines = GetInputLines().Where(t => !string.IsNullOrEmpty(t)).ToArray();
      for (var lineIndex = 0; lineIndex < inputLines.Length; lineIndex += 3) {
         var buttonAMatch = inputRegexButton.Match(inputLines[lineIndex]);
         var buttonBMatch = inputRegexButton.Match(inputLines[lineIndex + 1]);
         var prizeMatch = inputRegexPrize.Match(inputLines[lineIndex + 2]);

         machineSetupsList.Add(new MachineSetup([
            (int.Parse(buttonAMatch.Groups[1].Value), int.Parse(buttonAMatch.Groups[2].Value)),
            (int.Parse(buttonBMatch.Groups[1].Value), int.Parse(buttonBMatch.Groups[2].Value)),
            (int.Parse(prizeMatch.Groups[1].Value), int.Parse(prizeMatch.Groups[2].Value))
         ]));
      }
      machineSetups = machineSetupsList.ToArray();
   }

   private class MachineSetup(Vector2Int[] buttonsAndPrize) {
      private Vector2Int ButtonAOffset { get; } = buttonsAndPrize[0];
      private Vector2Int ButtonBOffset { get; } = buttonsAndPrize[1];
      private Vector2Int PrizeLocation { get; } = buttonsAndPrize[2];

      public (bool hasSolution, long pressA, long pressB) Simulate(long offsetPrize = 0) {
         var ax = ButtonAOffset.X;
         var ay = ButtonAOffset.Y;
         var bx = ButtonBOffset.X;
         var by = ButtonBOffset.Y;
         var px = PrizeLocation.X + offsetPrize;
         var py = PrizeLocation.Y + offsetPrize;

         var pressB = (ax * py - px * ay) / (double)(ax * by - bx * ay);
         if (pressB != (long)pressB) return (false, default, default);

         var pressA = (px - pressB * bx) / ax;
         if (pressA != (long)pressA) return (false, default, default);

         return (true, (long)pressA, (long)pressB);
      }
   }
}