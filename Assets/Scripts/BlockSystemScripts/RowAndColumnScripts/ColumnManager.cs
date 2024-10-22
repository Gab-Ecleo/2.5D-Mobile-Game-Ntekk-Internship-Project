﻿using BlockSystemScripts.BlockScripts;
using EventScripts;
using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Handles the behavior that validates the column of cells
    /// </summary>
    public class ColumnManager : AlignmentManager
    {
        private int _blockCounter;
        //A validation call that checks the count of blocks in a column
        public void ValidateColumn()
        {
            //Initialize a block counter variable
            _blockCounter = 0;
            foreach (var cell in GridCells)
            {
                //Checks each cell if there is currently a block in it
                if (cell.CurrentBlock == null) continue;
                //Checks if the block in this current cell is a powerup or not
                if (cell.CurrentBlock.BlockType == BlockType.PowerUp) continue;
                //Then checks if the current block in that cell is Landed. If it is, add +1 ot the block counter
                if (cell.CurrentBlock.BlockState is BlockState.Landed or BlockState.CanPickUp)
                {
                    _blockCounter++;
                }
            }
            //If the number in the block counter is the same as the total number of cells in this column, Trigger the method
            if (_blockCounter < GridCells.Count) return;
            FullColumn();
        }
        
        //Triggers a game over because of the full column 
        private void FullColumn()
        {
            Debug.Log("Column Full. GAME OVER. Restart the game");
            AudioEvents.ON_PLAYER_DEATH?.Invoke();
            GameEvents.TRIGGER_GAMEEND_SCREEN?.Invoke(true);
            GameEvents.IS_GAME_OVER?.Invoke(true);
            LocalStorageEvents.OnSaveCurrencyData?.Invoke();
            Time.timeScale = 0;
        }
    }
}