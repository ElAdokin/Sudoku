using UnityEngine;
using UnityEngine.UI;

public class ValueController : MonoBehaviour
{
    [SerializeField] private SelectionGroupController _selectionGroupController;
    [SerializeField] private int _value;

    public void OnClickToggle() 
    {
        if (GetComponent<Toggle>().isOn)
        {
            _selectionGroupController.AssingValue(_value);
        }
    }
}
