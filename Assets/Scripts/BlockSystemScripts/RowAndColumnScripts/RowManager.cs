﻿using BlockSystemScripts.BlockScripts;
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
        [SerializeField] private ScoresSO _playerScore;
        [SerializeField] private PlayerStatsSO _playerCurrStats;
     
        //A validation call that checks the number of blocks in a row
        public void ValidateRow()
        {
            //Initialize a block counter variable
            var blockCounter = 0;
            foreach (var cell in GridCells)
            {
                //Checks each cell if there is currently a block in it
                if (cell.CurrentBlock == null) continue;
                //Checks if the block in this current cell is a powerup or not
                if (cell.CurrentBlock.BlockType == BlockType.PowerUp) continue;
                //Then checks if the current block in that cell is Landed. If it is, add +1 ot the block counter
                if (cell.CurrentBlock.BlockState is BlockState.Landed or BlockState.CanPickUp)
                {
                    blockCounter++;
                }
            }
            //If the number in the block counter is the same as the total number of cells in this row, clear the row
            if (blockCounter < GridCells.Count) return;
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
                
                //A validation run for checking if the blocks in the row are homogenous
                sameColorCount++;
                if (sameColorCount<=1)
                {
                    //if this is the first block cleared, set the color of this block as reference for the homogenous color clear
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
                
                cell.DestroyBlock();

                // add points and update ui when cleared
                ScoreChanges();
            }
            
            //Behavior for a homogenous color clear
            if (sameColorCount == GridCells.Count)
            {
                Debug.Log("SAME COLORED ROW COMPLETED");
            }
        }

        [ContextMenu("Test clear ")]
        public void ScoreChanges()
        {
            int pointsToAdd = _playerScore.PointsToAdd;
            int multiplier = _playerScore.Multiplier;
            bool hasMultiplier = _playerCurrStats.hasMultiplier; 

            GameEvents.ON_SCORE_CHANGES?.Invoke(pointsToAdd, multiplier, hasMultiplier);
            GameEvents.ON_UI_CHANGES?.Invoke();

            //Debug.Log(_playerScore.Points);
        }

    }
}