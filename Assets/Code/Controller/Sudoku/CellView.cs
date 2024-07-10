using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    private bool _interactable;

    private int _correctValue;
    private int _currentValue = 0;
    
    [SerializeField] private Text _text;

    [SerializeField] private Toggle _sudokuToggle;

    public bool Interactable => _interactable;
    public Toggle SudokuToggle => _sudokuToggle;

    private SelectionGroupController _selectionController;

    [SerializeField] private Color _assingColor;
    private Color _correctColor = Color.black;

    public void Initialize()
    {
        _interactable = true;
        _selectionController = FindAnyObjectByType<SelectionGroupController>();
    }

    public void AssingCorrectValue(int value) 
    { 
        _correctValue = value;
    }

    public void SetInteractable(bool state) 
    { 
        _interactable = state;

        if (!state)
        {
            AssingValue(_correctValue);
            _sudokuToggle.isOn = false;
            _sudokuToggle.enabled = false;
            _text.color = _correctColor;
        }
    }

    public void AssingValue(int value)
    {
        _currentValue = value;

        if (_currentValue > 0 && _currentValue < 10)
        {
            _text.text = _currentValue.ToString();
            _text.color = _assingColor;
        }
        else
        {
            _text.text = string.Empty;
        }
    }

    public void ClearCellUI() 
    {
        if (!_interactable) 
            return;

        AssingValue(0);
    }

    public bool IsCorrect() 
    { 
        return _currentValue == _correctValue;
    }

    public void OnClickToggle(bool state) 
    {
        if(state)
            _selectionController.AssingSelection(this, _currentValue);
        else
            _selectionController.AssingSelection(this, 0);
    }
}
