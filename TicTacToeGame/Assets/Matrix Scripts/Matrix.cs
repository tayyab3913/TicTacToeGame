using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    protected int numOfRows;
    protected int numOfCols;
    protected int rowLength;
    protected int colLength;

    protected List<List<float>> matrixData;

    void InitializeMatrix()
    {
        matrixData = new List<List<float>>();
    }

    public Matrix()
    {
        InitializeMatrix();
    }

    public Matrix(int rows, int cols)
    {
        numOfRows = rows;
        numOfCols = cols;
        rowLength = numOfCols;
        colLength = numOfRows;
        InitializeMatrix();
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            matrixData.Add(new List<float>());
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                matrixData[x].Add(0);
            }
        }
    }

    public Matrix(float[,] array)
    {
        numOfRows = array.GetLength(0);
        numOfCols = array.GetLength(1);
        rowLength = numOfCols;
        colLength = numOfRows;
        InitializeMatrix();
        for (int x = 0; x < array.GetLength(0); x++)
        {
            matrixData.Add(new List<float>());
            for (int y = 0; y < array.GetLength(1); y++)
            {
                matrixData[x].Add(array[x, y]);
            }
        }
    }

    public int GetRowLength()
    {
        return rowLength;
    }

    public int GetColumnLength()
    {
        return colLength;
    }

    public int GetNoOfColumns()
    {
        return numOfCols;
    }

    public int GetNoOfRows()
    {
        return numOfRows;
    }

    public void SetMatrix(float[,] array)
    {
        for (int x = 0; x < array.GetLength(0); x++)
        {
            for (int y = 0; y < array.GetLength(1); y++)
            {
                matrixData[x][y] = array[x, y];
            }
        }
        OnMatrixUpdate();
    }

    public void PrintMatrix()
    {
        string matrixToPrint = "";
        for (int x = 0; x < matrixData.Count; x++)
        {
            for (int y = 0; y < matrixData[x].Count; y++)
            {
                matrixToPrint += matrixData[x][y] + "  ";
            }
            matrixToPrint += "\n";
        }
        Debug.Log(matrixToPrint);
    }

    public void SetValue(int row, int col, float value)
    {
        if (col < GetNoOfColumns() && row < GetNoOfRows())
        {
            matrixData[row][col] = value;
            OnMatrixUpdate();
        }
        else
        {
            Debug.Log("Index out of bounds");
        }
        
    }

    public float GetValue(int row, int col)
    {
        if (col < GetNoOfColumns() && row < GetNoOfRows())
        {
            return matrixData[row][col];
        }
        else
        {
            Debug.Log("Index out of bounds");
            return 0;
        }
    }

    public void SetRow(int row, float[] array)
    {
        if (row < GetNoOfRows())
        {
            for (int x = 0; x < array.Length; x++)
            {
                matrixData[row][x] = array[x];
            }
            OnMatrixUpdate();
        }
        else
        {
            Debug.Log("Index out of bounds");
        }
    }

    public void AddRow(float[] array)
    {
        matrixData.Add(new List<float>());
        for (int x = 0; x < array.Length; x++)
        {
            matrixData[matrixData.Count][x] = array[x];
        }
        numOfRows++;
        colLength++;
        OnMatrixUpdate();
    }

    public void SetColumn(int col, float[] array)
    {
        if (col < GetNoOfColumns())
        {
            for (int x = 0; x < array.Length; x++)
            {
                matrixData[x][col] = array[x];
            }
            OnMatrixUpdate();
        }
        else
        {
            Debug.Log("Index out of bounds");
        }
    }

    public void AddCol(float[] array)
    {
        for (int x = 0; x < array.Length; x++)
        {
            matrixData[x].Add(array[x]);
        }
        numOfCols++;
        rowLength++;
        OnMatrixUpdate();
    }

    public void SwapRows(int row1, int row2)
    {
        if (row1 > GetNoOfColumns() || row2 > GetNoOfRows() || row1 != row2)
        {
            Debug.Log("Index out of bound or row sizes not same");
            return;
        }
        float[] array1 = new float[matrixData[row1].Count];
        float[] array2 = new float[matrixData[row2].Count];
        for (int x = 0; x < matrixData[row1].Count; x++)
        {
            array1[x] = matrixData[row1][x];
        }
        for (int x = 0; x < matrixData[row2].Count; x++)
        {
            array2[x] = matrixData[row2][x];
        }
        for (int x = 0; x < matrixData[row1].Count; x++)
        {
            matrixData[row1][x] = array2[x];
        }
        for (int x = 0; x < matrixData[row2].Count; x++)
        {
            matrixData[row2][x] = array1[x];
        }
        OnMatrixUpdate();
    }

    public void SwapCols(int col1, int col2)
    {
        if (col1 > GetNoOfColumns() || col2 > GetNoOfColumns() || col1 != col2)
        {
            Debug.Log("Index out of bound or col sizes not same");
            return;
        }
        float[] array1 = new float[matrixData.Count];
        float[] array2 = new float[matrixData.Count];
        for (int x = 0; x < matrixData.Count; x++)
        {
            array1[x] = matrixData[x][col1];
        }
        for (int x = 0; x < matrixData.Count; x++)
        {
            array2[x] = matrixData[x][col2];
        }
        for (int x = 0; x < matrixData.Count; x++)
        {
            matrixData[x][col1] = array2[x];
        }
        for (int x = 0; x < matrixData.Count; x++)
        {
            matrixData[x][col2] = array1[x];
        }
        OnMatrixUpdate();
    }

    public Matrix AddMatrix(Matrix matrixToAdd)
    {
        if (GetRowLength() != matrixToAdd.GetRowLength() || GetColumnLength() != matrixToAdd.GetColumnLength())
        {
            Debug.Log("Matric sizes different");
            return new Matrix();
        }
        Matrix tempMatrix = new Matrix(GetColumnLength(), GetRowLength());
        for (int x = 0; x < GetColumnLength(); x++)
        {
            for (int y = 0; y < GetRowLength(); y++)
            {
                tempMatrix.SetValue(x, y, matrixData[x][y] + matrixToAdd.GetValue(x, y));
            }
        }
        return tempMatrix;
    }

    public Matrix SubtractMatrix(Matrix matrixToAdd)
    {
        if (matrixData.Count != matrixData.Count)
        {
            Debug.Log("Matric sizes different");
            return new Matrix();
        }
        Matrix tempMatrix = new Matrix(matrixData.Count, matrixData[0].Count);
        for (int x = 0; x < matrixData.Count; x++)
        {
            for (int y = 0; y < matrixData[0].Count; y++)
            {
                tempMatrix.SetValue(x, y, matrixData[x][y] - matrixToAdd.GetValue(x, y));
            }
        }
        return tempMatrix;
    }

    public void SetMatrix(float number)
    {
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            matrixData.Add(new List<float>());
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                matrixData[x][y] = number;
            }
        }
        OnMatrixUpdate();
    }

    public void SetRow(int row, float number)
    {
        if (row > GetNoOfRows()) return;
        for (int y = 0; y < GetNoOfColumns(); y++)
        {
            matrixData[row][y] = number;
        }
        OnMatrixUpdate();
    }

    public float[] GetRow(int row)
    {
        if (row > GetNoOfRows()) return new float[0];
        float[] tempArray = new float[GetNoOfRows()];
        for (int y = 0; y < GetNoOfColumns(); y++)
        {
            tempArray[y] = matrixData[row][y];
        }
        return tempArray;
    }

    public void SetColumn(int col, float number)
    {
        if (col > GetNoOfColumns()) return;
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                matrixData[y][col] = number;
            }
        OnMatrixUpdate();
    }

    public float[] GetColumn(int col)
    {
        if (col > GetNoOfColumns()) return new float[0];
        float[] tempArray = new float[GetNoOfColumns()];
        for (int y = 0; y < GetNoOfColumns(); y++)
        {
            tempArray[y] = matrixData[y][col];
        }
        return tempArray;
    }

    public float AddArrayMultiples(float[] array1, float[] array2)
    {
        float tempFloat = 0;
        float answer = 0;
        for (int x = 0; x < array1.Length; x++)
        {
            for (int y = 0; y < array2.Length; y++)
            {
                tempFloat = array1[x] * array2[y];
                answer += tempFloat;
            }
        }
        return answer;
    }

    public void SetDiagnol(float number)
    {
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            SetValue(x, x, number);
        }
        OnMatrixUpdate();
    }

    public void SetInverseDiagnol(float number)
    {
        int counter = GetNoOfColumns() - 1;
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            SetValue(x, counter, number);
            counter--;
        }
        OnMatrixUpdate();
    }

    public bool IsRowSame(int rowNumber)
    {
        if (GetNoOfColumns() < 2) return true;
        bool tempBool = false;
        float tempfloat = GetValue(rowNumber, 0);
        if(tempfloat == GetValue(rowNumber, 1))
        {
            tempBool = true;
        }
        for (int x = 0; x < GetNoOfColumns(); x++)
        {
            if(GetValue(rowNumber, x) != tempfloat)
            {
                tempBool = false;
            }
        }
        return tempBool;
    }

    public bool IsColumnSame(int colNumber)
    {
        if (GetNoOfRows() < 2) return true;
        bool tempBool = false;
        float tempfloat = GetValue(0, colNumber);
        if (tempfloat == GetValue(1, colNumber))
        {
            tempBool = true;
        }
        for (int x = 0; x < GetNoOfColumns(); x++)
        {
            if (GetValue(x, colNumber) != tempfloat)
            {
                tempBool = false;
            }
        }
        return tempBool;
    }

    public bool IsDiagnolSame()
    {
        if (GetNoOfColumns() < 2 && GetNoOfRows() < 2) return true;
        bool tempBool = false;
        float tempfloat = GetValue(0, 0);
        if (tempfloat == GetValue(1, 1))
        {
            tempBool = true;
        }
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            if (GetValue(x, x) != tempfloat)
            {
                tempBool = false;
            }
        }
        return tempBool;
    }

    public bool IsInverseDiagnolSame()
    {
        if (GetNoOfColumns() < 2 && GetNoOfRows() < 2) return true;
        int counter = GetNoOfColumns() - 1;
        bool tempBool = false;
        float tempfloat = GetValue(0, GetNoOfColumns() - 1);
        if (tempfloat == GetValue(1, GetNoOfColumns() - 2))
        {
            tempBool = true;
        }
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            if (GetValue(x, counter) != tempfloat)
            {
                tempBool = false; 
            }
            counter--;
        }
        return tempBool;
    }

    public Matrix Multiply(Matrix inputMatrix)
    {
        Matrix tempMatrix = new Matrix(GetNoOfRows(), inputMatrix.GetNoOfColumns());
        float tempFloat = 0;
        int counter = 0;
        for (int x = 0; x < GetNoOfRows(); x++)
        {
            counter = 0;
            for (int y = 0; y < GetNoOfColumns(); y++)
            {
                tempFloat = AddArrayMultiples(GetRow(x), GetColumn(y));
                tempMatrix.SetValue(x, counter, tempFloat);
                counter++;
            }
        }
        return tempMatrix;
    }

    public virtual void OnMatrixUpdate()
    {
        
    }
}
