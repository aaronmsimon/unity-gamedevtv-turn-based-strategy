using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float stoppingDistance = .1f;
    [SerializeField] private int maxMoveDistance = 4;

    [Header("Animation")]
    [SerializeField] private Animator unitAnimator;

    private Vector3 targetPosition;
    
    protected override void Awake()
    {
        base.Awake();

        // Setting the starting target to the current position so it doesn't try to move to 0, 0
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (!isActive) return;

        // Get move direction based on the current vs target position
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        // Move to within a set stopping distance so there's no jittering (though I didn't experience this)
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            // Move to the new position
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            // Set animation parameter
            unitAnimator.SetBool("IsWalking", true);
        } else
        {
            // Unset animation parameter if not moving
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }

        // Rotate based on move direction
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }

    public void Move(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        // Set target position
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        isActive = true;
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    // If it's not within the grid bounds
                    continue;
                }

                if (unitGridPosition == testGridPosition)
                {
                    // Same grid position where the unit is already at
                    continue;
                }

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position already occupied with another unit
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public override string GetActionName()
    {
        return "Move";
    }
}
