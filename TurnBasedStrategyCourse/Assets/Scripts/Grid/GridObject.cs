public class GridObject
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return "<color=#E46252>x</color><color=#FFFFFF> " + gridPosition.x + ",</color> <color=#5E88F5>z</color><color=#FFFFFF> " + gridPosition.z + "</color>";
    }
}
