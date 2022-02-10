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

    // This method is called to initialize the cell with defautl values
    void InitializeCell()
    {
        this.status = Status.none;
        rowNumber = 0;
        colNumber = 0;
    }

    // This method is not a method and a constructor
    public Cell()
    {
        InitializeCell();
    }

    // This method is not a method and a constructor
    public Cell(int row, int col)
    {
        this.status = Status.none;
        rowNumber = row;
        colNumber = col;
    }

    // This method returns the row number of the cell
    public int GetRow()
    {
        return rowNumber;
    }

    // This method returns the column number of the cell
    public int GetColumn()
    {
        return colNumber;
    }

    // This method sets the status of the cell
    public void SetStatus(Status tempStatus)
    {
        this.status = tempStatus;
        onStatusUpdate.Invoke(tempStatus);
    }

    // This method invokes a delegate when it is called
    public void OnInteraction()
    {
        onCellInteraction?.Invoke(GetRow(), GetColumn());
    }

    // This method returns the current status of the cell
    public Status GetStatus()
    {
        return status;
    }

    // Enum Status is defined here
    public enum Status { none, cross, circle, win, loose}
}
