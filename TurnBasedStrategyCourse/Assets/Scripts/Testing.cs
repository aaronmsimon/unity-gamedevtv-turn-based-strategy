using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    private void Start()
    {
    }

    private void Update()
    {
        gridSystemVisual.HideAllGridPositions();
        gridSystemVisual.ShowGridPositionList(unit.GetMoveAction().GetValidActionGridPositionList());
    }
}
