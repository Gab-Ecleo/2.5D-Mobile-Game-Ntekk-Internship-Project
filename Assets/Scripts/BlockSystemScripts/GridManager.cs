using System;
using System.Collections.Generic;
using BlockSystemScripts.RowAndColumnScripts;
using BlockSystemScripts.RowAndColumnScripts.BlockSpawnerScripts;
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
        
        [Header("Grid Cell Data")]
        [SerializeField] private GameObject gridCellParent; //Parent game object for the grid cells
        [SerializeField] private GameObject gridCellPrefab; //Prefab to be referenced for each grid cells
        
        [Header("Manager Data")]
        [SerializeField] private GameObject alignmentManagerParent; //Parent game object for the row and column managers
        [SerializeField] private GameObject spawnManagerParent; //Parent game object for the SpawnManager
        [SerializeField] private GameObject rowManagerPrefab; //Prefab to be referenced for the row managers
        [SerializeField] private GameObject columnManagerPrefab; //Prefab to be referenced for the row managers
        [SerializeField] private GameObject spawnManagerPrefab;
        
        //DO NOT MODIFY IN INSPECTOR. Displays each Row and Column manager present in the scene. 
        [Header("Manager Lists")]
        [SerializeField] private List<RowManager> rowManagers;
        [SerializeField] private List<ColumnManager> columnManagers;
        [SerializeField] private List<BlockSpawner> spawnManagers;
        #endregion
        
        private void Awake()
        {
            //Gives the value of the determined starting positions to the current positions
            _xCurrentPos = xStartPos;
            _yCurrentPos = yStartPos;
            
            //Generate Row and Columns first before generating the cells
            GenerateManagers();
            GenerateCell();
        }
        

        //Generate Row and Columns
        private void GenerateManagers()
        {
            //Generate rows depending on the assigned count of rows
            for (var currentRowCount = 0; currentRowCount < rowCount; currentRowCount++)
            {
                //Generate row and add that row to the row list
                var row = Instantiate(rowManagerPrefab, new Vector3(0,0,0), quaternion.identity, alignmentManagerParent.transform);
                row.name = $"Row {currentRowCount + 1}";
                if (row.GetComponent<RowManager>()==null) row.AddComponent<RowManager>();
                rowManagers.Add(row.GetComponent<RowManager>());
            }

            //Generate columns and block spawners depending on the assigned count of columns
            for (var currentColumnCount = 0; currentColumnCount < columnCount; currentColumnCount++)
            {
                //Generate column and add that column to the column list
                var column = Instantiate(columnManagerPrefab, new Vector3(0,0,0), quaternion.identity, alignmentManagerParent.transform);
                column.name = $"Column {currentColumnCount + 1}";
                if (column.GetComponent<ColumnManager>()==null) column.AddComponent<ColumnManager>();
                columnManagers.Add(column.GetComponent<ColumnManager>());
                
                //Generate a block spawner and add that spawner to the spawner list
                var spawner = Instantiate(spawnManagerPrefab, new Vector3(0, 0, 0), quaternion.identity, spawnManagerParent.transform);
                spawner.name = $"Spawner {currentColumnCount + 1}";
                if (spawner.GetComponent<BlockSpawner>()==null) spawner.AddComponent<BlockSpawner>();
                spawnManagers.Add(spawner.GetComponent<BlockSpawner>());
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
                    
                    //Assigns this cell to respective row,column, and spawn manager.
                    rowManagers[currentRowCount].AddCell(cell);
                    columnManagers[currentColumnCount].AddCell(cell);
                    spawnManagers[currentColumnCount].AddCell(cell);
                    
                    //Assigns a the respective row and column manager to the cell for reference.
                    if (cell.GetComponent<GridCell>() == null) cell.AddComponent<GridCell>();
                    cell.GetComponent<GridCell>().AssignRowAndColumn(rowManagers[currentRowCount],columnManagers[currentColumnCount]);
                    
                    //Adds the determined increment to the current horizontal position for the next column.
                    _xCurrentPos += xPosIncrement;
                }
                //After the Column Loop ends, reset the current horizontal position back to the starting pos for the next set of column cycles.
                _xCurrentPos = xStartPos;
                //Adds the determined increment to the current vertical position for the next column.
                _yCurrentPos += yPosIncrement;
            }
        }
    }
}