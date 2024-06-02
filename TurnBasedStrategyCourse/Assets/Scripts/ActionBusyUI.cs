using System;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private Transform actionBusyContainer;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void UnitActionSystem_OnBusyChanged(object sender, EventArgs e)
    {
        Debug.Log("busy");
        BusyChangedEventArgs args = (BusyChangedEventArgs)e;
        actionBusyContainer.gameObject.SetActive(args.IsBusy);
    }
}
