using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

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

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStarted?.Invoke(this, EventArgs.Empty);
                }
            }

            // conversely, could do it this way to avoid extra indentation:
            // if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            // {
            //     return;
            // }

            // if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            // {
            //     return;
            // }

            // SetBusy();
            // selectedAction.TakeAction(mouseGridPosition, ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        InvokeOnBusyChanged();
    }

    private void ClearBusy()
    {
        isBusy = false;
        InvokeOnBusyChanged();
    }

    private void InvokeOnBusyChanged()
    {
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Set a ray from the screen to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Shoot a raycast infinite distance to only look for units and update the selected unit if it finds one
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit == selectedUnit)
                    {
                        // unit is already selected
                        return false;
                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }

        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        // By design, we will always make sure every unit has a move action (this acts as a default)
        SetSelectedAction(unit.GetMoveAction());

        // ? does a null check and only continues if not null - is equivalent to checking if there are subscribers by checking if it is != null
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        // to keep field private
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }
}
