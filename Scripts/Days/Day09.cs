namespace AdventOfCode2024.Scripts.Days;

public class Day09 : AbstractDay {
   protected override int Day => 9;

   public override string Part1() {
      ParseInput(out var blocks, out var gaps);

      var checksum = 0L;
      var blockIndexToMove = blocks.Length - 1;
      var remainingInBlockToMove = blocks[blockIndexToMove].length;
      var currentPosition = 0;
      for (var index = 0; index <= blockIndexToMove; ++index) {
         var remainingItemsInBlock = index == blockIndexToMove ? remainingInBlockToMove : blocks[index].length;
         for (var blockItemIndex = 0; blockItemIndex < remainingItemsInBlock; ++blockItemIndex) {
            checksum += currentPosition * blocks[index].id;
            currentPosition++;
         }

         for (var gapItemIndex = 0; index < blockIndexToMove && gapItemIndex < gaps[index]; ++gapItemIndex) {
            while (index < blockIndexToMove && remainingInBlockToMove == 0) {
               blockIndexToMove--;
               remainingInBlockToMove = blocks[blockIndexToMove].length;
            }

            if (index < blockIndexToMove) {
               checksum += currentPosition * blocks[blockIndexToMove].id;
               remainingInBlockToMove--;
               currentPosition++;
            }
         }
      }

      return $"{checksum}";
   }

   public override string Part2() {
      ParseInput(out var blocks, out var gaps);

      var checksum = 0L;
      var movedBlocks = new bool[blocks.Length];
      var currentPosition = 0;
      for (var index = 0; index < blocks.Length; ++index) {
         if (movedBlocks[blocks[index].id]) {
            currentPosition += blocks[index].length;
         }
         else {
            for (var blockItemIndex = 0; blockItemIndex < blocks[index].length; ++blockItemIndex) {
               checksum += currentPosition * blocks[index].id;
               currentPosition++;
            }
         }

         if (index < gaps.Length) {
            var gapRemainingSpace = gaps[index];
            while (gapRemainingSpace > 0) {
               var blockToMove = blocks.LastOrDefault(t => t.id > index && !movedBlocks[t.id] && t.length <= gapRemainingSpace);
               if (blockToMove == default) {
                  currentPosition += gapRemainingSpace;
                  gapRemainingSpace = 0;
               }
               else {
                  movedBlocks[blockToMove.id] = true;
                  gapRemainingSpace -= blockToMove.length;
                  for (var blockItemIndex = 0; blockItemIndex < blockToMove.length; ++blockItemIndex) {
                     checksum += currentPosition * blockToMove.id;
                     currentPosition++;
                  }
               }
            }
         }
      }

      return $"{checksum}";
   }

   private void ParseInput(out (int length, int id)[] blocks, out int[] gaps) {
      var input = GetInputText().Trim().Select((t, i) => (length: int.Parse($"{t}"), position: i)).ToArray();
      blocks = input.Where(t => t.position % 2 == 0).Select(t => (t.length, id: t.position / 2)).ToArray();
      gaps = input.Where(t => t.position % 2 == 1).Select(t => t.length).ToArray();
   }
}