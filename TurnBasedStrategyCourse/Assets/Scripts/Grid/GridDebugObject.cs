using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    public GridObject GetGridObject()
    {
        return this.gridObject;
    }
}
