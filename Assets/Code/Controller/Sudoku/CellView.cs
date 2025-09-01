using UnityEngine;
using UnityEngine.UI;

public class CellView : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Toggle _sudokuToggle;
    [SerializeField] private CellViewData _cellViewData;

    private GridGenerator _gridGenerator;
    private Vector2Int _viewPosition;

    private bool _interactable;
    private int _correctValue;
    private int _value = 0;

    private SelectionGroupController _selectionController;

    public bool Interactable => _interactable;
    public Toggle SudokuToggle => _sudokuToggle;
    public int Value => _value;
    public int CorrectValue => _correctValue;

    public Vector2Int ViewPosition { get => _viewPosition; set => _viewPosition = value; }
    
    public void Initialize(GridGenerator gridGenerator, Vector2Int position)
    {
        _gridGenerator = gridGenerator;
        _viewPosition = position;
        _interactable = true;
        _selectionController = FindAnyObjectByType<SelectionGroupController>();
        transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().color = _cellViewData.SelectionColor;
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
            _text.color = _cellViewData.CorrectColor;
        }
    }

    public void AssingValue(int value)
    {
        _value = value;

        if (_value > 0 && _value < 10)
        {
            _text.text = _value.ToString();
            _text.color = _cellViewData.AssingColor;
        }
        else
        {
            _text.text = string.Empty;
        }

        _gridGenerator.AddCellViewToChekList(this);    
    }

    public void ClearCellUI() 
    {
        if (!_interactable) 
            return;

        AssingValue(0);
    }

    public bool IsCorrect() 
    { 
        return _value == _correctValue;
    }

    public void OnClickToggle(bool state) 
    {
        if(state)
            _selectionController.AssingSelection(this, _value);
        else
            _selectionController.AssingSelection(this, 0);
    }
}
