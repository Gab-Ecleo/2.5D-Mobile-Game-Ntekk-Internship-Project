using System;
using System.Collections.Generic;
using BlockSystemScripts.BlockSpawnerScripts;
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
        
        [Header("Grid Cell Data")]
        [SerializeField] private GameObject gridCellParent; //Parent game object for the grid cells
        [SerializeField] private GameObject gridCellPrefab; //Prefab to be referenced for each grid cells
        
        [Header("Manager Data")]
        [SerializeField] private GameObject alignmentManagerParent; //Parent game object for the row and column managers
        [SerializeField] private BlockSpawnersManager spawnManagerParent; //Parent game object for the SpawnManager
        [SerializeField] private GameObject rowManagerPrefab; //Prefab to be referenced for the row managers
        [SerializeField] private GameObject columnManagerPrefab; //Prefab to be referenced for the row managers
        [SerializeField] private GameObject spawnManagerPrefab;//Prefab to be referenced for the spawn managers
        
        //DO NOT MODIFY IN INSPECTOR. Displays each list of managers present in the scene. 
        [Header("Manager Lists")]
        [SerializeField] private List<RowManager> rowManagersList;
        [SerializeField] private List<ColumnManager> columnManagersList;
        [SerializeField] private List<BlockSpawner> spawnManagersList;
        #endregion
        
        private void Awake()
        {
            //Resets the lists before starting the game to make sure there are no objects in them. 
            rowManagersList.Clear();
            columnManagersList.Clear();
            spawnManagersList.Clear();
            spawnManagerParent.ClearList();
            
            //Gives the value of the determined starting positions to the current positions
            _xCurrentPos = xStartPos;
            _yCurrentPos = yStartPos;
            
            //Generate Row and Columns, flush the lists of cells, then generate the cells
            GenerateManagers();
            FlushLists();
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
                rowManagersList.Add(row.GetComponent<RowManager>());
            }

            //Generate columns and block spawners depending on the assigned count of columns
            for (var currentColumnCount = 0; currentColumnCount < columnCount; currentColumnCount++)
            {
                //Generate column and add that column to the column list
                var column = Instantiate(columnManagerPrefab, new Vector3(0,0,0), quaternion.identity, alignmentManagerParent.transform);
                column.name = $"Column {currentColumnCount + 1}";
                if (column.GetComponent<ColumnManager>()==null) column.AddComponent<ColumnManager>();
                columnManagersList.Add(column.GetComponent<ColumnManager>());
                
                //Generate a block spawner and add that spawner to the spawner list, then add that spawner to it's parent's list
                var spawner = Instantiate(spawnManagerPrefab, new Vector3(0, 0, 0), quaternion.identity, spawnManagerParent.transform);
                spawner.name = $"Spawner {currentColumnCount + 1}";
                var blockSpawnerScript = spawner.GetComponent<BlockSpawner>();
                if (blockSpawnerScript==null) spawner.AddComponent<BlockSpawner>();
                spawnManagersList.Add(blockSpawnerScript);
                blockSpawnerScript.SetSpawnManager();
                spawnManagerParent.AddSpawnersToList(blockSpawnerScript);
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
                    if (cell.GetComponent<GridCell>() == null) cell.AddComponent<GridCell>();
                    var cellScript = cell.GetComponent<GridCell>();

                    //Assigns this cell to respective row,column, and spawn manager.
                    rowManagersList[currentRowCount].AddCell(cellScript);
                    columnManagersList[currentColumnCount].AddCell(cellScript);
                    spawnManagersList[currentColumnCount].AddCell(cellScript);
                    
                    //Assigns a the respective managers to the cell for reference.
                    cellScript.AssignRowAndColumn(rowManagersList[currentRowCount],columnManagersList[currentColumnCount], spawnManagersList[currentColumnCount],currentRowCount, currentColumnCount);
                    
                    //Adds the determined increment to the current horizontal position for the next column.
                    _xCurrentPos += xPosIncrement;
                }
                //After the Column Loop ends, reset the current horizontal position back to the starting pos for the next set of column cycles.
                _xCurrentPos = xStartPos;
                //Adds the determined increment to the current vertical position for the next column.
                _yCurrentPos += yPosIncrement;
            }
        }
        
        //Flushes all the managers lists
        private void FlushLists()
        {
            foreach (var manager in rowManagersList)
            {
                manager.ClearList();
            }

            foreach (var manager in columnManagersList)
            {
                manager.ClearList();
            }
            foreach (var manager in spawnManagersList)
            {
                manager.ClearList();
            }
        }
    }
}