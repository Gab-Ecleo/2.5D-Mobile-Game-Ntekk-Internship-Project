using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Used for Column Managers that validates their respective column of cells
    /// </summary>
    public class ColumnManager : AlignmentManager
    {
        public override void AddCell(GameObject item)
        {
            base.AddCell(item);
        }
        
        //Called by the GridCell class to validate the column assigned to this Manager.
        public void ValidateColumn()
        {
        }
    }
}