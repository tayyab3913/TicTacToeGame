using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityCell : MonoBehaviour
{
    public List<GameObject> cellOptions;
    private Cell.Status activeStatus;
    public Cell cellScript = new Cell();

    // Start is called before the first frame update
    void Start()
    {
        //SetState(cellScript.GetStatus());
        cellScript.onStatusUpdate += SetState;
    }

    // This method updates the visual cell based on input status
    public void SetState(Cell.Status status)
    {
        for (int i = 0; i < cellOptions.Count; i++)
        {
            if ((int)status == i)
            {
                cellOptions[i].SetActive(true);
            } else
            {
                cellOptions[i].SetActive(false);
            }
        }
    }

    // This method overrides the monobehaviour function to check for mouse click
    private void OnMouseDown()
    {
        cellScript.OnInteraction();
    }

    // This method unsubscribes from all delegates on destroy
    private void OnDestroy()
    {
        cellScript.onStatusUpdate -= SetState;
    }

    // This method gives the cell reference to this script
    public void SetCell(Cell cellScript)
    {
        this.cellScript = cellScript;
        SetState(cellScript.GetStatus());
    }
}
