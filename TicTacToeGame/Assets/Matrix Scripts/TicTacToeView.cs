using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeView : MonoBehaviour
{
    public int inputRows;
    public int inputColumns;
    public float horizontalSpacing;
    public float verticalSpacing;

    float horizontalDistance = 0;
    float verticalDistance = 0;

    public GameObject cellPrefab;

    public Camera mainCamera;

    List<List<GameObject>> unityCellsList = new List<List<GameObject>>();

    TicTacToeGrid tTTGrid;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGrid();
    }

    // Update is called once per frame
    void Update()
    {
        tTTGrid.OnWin();
        tTTGrid.OnDraw();
    }

    // This method initializes the tictactor grid and subscribes to relevant delegates
    void InitializeGrid()
    {
        tTTGrid = new TicTacToeGrid(inputRows, inputColumns);
        tTTGrid.onCellCreated += OnCellCreated;
        tTTGrid.onRowCreated += AddRow;
        tTTGrid.onAllCellsDone += AllignGrid;
        tTTGrid.InitializeCells();
    }

    // This method is called via a delegate to create an instance of the cell prefab
    public void OnCellCreated(Cell cell)
    {
        GameObject cellView = Instantiate(cellPrefab, this.transform.position, cellPrefab.transform.rotation);
        unityCellsList[cell.GetRow()].Add(cellView);
        cellView.GetComponent<UnityCell>().SetCell(cell);
    }

    // This method adds row to the cells instances list
    public void AddRow()
    {
        unityCellsList.Add(new List<GameObject>());
    }

    // This method alligns the grid in shape and also checks for horizontal and vertical inputs
    public void AllignGrid()
    {
        float tempLength = Mathf.Sqrt(unityCellsList.Count);
        for (int x = 0; x < unityCellsList.Count; x++)
        {
            for (int y = 0; y < unityCellsList[x].Count; y++)
            {
                Vector3 tempPosition = GetPosition(x, y);
                unityCellsList[x][y].transform.position = tempPosition;
            }
            IncreaseVerticalDistance();
            ResetHorizontalDistance();
        }
        PositionCamera(GetCenterPosition());
    }

    // This method calculates the position for a cell location
    Vector3 GetPosition(int x, int y)
    {
        Vector3 tempVector3 = new Vector3(unityCellsList[x][y].GetComponent<UnityCell>().cellScript.GetColumn() + horizontalDistance, 0, unityCellsList[x][y].GetComponent<UnityCell>().cellScript.GetRow() + verticalDistance);
        horizontalDistance = horizontalDistance + horizontalSpacing;
        return tempVector3;
    }

    // This method increases the vertical distance for allignment according to the vertical spacing
    void IncreaseVerticalDistance()
    {
        verticalDistance = verticalDistance + verticalSpacing;
    }

    // This method resets the vertical distance for allignment
    void ResetHorizontalDistance()
    {
        horizontalDistance = 0;
    }

    // This method places the camera at the appropriate position
    void PositionCamera(Vector3 position)
    {
        mainCamera.transform.position = position;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, unityCellsList.Count + 1f + horizontalSpacing + verticalSpacing, mainCamera.transform.position.z);
    }

    // This method gets the center position of all the elements of the cell instances list using Bounds class
    Vector3 GetCenterPosition()
    {
        var bounds = new Bounds(unityCellsList[0][0].transform.position, Vector3.zero);
        for (int x = 0; x < unityCellsList.Count; x++)
        {
            for (int y = 0; y < unityCellsList[x].Count; y++)
            {
                bounds.Encapsulate(unityCellsList[x][y].transform.position);
            }
        }
        return bounds.center;
    }

    // This method unsubscribes from all delegates on destroy
    private void OnDestroy()
    {
        tTTGrid.onCellCreated -= OnCellCreated;
        tTTGrid.onRowCreated -= AddRow;
        tTTGrid.onAllCellsDone -= AllignGrid;
    }
}
