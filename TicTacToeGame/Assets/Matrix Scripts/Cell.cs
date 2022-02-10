using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private int rowNumber;
    private int colNumber;
    private Status status = Status.none;

    public delegate void OnStatusUpdate(Status status);
    public event OnStatusUpdate onStatusUpdate;

    public delegate void OnCellInteraction(int rows, int cols);
    public event OnCellInteraction onCellInteraction;

    void InitializeCell()
    {
        this.status = Status.none;
        rowNumber = 0;
        colNumber = 0;
    }
    public Cell()
    {
        InitializeCell();
    }

    public Cell(int row, int col)
    {
        this.status = Status.none;
        rowNumber = row;
        colNumber = col;
    }

    public int GetRow()
    {
        return rowNumber;
    }

    public int GetColumn()
    {
        return colNumber;
    }

    public void SetStatus(Status tempStatus)
    {
        this.status = tempStatus;
        onStatusUpdate.Invoke(tempStatus);
    }

    public void OnInteraction()
    {
        onCellInteraction?.Invoke(GetRow(), GetColumn());
    }

    public Status GetStatus()
    {
        return status;
    }

    public enum Status { none, cross, circle, win, loose}
}
