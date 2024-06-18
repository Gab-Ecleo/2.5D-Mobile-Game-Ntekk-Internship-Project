using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Inheritor of AlignmentManager
    /// Used for Row Managers that validates their respective row of cells
    /// </summary>
    public class RowManager : AlignmentManager
    {
        public override void AddCell(GameObject item)
        {
            base.AddCell(item);
        }

        //Called by the GridCell class to validate the row assigned to this Manager.
        public void ValidateRow()
        {
            
        }
    }
}