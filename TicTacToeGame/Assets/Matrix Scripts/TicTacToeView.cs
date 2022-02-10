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

    void InitializeGrid()
    {
        tTTGrid = new TicTacToeGrid(inputRows, inputColumns);
        tTTGrid.onCellCreated += OnCellCreated;
        tTTGrid.onRowCreated += AddRow;
        tTTGrid.onAllCellsDone += AllignGrid;
        tTTGrid.InitializeCells();
    }

    public void OnCellCreated(Cell cell)
    {
        GameObject cellView = Instantiate(cellPrefab, this.transform.position, cellPrefab.transform.rotation);
        unityCellsList[cell.GetRow()].Add(cellView);
        cellView.GetComponent<UnityCell>().SetCell(cell);
    }

    public void AddRow()
    {
        unityCellsList.Add(new List<GameObject>());
    }

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

    Vector3 GetPosition(int x, int y)
    {
        Vector3 tempVector3 = new Vector3(unityCellsList[x][y].GetComponent<UnityCell>().cellScript.GetColumn() + horizontalDistance, 0, unityCellsList[x][y].GetComponent<UnityCell>().cellScript.GetRow() + verticalDistance);
        horizontalDistance = horizontalDistance + horizontalSpacing;
        return tempVector3;
    }

    void IncreaseVerticalDistance()
    {
        verticalDistance = verticalDistance + verticalSpacing;
    }

    void ResetHorizontalDistance()
    {
        horizontalDistance = 0;
    }

    void PositionCamera(Vector3 position)
    {
        mainCamera.transform.position = position;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, unityCellsList.Count + 1f + horizontalSpacing + verticalSpacing, mainCamera.transform.position.z);
    }

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
}
