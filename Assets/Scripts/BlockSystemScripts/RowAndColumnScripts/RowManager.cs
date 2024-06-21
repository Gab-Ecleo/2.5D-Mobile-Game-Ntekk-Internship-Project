using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Handles the behavior that validates the row of cells
    /// </summary>
    public class RowManager : AlignmentManager
    {
        private int _blockCounter;
        //A validation call that checks the number of blocks in a row
        public void ValidateRow()
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
            ClearRow();
        }

        [ContextMenu("CLEAR ROW!")]
        public void ClearRow()
        {
            foreach (var cell in GridCells)
            {
                if (cell.CurrentBlock == null) continue;
                cell.DestroyBlock();
            }
            Debug.Log("ROW CLEARED");
        }
    }
}