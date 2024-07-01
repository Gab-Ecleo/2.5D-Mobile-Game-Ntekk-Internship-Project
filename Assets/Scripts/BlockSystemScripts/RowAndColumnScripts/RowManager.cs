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
        private int _blockCounter;
        //A validation call that checks the number of blocks in a row
        public void ValidateRow()
        {
            //Initialize a block counter variable
            _blockCounter = 0;
            foreach (var cell in GridCells)
            {
                //Checks each cell if there is currently a block in it
                if (cell.CurrentBlock == null) continue;
                //Then checks if the current block in that cell is Landed. If it is, add +1 ot the block counter
                if (cell.CurrentBlock.IsLanded())
                {
                    _blockCounter++;
                }
            }
            //If the number in the block counter is the same as the total number of cells in this row, clear the row
            if (_blockCounter < GridCells.Count) return;
            ClearRow();
        }

        //Clear this row 
        [ContextMenu("CLEAR ROW!")]
        public void ClearRow()
        {
            foreach (var cell in GridCells)
            {
                if (cell.CurrentBlock == null) continue;
                cell.DestroyBlock();

                // add points and update ui when cleared
                ScoreChanges();
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