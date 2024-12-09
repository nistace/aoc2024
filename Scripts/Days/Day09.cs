namespace AdventOfCode2024.Scripts.Days;

public class Day09 : AbstractDay {
   protected override int Day => 9;

   public override string Part1() {
      var input = GetInputText().Trim().Select((t, i) => (length: int.Parse($"{t}"), position: i)).ToArray();
      var blocks = input.Where(t => t.position % 2 == 0).Select(t => (t.length, id: t.position / 2)).ToArray();
      var gaps = input.Where(t => t.position % 2 == 1).Select(t => t.length).ToArray();

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

   public override string Part2() => "";
}