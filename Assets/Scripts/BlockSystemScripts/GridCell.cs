using BlockSystemScripts.BlockScripts;
using BlockSystemScripts.BlockSpawnerScripts;
using BlockSystemScripts.RowAndColumnScripts;
using UnityEngine;

namespace BlockSystemScripts
{
    /// <summary>
    /// Handles the behavior of a grid cell
    /// </summary>
    public class GridCell : MonoBehaviour
    {
        //DO NOT MODIFY IN INSPECTOR. Displays the assigned row and column manager to this object
        [Header("Test References. To be private")]
        [SerializeField] private BlockScript currentBlock;
        [SerializeField] private RowManager assignedRow;
        [SerializeField] private ColumnManager assignedColumn;
        [SerializeField] private BlockSpawner assignedSpawner;
        [SerializeField] private int rowIndex, columnIndex;

        public BlockScript CurrentBlock => currentBlock;
        public RowManager AssignedRow => assignedRow;
        public ColumnManager AssignedColumn => assignedColumn;
        public BlockSpawner AssignedSpawner  => assignedSpawner;
        public int RowIndex => rowIndex;
        public int ColumnIndex => columnIndex;

        //On the generation of this cell, assign the respective managers to it. 
        public void AssignRowAndColumn(RowManager designatedRow, ColumnManager designatedColumn, BlockSpawner designatedSpawner, int rowNum, int colNum)
        {
            assignedRow = designatedRow;
            assignedColumn = designatedColumn;
            assignedSpawner = designatedSpawner;

            rowIndex = rowNum;
            columnIndex = colNum;
        }

        public void FillCellSlot(BlockScript block)
        {
            currentBlock = block;
        }

        public void EmptyCellSlot()
        {
            currentBlock = null;
        }

        public void DestroyBlock()
        {
            Destroy(currentBlock.gameObject);
            EmptyCellSlot();
        }
    }
}