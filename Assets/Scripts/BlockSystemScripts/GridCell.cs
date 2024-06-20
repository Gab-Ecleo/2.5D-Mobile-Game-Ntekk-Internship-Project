using BlockSystemScripts.BlockScripts;
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
        [SerializeField] private BlockScript currentBlock;
        [SerializeField] private RowManager assignedRow;
        [SerializeField] private ColumnManager assignedColumn;
        [SerializeField] private int rowNumber, columnNumber;
        
        public BlockScript CurrentBlock => currentBlock;
        public RowManager AssignedRow => assignedRow;
        public ColumnManager AssignedColumn => assignedColumn;
        public int RowNumber => rowNumber;
        public int ColumnNumber => columnNumber;

        //On the generation of this cell, assign the respective row and column manager to it. 
        public void AssignRowAndColumn(RowManager designatedRow, ColumnManager designatedColumn, int rowNum, int colNum)
        {
            assignedRow = designatedRow;
            assignedColumn = designatedColumn;

            rowNumber = rowNum;
            columnNumber = colNum;
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