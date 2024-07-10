using UnityEngine;
using UnityEngine.UI;

public class SelectionGroupController : MonoBehaviour
{
    [SerializeField] private CellView _assingCell;
    [SerializeField] private int _value;

    public void AssingSelection(CellView cell, int value)
    {
        _assingCell = cell;

        TurnOffToggles();

        _value = value;

        if (_value > 0)
            ActivateToggle(_value - 1);
    }

    private void ActivateToggle(int index)
    {
        transform.GetChild(index).GetComponent<Toggle>().isOn = true;
    }

    private void TurnOffToggles()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<Toggle>().isOn = false;
    }

    public void AssingValue(int value)
    {
        if (_assingCell == null)
        {
            TurnOffToggles();
            return;
        }

        _value = value;

        _assingCell.AssingValue(_value);
    }

    public void RemoveCell() 
    {
        AssingSelection(null, 0);
    }
}
