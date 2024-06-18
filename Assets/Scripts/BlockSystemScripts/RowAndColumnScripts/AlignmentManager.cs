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
        [SerializeField] private List<GameObject> gridCells;

        public virtual void AddCell(GameObject item)
        {
            gridCells.Add(item);
        }
    }
}
