using BlockSystemScripts.BlockScripts;
using ScriptableData;
using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Handles the behavior that validates the row of cells
    /// </summary>
    public class RowManager : AlignmentManager
    {
        [SerializeField] private RowState rowState;
        
        [Header("Data References")]
        [SerializeField] private ScoresSO _playerScore;
        [SerializeField] private PlayerStatsSO _playerCurrStats;
     
        //A validation call that checks the number of blocks in a row
        public void ValidateRow()
        {
            //Initialize counters
            var blockCounter = 0;
            var sameColorCount = 0;
            var colorType = BlockType.None;
            foreach (var cell in GridCells)
            {
                //Checks each cell if there is currently a block in it
                if (cell.CurrentBlock == null) continue;
                //Checks if the block in this current cell is a powerup or not
                if (cell.CurrentBlock.BlockType == BlockType.PowerUp) continue;
                //Then checks if the current block in that cell is Landed. If it is, add +1 ot the block counter
                if (cell.CurrentBlock.BlockState is not (BlockState.Landed or BlockState.CanPickUp)) continue;
                blockCounter++;

                //checks if the validation implemented is homogeneous only clear
                if (rowState == RowState.HomoOnlyClear)
                {
                    //A validation run for checking if the blocks in the row are homogenous
                    sameColorCount++;
                    if (sameColorCount<=1)
                    {
                        //if this is the first block, set the color of this block as reference for the homogenous color clear
                        colorType = cell.CurrentBlock.BlockType;
                    }
                    else
                    {
                        //if the color type of the current block is different from the referenced color, reduce the same color counter
                        if (cell.CurrentBlock.BlockType != colorType)
                        {
                            sameColorCount--;
                        }
                    }
                }
            }
            //If the number in the block counter is the same as the total number of cells in this row, clear the row
            if (blockCounter < GridCells.Count) return;
            if (sameColorCount < GridCells.Count && rowState == RowState.HomoOnlyClear) return;
            ClearRow();
        }

        //Clear this row 
        [ContextMenu("CLEAR ROW!")]
        public void ClearRow()
        {
            var sameColorCount = 0;
            var colorType = BlockType.None;
            foreach (var cell in GridCells)
            {
                if (cell.CurrentBlock == null) continue;
                if (cell.CurrentBlock.BlockType == BlockType.PowerUp) continue;

                //checks if the validation implemented is not homogenous only clear
                if (rowState == RowState.HeteroClear)
                {
                    //A validation run for checking if the blocks in the row are homogenous
                    sameColorCount++;
                    if (sameColorCount<=1)
                    {
                        //if this is the first block, set the color of this block as reference for the homogenous color clear
                        colorType = cell.CurrentBlock.BlockType;
                    }
                    else
                    {
                        //if the color type of the current block is different from the referenced color, reduce the same color counter
                        if (cell.CurrentBlock.BlockType != colorType)
                        {
                            sameColorCount--;
                        }
                    }
                }
                cell.DestroyBlock();

                // add points and update ui when cleared
                ScoreChanges();
            }
            
            //Behavior for a homogenous color clear
             if (sameColorCount == GridCells.Count && rowState == RowState.HeteroClear)
             {
                 ColorScore(sameColorCount);
                 Debug.Log("SAME COLORED ROW COMPLETED");
             }
        }

        [ContextMenu("Test clear ")]
        public void ScoreChanges() // for ui test
        {
            GameEvents.ON_SCORE_CHANGES?.Invoke();
        }

        private void ColorScore(int pointsToAdd)
        {
            int multiplier = _playerScore.Multiplier;
            bool hasMultiplier = _playerCurrStats.stats.hasMultiplier;

            GameEvents.ON_SCORE_CHANGES?.Invoke();
        }
    }
}