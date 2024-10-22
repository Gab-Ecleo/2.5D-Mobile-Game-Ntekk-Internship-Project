using System;
using System.Collections.Generic;
using UnityEngine;

namespace BlockSystemScripts.RowAndColumnScripts
{
    /// <summary>
    /// Parent Script of RowManager OR ColumnManager
    /// Adds the newly generated cell to the list of cells for this Column/Row Manager
    /// </summary>
    public class AlignmentManager : MonoBehaviour
    {
        //DO NOT MODIFY IN INSPECTOR. References each cell belonging to this manager, depending on the type of inheritor. 
        [Header("Test References. To be private")]
        [SerializeField] private List<GridCell> gridCells;
        public List<GridCell> GridCells => gridCells;

        //Called by the GridManager to add the newly generated cell to this list
        public void AddCell(GridCell item)
        {
            gridCells.Add(item);
        }

        //Called by the GridManager to clear this list before adding new items
        public void ClearList()
        {
            gridCells.Clear();
        }
    }
}
