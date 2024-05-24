using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private void Awake()
    {
        // Setup singleton pattern
        if (Instance != null)
        {
            Debug.LogError($"There's more than one UnitActionSystem {transform} - {Instance}");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        // Input for Left Mouse Button
        if (Input.GetMouseButtonDown(0))
        {
            // Select a unit
            if (TryHandleUnitSelection()) return;

            // Move selected unit
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                selectedUnit.GetMoveAction().Move(mouseGridPosition);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            selectedUnit.GetSpinAction().Spin();
        }
    }

    private bool TryHandleUnitSelection()
    {
        // Set a ray from the screen to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Shoot a raycast infinite distance to only look for units and update the selected unit if it finds one
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        // ? does a null check and only continues if not null - is equivalent to checking if there are subscribers by checking if it is != null
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        // to keep field private
        return selectedUnit;
    }
}
