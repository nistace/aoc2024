using System.Text;
using AdventOfCode2024.Scripts.Common;

namespace AdventOfCode2024.Scripts.Days;

public class Day21 : AbstractDay {
   protected override int Day => 21;

   public override string Part1() {
      ParseInput(out var combinations);
      
      // @formatter:off
      var numericKeypad = new Keypad(new Dictionary<Vector2Int, char> {
         { (0, 0), '7' }, { (1, 0), '8' }, { (2, 0), '9' },
         { (0, 1), '4' }, { (1, 1), '5' }, { (2, 1), '6' },
         { (0, 2), '1' }, { (1, 2), '2' }, { (2, 2), '3' },
                            { (1, 3), '0' }, { (2, 3), Keys.Activate },
      });
      
      var directionalKeypad = new Keypad(new Dictionary<Vector2Int, char> {
                                                           { (1, 0), Keys.Directionals[Vector2Int.Up] },   { (2, 0), Keys.Activate },
         { (0, 1), Keys.Directionals[Vector2Int.Left] }, { (1, 1), Keys.Directionals[Vector2Int.Down] }, { (2, 1), Keys.Directionals[Vector2Int.Right] },
      });
      // @formatter:on

      var keypads = new[] { numericKeypad, directionalKeypad, directionalKeypad };

      /*     DebugCombinationToWrite(keypads, "179A");
           DebugWrittenCombination(keypads, "<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A");
           DebugWrittenCombination(keypads, "<<vAA>A^>AA<Av>A^AvA^A<<vA^>>AAvA^A<vA^>AA<A>A<<vA>A^>AAA<Av>A^A");
     */
      var complexitySum = combinations.Sum(combination => int.Parse(combination.Replace($"{Keys.Activate}", "")) * GetSolutionLength(keypads, combination));
      return $"{complexitySum}";
   }

   private static int GetSolutionLength(Keypad[] keypads, string combination) => keypads.Aggregate(combination, (current, keypad) => keypad.GetCombinationToWrite(current)).Length;

   private static void DebugCombinationToWrite(Keypad[] keypads, string combination) {
      Console.WriteLine(combination);
      var result = combination;
      foreach (var keypad in keypads) {
         result = keypad.GetCombinationToWrite(result);
         Console.WriteLine(result);
      }
      Console.WriteLine();
   }

   private static void DebugWrittenCombination(Keypad[] keypads, string combination) {
      Console.WriteLine(combination);
      var result = combination;
      foreach (var keypad in keypads.Reverse()) {
         result = keypad.GetCombinationWrittenBy(result);
         Console.WriteLine(result);
      }
      Console.WriteLine();
   }

   public override string Part2() {
      return "";
   }

   public void ParseInput(out List<string> combinations) {
      combinations = GetInputLines().Where(t => !string.IsNullOrEmpty(t)).ToList();
   }

   private static class Keys {
      public static IReadOnlyDictionary<Vector2Int, char> Directionals { get; } =
         new Dictionary<Vector2Int, char> { { Vector2Int.Left, '<' }, { Vector2Int.Right, '>' }, { Vector2Int.Up, '^' }, { Vector2Int.Down, 'v' } };

      public static char Activate => 'A';
   }

   private class Keypad {
      private Dictionary<Vector2Int, char> PositionKeys { get; }
      private Dictionary<char, Vector2Int> KeyPositions { get; }

      private Dictionary<(char, char), IReadOnlyList<Vector2Int>> ShortestPaths { get; } = new Dictionary<(char, char), IReadOnlyList<Vector2Int>>();

      public Keypad(Dictionary<Vector2Int, char> keys) {
         PositionKeys = keys.ToDictionary();
         KeyPositions = keys.Values.ToDictionary(t => t, t => keys.First(u => u.Value == t).Key);

         GenerateShortestPaths([Vector2Int.Right, Vector2Int.Up, Vector2Int.Down, Vector2Int.Left]);
      }

      private void GenerateShortestPaths(Vector2Int[] preferredDirections) {
         var openNodes = new Dictionary<char, (IReadOnlyList<Vector2Int> combination, int minDirectionIndex)>();

         foreach ((var fromKey, var fromKeyPosition) in KeyPositions) {
            openNodes.Add(fromKey, (Array.Empty<Vector2Int>(), 0));

            while (openNodes.Count > 0) {
               (var node, (var combination, var firstDirectionIndex)) = openNodes.OrderBy(t => t.Value.combination.Count).First();
               var nodePosition = KeyPositions[node];
               openNodes.Remove(node);
               ShortestPaths.Add((fromKey, node), TrySortCombination(combination, fromKeyPosition, preferredDirections));

               for (var directionIndex = firstDirectionIndex; directionIndex < preferredDirections.Length; ++directionIndex) {
                  var direction = preferredDirections[directionIndex];
                  var nextNodePosition = nodePosition + direction;
                  if (PositionKeys.TryGetValue(nextNodePosition, out var nextNode) && !ShortestPaths.ContainsKey((fromKey, nextNode)) && !openNodes.ContainsKey(nextNode)) {
                     openNodes.Add(nextNode, (combination.Append(direction).ToArray(), directionIndex));
                  }
               }
            }
         }
      }

      private IReadOnlyList<Vector2Int> TrySortCombination(IReadOnlyList<Vector2Int> combination, Vector2Int origin, Vector2Int[] orderedDirectionCosts) {
         var sortedCombination = combination.OrderByDescending(t => Array.IndexOf(orderedDirectionCosts, t)).ToArray();

         if (sortedCombination.SequenceEqual(combination)) return combination;

         var sortedCheckKeyPosition = origin;
         foreach (var combinationStep in sortedCombination) {
            sortedCheckKeyPosition += combinationStep;
            if (!PositionKeys.ContainsKey(sortedCheckKeyPosition)) return combination;
         }
         return sortedCombination;
      }

      public string GetCombinationToWrite(string written) {
         var pointingKey = Keys.Activate;
         var instructions = new StringBuilder();
         foreach (var key in written) {
            if (pointingKey != key) {
               foreach (var directionalInstruction in ShortestPaths[(pointingKey, key)]) {
                  instructions.Append(Keys.Directionals[directionalInstruction]);
               }
            }
            instructions.Append(Keys.Activate);
            pointingKey = key;
         }
         return instructions.ToString();
      }

      // ReSharper disable once UnusedMember.Local
      public string GetCombinationWrittenBy(string combination) {
         var pointedPosition = KeyPositions[Keys.Activate];
         var written = new StringBuilder();
         foreach (var key in combination) {
            if (key == Keys.Activate) {
               written.Append(PositionKeys[pointedPosition]);
            }
            else {
               pointedPosition += Keys.Directionals.Single(t => t.Value == key).Key;
            }
         }
         return written.ToString();
      }
   }
}