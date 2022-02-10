using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeGrid : Matrix
{
    List<List<Cell>> cellGrid = new List<List<Cell>>();
    Cell.Status currentTurn = Cell.Status.cross;

    public delegate void OnCellCreated(Cell cell);
    public event OnCellCreated onCellCreated;

    public delegate void OnRowCreated();
    public event OnRowCreated onRowCreated;

    public delegate void OnAllCellsDone();
    public event OnAllCellsDone onAllCellsDone;

    private int rowSame = 999;
    private int colSame = 999;

    private bool winDeclared = false;

    // This method is not a method and a constructor
    public TicTacToeGrid(int rows, int cols) : base(rows, cols)
    {
        
    }

    // This method is not a method and a constructor
    public TicTacToeGrid(float[,] inputArray) : base(inputArray)
    {

    }

    // This method initializes cells according to the input values for the constructor and saves them in a list
    public void InitializeCells()
    {
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            cellGrid.Add(new List<Cell>());
            onRowCreated?.Invoke();
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                Cell cell = new Cell(x, y);
                cell.onCellInteraction += TakeTurn;
                cellGrid[x].Add(cell);
                onCellCreated?.Invoke(cell);
            }
        }
        onAllCellsDone?.Invoke();
    }

    // This method is an overridden method which is being called in the base class whenever data is changed there in any method
    public override void OnMatrixUpdate()
    {
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                cellGrid[x][y].SetStatus((Cell.Status)GetValue(x, y));
            }
        }
    }

    // This method sets matrix value according to current turn variable
    public void TakeTurn(int rows, int cols)
    {
        if(GetValue(rows, cols) == 0 && !winDeclared)
        {
            SetValue(rows, cols, (int)currentTurn);
            ChangeTurn();
        }
    }

    // This method changes the current turn variable
    void ChangeTurn()
    {
        if(currentTurn == Cell.Status.cross)
        {
            currentTurn = Cell.Status.circle;
        } else
        {
            currentTurn = Cell.Status.cross;
        }
    }

    // This method is called to do appropriate tasks when someone wins
    public void OnWin()
    {
        if (!CheckWin()) return;
        if (winDeclared) return;
        if (rowSame != 999)
        {
            SetRow(rowSame, (int)Cell.Status.win);
        }
        else if(colSame != 999)
        {
            SetColumn(colSame, (int)Cell.Status.win);
        }
        else if(CheckDiagnolWin())
        {
            SetDiagnol((int)Cell.Status.win);
        }
        else if (CheckInverseDiagnolWin())
        {
            SetInverseDiagnol((int)Cell.Status.win);
        }
        winDeclared = true;
        ChangeTurn();
        Debug.Log("Game End: " + currentTurn.ToString() + " Won");
    }

    // This method to perform appropriate tasks when the game draws
    public void OnDraw()
    {
        if (winDeclared) return;
        if (CheckDraw())
        {
            Debug.Log("It's a draw. Nobody Wins");
            winDeclared = true;
        }
    }

    // This method checks if the game is won or not
    public bool CheckWin()
    {
        if(CheckRowsWin() || CheckColsWin() || CheckDiagnolWin() || CheckInverseDiagnolWin())
        {
            return true;
        }
        return false;
    }

    // This method checks if any of the rows are same or not
    public bool CheckRowsWin()
    {
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            if (base.IsRowSame(x) && GetValue(x, 0) != 0)
            {
                rowSame = x;
                return true;
            }
        }
        return false;
    }

    // This method checks if any of the columns is same or not
    public bool CheckColsWin()
    {
        for (int x = 0; x < GetNoOfColumns(); x++)
        {
            if (IsColumnSame(x) && GetValue(0, x) != 0)
            {
                colSame = x;
                return true;
            }
        }
        return false;
    }

    // This method checks if the diagnol is same or not
    public bool CheckDiagnolWin()
    {
        if(IsDiagnolSame() && GetValue(0, 0) != 0)
        {
            return true;
        }
        return false;
    }

    // This method checks if inverse diagnol is same or not
    public bool CheckInverseDiagnolWin()
    {
        if (IsInverseDiagnolSame() && GetValue(0, GetNoOfColumns()-1) != 0)
        {
            return true;
        }
        return false;
    }

    // This method checks if the game is draw or not
    public bool CheckDraw()
    {
        if (CheckWin()) return false;
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                if(GetValue(x, y) == 0) return false;
            }
        }
        return true;
    }
}