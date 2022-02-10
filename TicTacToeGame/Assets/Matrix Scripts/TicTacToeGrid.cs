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

    public TicTacToeGrid(int rows, int cols) : base(rows, cols)
    {
        
    }

    public TicTacToeGrid(float[,] inputArray) : base(inputArray)
    {

    }

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

    public void TakeTurn(int rows, int cols)
    {
        if(GetValue(rows, cols) == 0 && !winDeclared)
        {
            SetValue(rows, cols, (int)currentTurn);
            ChangeTurn();
        }
    }

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
        Debug.Log("Game End. Player Won");
    }

    public void OnDraw()
    {
        if (winDeclared) return;
        if (CheckDraw())
        {
            Debug.Log("It's a draw. Nobody Wins");
            winDeclared = true;
        }
    }

    public bool CheckWin()
    {
        if(CheckRowsWin() || CheckColsWin() || CheckDiagnolWin() || CheckInverseDiagnolWin())
        {
            return true;
        }
        return false;
    }

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

    public bool CheckDiagnolWin()
    {
        if(IsDiagnolSame() && GetValue(0, 0) != 0)
        {
            return true;
        }
        return false;
    }

    public bool CheckInverseDiagnolWin()
    {
        if (IsInverseDiagnolSame() && GetValue(0, GetNoOfColumns()-1) != 0)
        {
            return true;
        }
        return false;
    }

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