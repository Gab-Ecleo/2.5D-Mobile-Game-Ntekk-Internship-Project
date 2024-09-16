using System;
using BlockSystemScripts.BlockScripts;
using BlockSystemScripts.BlockSpawnerScripts;
using BlockSystemScripts.RowAndColumnScripts;
using EventScripts;
using UnityEngine;

namespace BlockSystemScripts
{
    /// <summary>
    /// Handles the behavior of a grid cell
    /// </summary>
    public class GridCell : MonoBehaviour
    {
        #region VARIABLES
        //DO NOT MODIFY IN INSPECTOR. Displays the assigned row and column manager to this object
        [Header("Assigned Scripts. DO NOT MODIFY")]
        [SerializeField] private BlockScript currentBlock;
        [SerializeField] private RowManager assignedRow;
        [SerializeField] private ColumnManager assignedColumn;
        [SerializeField] private BlockSpawner assignedSpawner;
        
        [Header("Assigned Indexes. DO NOT MODIFY")]
        [SerializeField] private int rowIndex;
        [SerializeField] private int columnIndex;
        
        [Header("Column Neighbors. DO NOT MODIFY")]
        [SerializeField] private GridCell previousYCell;
        [SerializeField] private GridCell nextYCell;
        
        [Header("Row Neighbors. DO NOT MODIFY")]
        [SerializeField] private GridCell previousXCell;
        [SerializeField] private GridCell nextXCell;

        public BlockScript CurrentBlock => currentBlock;
        public RowManager AssignedRow => assignedRow;
        public ColumnManager AssignedColumn => assignedColumn;
        public BlockSpawner AssignedSpawner  => assignedSpawner;
        public int RowIndex => rowIndex;
        public int ColumnIndex => columnIndex;
        public GridCell PreviousYCell => previousYCell;
        public GridCell NextYCell => nextYCell;
        
        public GridCell PreviousXCell => previousXCell;
        public GridCell NextXCell => nextXCell;
        #endregion
        

        //On the generation of this cell, assign the respective managers to it. 
        public void AssignRowAndColumn(RowManager designatedRow, ColumnManager designatedColumn, BlockSpawner designatedSpawner, int rowNum, int colNum)
        {
            assignedRow = designatedRow;
            assignedColumn = designatedColumn;
            assignedSpawner = designatedSpawner;

            rowIndex = rowNum;
            columnIndex = colNum;
        }

        //Triggered once after the generation of the grid has been completed
        private void InitializePrevAndNextCell()
        {
            #region UP AND DOWN NEIGHBORS

            if (rowIndex != 0)
            {
                previousYCell = assignedColumn.GridCells[rowIndex - 1];
            }

            if (rowIndex != assignedColumn.GridCells.Count - 1)
            {
                nextYCell = assignedColumn.GridCells[rowIndex + 1];
            }

            #endregion

            #region LEFT AND RIGHT NEIGBORS

            if (columnIndex != 0)
            {
                previousXCell = assignedRow.GridCells[columnIndex - 1];
            }
            
            if (columnIndex != assignedRow.GridCells.Count - 1)
            {
                nextXCell = assignedRow.GridCells[columnIndex + 1];
            }

            #endregion
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

        private void OnEnable()
        {
            GridGenerationEvents.OnGridCompletion += InitializePrevAndNextCell;
        }

        private void OnDisable()
        {
            GridGenerationEvents.OnGridCompletion -= InitializePrevAndNextCell;
        }
    }
}