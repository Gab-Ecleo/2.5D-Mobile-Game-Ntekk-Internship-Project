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
            _blockCounter = 0;
            foreach (var cell in GridCells)
            {
                if (cell.CurrentBlock == null) continue;
                if (cell.CurrentBlock.IsLanded())
                {
                    _blockCounter++;
                }
            }
            Debug.Log($"{gameObject.name}: {_blockCounter}/{GridCells.Count}");
            if (_blockCounter < GridCells.Count) return;
            FullColumn();
        }

        private void FullColumn()
        {
            Debug.Log("Column Full. GAME OVER. Restart the game");
            Time.timeScale = 0;
        }
    }
}