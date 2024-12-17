namespace AdventOfCode2024.Scripts.Days;

public class Day17 : AbstractDay {
   protected override int Day => 17;
   private const int A = 0;
   private const int B = 1;
   private const int C = 2;

   public override string Part1() {
      ParseInput(out var registers, out var programInstructions);
      var programRunner = new ProgramRunner(registers, programInstructions);
      programRunner.Run();

      return string.Join(",", programRunner.OutputValues);
   }

   public override string Part2() => "";

   private void ParseInput(out int[] registerValues, out int[] program) {
      var inputLines = GetInputLines();

      registerValues = new[] { "Register A: ", "Register B: ", "Register C: " }.Select(t => int.Parse(inputLines.First(line => line.StartsWith(t)).Split(t).Last().Trim())).ToArray();
      program = inputLines.First(t => t.StartsWith("Program: ")).Split("Program: ").Last().Trim().Split(",").Select(int.Parse).ToArray();
   }

   private class ProgramRunner {
      private int pointer;
      private readonly int[] registers;
      private readonly int[] programInstructions;
      private readonly Action<int>[] operations;
      private readonly List<int> outputValues = [];

      public IReadOnlyList<int> OutputValues => outputValues;

      public ProgramRunner(int[] registers, int[] programInstructions) {
         this.registers = registers;
         this.programInstructions = programInstructions;
         pointer = 0;
         operations = [Adv, Bxl, Bst, Jnz, Bxc, Out, Bdv, Cdv];
      }

      private void Adv(int operand) => Dv(A, operand);
      private void Bdv(int operand) => Dv(B, operand);
      private void Cdv(int operand) => Dv(C, operand);

      private void Dv(int register, int operand) {
         registers[register] = registers[A] / (int)Math.Pow(2, Combo(operand));
         pointer += 2;
      }

      private void Bxl(int operand) {
         registers[B] ^= operand;
         pointer += 2;
      }

      private void Bst(int operand) {
         registers[B] = Combo(operand) % 8;
         pointer += 2;
      }

      private void Jnz(int operand) => pointer = registers[A] == 0 ? pointer + 2 : operand;

      private void Bxc(int operand) {
         registers[B] ^= registers[C];
         pointer += 2;
      }

      private void Out(int operand) {
         outputValues.Add(Combo(operand) % 8);
         pointer += 2;
      }

      private int Combo(int operand) => operand switch {
         < 4 => operand,
         < 7 => registers[operand - 4],
         _ => 0
      };

      public void Run() {
         while (pointer < programInstructions.Length) {
            operations[programInstructions[pointer]](programInstructions[pointer + 1]);
         }
      }
   }
}