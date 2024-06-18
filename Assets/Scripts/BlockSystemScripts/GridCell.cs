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
        [SerializeField] private RowManager assignedRow;
        [SerializeField] private ColumnManager assignedColumn;

        //On the generation of this cell, assign the respective row and column manager to it. 
        public void AssignRowAndColumn(RowManager designatedRow, ColumnManager designatedColumn)
        {
            assignedRow = designatedRow;
            assignedColumn = designatedColumn;

        }
    }
}