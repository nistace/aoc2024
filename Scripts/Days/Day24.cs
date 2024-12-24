namespace AdventOfCode2024.Scripts.Days;

public class Day24 : AbstractDay {
   protected override int Day => 24;
   protected Dictionary<string, Func<bool, bool, bool>> Gates { get; } = new Dictionary<string, Func<bool, bool, bool>> {
      { "OR", (a, b) => a || b },
      { "XOR", (a, b) => a != b },
      { "AND", (a, b) => a && b },
   };

   public override string Part1() {
      ParseInput(out var initialValues, out var instructions);

      var values = initialValues.ToDictionary(t => t.Key, t => t.Value);

      var remainingInstructions = instructions.ToHashSet();
      while (remainingInstructions.Count > 0) {
         var instruction = remainingInstructions.FirstOrDefault(t => values.ContainsKey(t.inputAKey) && values.ContainsKey(t.inputBKey));
         values.Add(instruction.outputKey, Gates[instruction.gate].Invoke(values[instruction.inputAKey], values[instruction.inputBKey]));
         remainingInstructions.Remove(instruction);
      }

      var outputDecimal = values
         .Where(t => t.Key.StartsWith('z'))
         .Where(t => t.Value)
         .Select(t => int.Parse(t.Key[1..]))
         .Aggregate(0L, (current, zIndex) => current + (long)Math.Pow(2, zIndex));

      return $"{outputDecimal}";
   }

   public override string Part2() {
      ParseInput(out var initialValues, out var instructions);

      var outputInstructions = instructions.ToDictionary(t => t.outputKey, t => (t.gate, t.inputAKey, t.inputBKey));

      var erroneousInstructionKeys = new HashSet<string>();

      // All Z's, except the last one, must come from a XOR
      foreach (var zInstruction in outputInstructions
         .Where(t => t.Key.StartsWith('z') && t.Value.gate != "XOR" && initialValues.Any(u => u.Key == t.Key.Replace('z', 'x') || u.Key == t.Key.Replace('z', 'y')))
         .OrderBy(t => t.Key)) {
         erroneousInstructionKeys.Add(zInstruction.Key);
         Console.WriteLine($"{zInstruction.Key} = {zInstruction.Value.inputAKey} {zInstruction.Value.gate} {zInstruction.Value.inputBKey}");
      }

      // All XOR's must either come from X and Y, or output to a Z
      foreach (var zInstruction in outputInstructions
         .Where(t => !t.Key.StartsWith('z') && !t.Value.inputAKey.StartsWith('x') && !t.Value.inputAKey.StartsWith('y') && t.Value.gate == "XOR")
         .OrderBy(t => t.Key)) {
         erroneousInstructionKeys.Add(zInstruction.Key);
         Console.WriteLine($"{zInstruction.Key} = {zInstruction.Value.inputAKey} {zInstruction.Value.gate} {zInstruction.Value.inputBKey}");
      }

      // All AND's must either come from X and Y, or come from an XOR and an OR
      foreach (var zInstruction in outputInstructions
         .Where(t => t.Value.gate == "AND"
            && !erroneousInstructionKeys.Contains(t.Key)
            && !erroneousInstructionKeys.Contains(t.Value.inputAKey)
            && !erroneousInstructionKeys.Contains(t.Value.inputBKey)
            && !(t.Value.inputAKey.StartsWith('x') || t.Value.inputAKey.StartsWith('y')))
         .OrderBy(t => t.Key)) {

         var aGate = outputInstructions[zInstruction.Value.inputAKey].gate;
         var bGate = outputInstructions[zInstruction.Value.inputBKey].gate;

         if (!(aGate == "OR" && bGate == "XOR" || aGate == "XOR" && bGate == "OR")) {
            erroneousInstructionKeys.Add(zInstruction.Key);
            Console.WriteLine($"{zInstruction.Key} = {aGate} {zInstruction.Value.gate} {bGate}");

         }
      }


      return string.Join(",", erroneousInstructionKeys.OrderBy(t => t));

   }

   private void ParseInput(out Dictionary<string, bool> initialValues, out List<(string gate, string inputAKey, string inputBKey, string outputKey)> instructions) {
      var inputLines = GetInputLines();
      initialValues = inputLines
         .TakeWhile(t => !string.IsNullOrEmpty(t))
         .Select(t => t.Split(":"))
         .Select(t => (key: t[0], value: t[1].Trim() == "1"))
         .ToDictionary(t => t.key, t => t.value);

      instructions = inputLines.Skip(initialValues.Count).Where(t => !string.IsNullOrEmpty(t)).Select(t => t.Trim().Split(" ")).Select(t => (t[1], t[0], t[2], t[4])).ToList();
   }
}
