using System;
using System.Collections.Generic;
using BlockSystemScripts.RowAndColumnScripts;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace BlockSystemScripts
{
    /// <summary>
    /// Generates Row and Columns, then generates their cells
    /// </summary>
    public class GridManager : MonoBehaviour
    {
        #region Variables
        [Header("Grid Count")]
        [SerializeField] private int rowCount = 7; //How many rows there are in the grid
        [SerializeField] private int columnCount =9; //How many columns there are in the grid
        
        [Header("Grid Starting Positions")]
        [SerializeField] private float xStartPos; //Starting Horizontal position of the grid
        [SerializeField] private float yStartPos; //Starting Vertical position of the grid
        private float _xCurrentPos;
        private float _yCurrentPos;
        
        [Header("Cell Increments")]
        [SerializeField] private float xPosIncrement; //Increment for each cells' Horizontal position. 
        [SerializeField] private float yPosIncrement; //Increment for each cells' Vertical position.
        
        [Header("Alignment Manager Data")]
        [SerializeField] private GameObject alignmentManagerParent; //Parent game object for the row and column managers
        [SerializeField] private GameObject rowManagerPrefab; //Prefab to be referenced for the row managers
        [SerializeField] private GameObject columnManagerPrefab; //Prefab to be referenced for the row managers
        
        [Header("Grid Cell Data")]
        [SerializeField] private GameObject gridCellParent; //Parent game object for the grid cells
        [SerializeField] private GameObject gridCellPrefab; //Prefab to be referenced for each grid cells
        
        //DO NOT MODIFY IN INSPECTOR. Displays each Row and Column manager present in the scene. 
        [Header("Row and Column Lists")]
        [SerializeField] private List<RowManager> rowManagers;
        [SerializeField] private List<ColumnManager> columnManagers;
        
        #endregion
        
        private void Awake()
        {
            //Gives the value of the determined starting positions to the current positions
            _xCurrentPos = xStartPos;
            _yCurrentPos = yStartPos;
            
            //Generate Row and Columns first before generating the cells
            GenerateRowAndColumnManagers();
            GenerateCell();
        }
        
        //Generate Row and Columns
        private void GenerateRowAndColumnManagers()
        {
            //Generate rows depending on the assigned count of rows
            for (var currentRowCount = 0; currentRowCount < rowCount; currentRowCount++)
            {
                //Instantiates the row manager game object, then gives it a name with its position in the list
                var item = Instantiate(rowManagerPrefab, new Vector3(0,0,0), quaternion.identity, alignmentManagerParent.transform);
                item.name = $"Row {currentRowCount + 1}";
                
                //if the row manager has no RowManager script, give it one
                if (item.GetComponent<RowManager>()==null)
                {
                    item.AddComponent<RowManager>();
                }
                
                //Adds this row manager to the list of row managers
                rowManagers.Add(item.GetComponent<RowManager>());
            }

            //Generate rows depending on the assigned count of columns
            for (var currentColumnCount = 0; currentColumnCount < columnCount; currentColumnCount++)
            {
                //Instantiates the row manager game object, then gives it a name with its position in the list
                var item = Instantiate(columnManagerPrefab, new Vector3(0,0,0), quaternion.identity, alignmentManagerParent.transform);
                item.name = $"Column {currentColumnCount + 1}";
                
                //if the row manager has no RowManager script, give it one
                if (item.GetComponent<ColumnManager>()==null)
                {
                    item.AddComponent<ColumnManager>();
                }
                
                //Adds this row manager to the list of row managers
                columnManagers.Add(item.GetComponent<ColumnManager>());
            }
        }
        
        //Generate Cells for their respective rows and columns
        private void GenerateCell()
        {
            //Row Loop. Assigns all the respective cells to the current row before moving on to the next row.
            for (var currentRowCount = 0; currentRowCount < rowCount; currentRowCount++)
            {
                //Column Loop. Assigns all the respective cells to the columns of the current row before moving to the next set of columns
                for (var currentColumnCount = 0; currentColumnCount < columnCount; currentColumnCount++)
                {
                    //Instantiate a cell, then gives it a name depending on which Row and Column it belongs to.
                    var cell = Instantiate(gridCellPrefab, new Vector3(_xCurrentPos, _yCurrentPos, 0), quaternion.identity, gridCellParent.transform);
                    cell.name = $"Cell R{currentRowCount + 1} C{currentColumnCount + 1}";
                    
                    //Assigns this cell to respective row and column manager.
                    rowManagers[currentRowCount].AddCell(cell);
                    columnManagers[currentColumnCount].AddCell(cell);
                    
                    //If this cell has no GridCell script, then give it one.
                    if (cell.GetComponent<GridCell>() == null)
                    {
                        cell.AddComponent<GridCell>();
                    }
                    
                    //Assigns a the respective row and column manager to the cell for reference.
                    cell.GetComponent<GridCell>().AssignRowAndColumn(rowManagers[currentRowCount],columnManagers[currentColumnCount]);
                    
                    //Adds the determined increment to the current horizontal position for the next column.
                    _xCurrentPos += xPosIncrement;
                }
                
                //After the Column Loop ends, reset the current horizontal position back to the starting pos for the next set of column cycles.
                _xCurrentPos = xStartPos;
                ////Adds the determined increment to the current vertical position for the next column.
                _yCurrentPos += yPosIncrement;
            }
        }
    }
}