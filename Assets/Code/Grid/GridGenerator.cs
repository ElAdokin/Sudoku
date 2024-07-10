using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    private SudokuData _sudokuData;
    
    private GridData _gridData;

    public CellView[] _cellObjects;
    private int _indexObject;
    
    private Cell[,] _cells;
    private CellView[,] _cellsViews;

    [SerializeField] private List<Vector2Int> _staticCells = new List<Vector2Int>();

    private int _randomNumber;
    private int _staticNumbers = 25;
    private int _staticCellsForBox;
    private List<int> _staticIndexes = new List<int>();
    private List<int> _randomIndexesForRest = new List<int>();
    private int _restStaticCellsForBox;
    private int _timesToNoAddIndex;

    private Vector2Int _newStaticCellPosition;

    private int _errors;

    private Vector2Int[] _selectedBox = new Vector2Int[9];

    
    private readonly Vector2Int[] _upLeft = { new Vector2Int(0, 8), new Vector2Int(1, 8), new Vector2Int(2, 8), new Vector2Int(0, 7), new Vector2Int(1, 7), new Vector2Int(2, 7), new Vector2Int(0, 6), new Vector2Int(1, 6), new Vector2Int(2, 6) };
    private readonly Vector2Int[] _up = { new Vector2Int(3, 8), new Vector2Int(4, 8), new Vector2Int(5, 8), new Vector2Int(3, 7), new Vector2Int(4, 7), new Vector2Int(5, 7), new Vector2Int(3, 6), new Vector2Int(4, 6), new Vector2Int(5, 6) };
    private readonly Vector2Int[] _upRight = { new Vector2Int(6, 8), new Vector2Int(7, 8), new Vector2Int(8, 8), new Vector2Int(6, 7), new Vector2Int(7, 7), new Vector2Int(8, 7), new Vector2Int(6, 6), new Vector2Int(7, 6), new Vector2Int(8, 6) };
    private readonly Vector2Int[] _left = { new Vector2Int(0, 5), new Vector2Int(1, 5), new Vector2Int(2, 5), new Vector2Int(0, 4), new Vector2Int(1, 4), new Vector2Int(2, 4), new Vector2Int(0, 3), new Vector2Int(1, 3), new Vector2Int(2, 3) };
    private readonly Vector2Int[] _center = { new Vector2Int(3, 5), new Vector2Int(4, 5), new Vector2Int(5, 5), new Vector2Int(3, 4), new Vector2Int(4, 4), new Vector2Int(5, 4), new Vector2Int(3, 3), new Vector2Int(4, 3), new Vector2Int(5, 3) };
    private readonly Vector2Int[] _right = { new Vector2Int(6, 5), new Vector2Int(7, 5), new Vector2Int(8, 5), new Vector2Int(6, 4), new Vector2Int(7, 4), new Vector2Int(8, 4), new Vector2Int(6, 3), new Vector2Int(7, 3), new Vector2Int(8, 3) };
    private readonly Vector2Int[] _downLeft = { new Vector2Int(0, 2), new Vector2Int(1, 2), new Vector2Int(2, 2), new Vector2Int(0, 1), new Vector2Int(1, 1), new Vector2Int(2, 1), new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(2, 0) };
    private readonly Vector2Int[] _down = { new Vector2Int(3, 2), new Vector2Int(4, 2), new Vector2Int(5, 2), new Vector2Int(3, 1), new Vector2Int(4, 1), new Vector2Int(5, 1), new Vector2Int(3, 0), new Vector2Int(4, 0), new Vector2Int(5, 0) };
    private readonly Vector2Int[] _downRight = { new Vector2Int(6, 2), new Vector2Int(7, 2), new Vector2Int(8, 2), new Vector2Int(6, 1), new Vector2Int(7, 1), new Vector2Int(8, 1), new Vector2Int(6, 0), new Vector2Int(7, 0), new Vector2Int(8, 0) };

    private List<Vector2Int[]> _boxes = new List<Vector2Int[]>();

    private Vector2Int[] _checkerBox;

    private int _reRollCounter;
    private int _accelerateCounter;

    private SelectionGroupController _selectionGroupController;

    public IEnumerator InitializeGridGenerator(SudokuData data) 
    {
        _sudokuData = data;
        _gridData = data.GridData;

        Debug.Log(_cellObjects.Length);

        _selectionGroupController = FindObjectOfType<SelectionGroupController>();
        GameObject.Find("CheckButton").GetComponent<Button>().onClick.AddListener(() => CheckSudoku());

        _boxes = new List<Vector2Int[]>()
        {
            _downLeft, 
            _down, 
            _downRight, 
            _left, 
            _center, 
            _right, 
            _upLeft, 
            _up, 
            _upRight
        };

        _cells = new Cell[_gridData.GridSize.x, _gridData.GridSize.y];
        _cellsViews = new CellView[_gridData.GridSize.x, _gridData.GridSize.y];

        yield return CreateCellsViews();

        if (_sudokuData.CheckFileExistance())
        {
            yield return _sudokuData.LoadSudoku();
            
            yield return AssingSudokuValues(_sudokuData.NextSudoku);

            yield return SelectStaticCells();

            yield return CleanSudoku();

            GetComponent<GameInstaller>().InitializeSceneController();

            yield return CreateCells();

            yield return GenerateSudoku();

            yield return AssingNextSudokuValues();

            yield return _sudokuData.SaveSudoku();
        }
        else 
        {
            yield return CreateCells();

            yield return GenerateSudoku();

            yield return AssingSudokuValuesFromCells();
            
            yield return AssingNextSudokuValues();

            yield return SelectStaticCells();

            yield return CleanSudoku();

            yield return _sudokuData.SaveSudoku();

            GetComponent<GameInstaller>().InitializeSceneController();
        }

        yield break;
    }

    private IEnumerator CreateCellsViews() 
    {
        for (int x = 0; x < _cellsViews.GetLength(0); x++)
        {
            for (int y = 0; y < _cellsViews.GetLength(1); y++)
            {
                _indexObject = (x * _cellsViews.GetLength(0)) + y;
                _cellsViews[x, y] = _cellObjects[_indexObject];
                _cellsViews[x, y].gameObject.name = "Cell_" + x.ToString() + "_" + y.ToString();
                _cellsViews[x, y].Initialize();
            }
        }

        yield break;
    }

    private IEnumerator AssingSudokuValues(List<int> _values) 
    {
        for (int x = 0; x < _cellsViews.GetLength(0); x++)
        {
            for (int y = 0; y < _cellsViews.GetLength(1); y++)
            {
                _cellsViews[x, y].AssingCorrectValue(_values[(y * 9) + x]);
            }
        }

        yield break;
    }

    private IEnumerator CreateCells()
    {
        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                _cells[x, y] = new Cell(new Vector2Int(x, y), GetSudokuBoxForCell(new Vector2Int(x, y)));
            }
        }

        yield break;
    }

    private IEnumerator AssingSudokuValuesFromCells()
    {
        for (int x = 0; x < _cellsViews.GetLength(0); x++)
        {
            for (int y = 0; y < _cellsViews.GetLength(1); y++)
            {
                _cellsViews[x, y].AssingCorrectValue(_cells[x, y].GetCorrectValue());
            }
        }

        yield break;
    }

    private IEnumerator AssingNextSudokuValues()
    {
        _sudokuData.NextSudoku.Clear();

        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            for (int y = 0; y < _cells.GetLength(1); y++)
            {
                _sudokuData.NextSudoku.Add(_cells[x,y].GetCorrectValue());
            }
        }

        

        yield break;
    }

    public IEnumerator GenerateSudoku()
    {
        _reRollCounter = 0;

        for (int i = 0; i < 9;)
        {
            SetSelectedBox((SudokuBox)i);

            yield return ResetBoxCells();

            while (!IsAllBoxPrepare())
            {
                for (int e = 1; e < 10; e++)
                {
                    for (int j = 0; j < _selectedBox.Length; j++)
                    {
                        if (_cells[_selectedBox[j].x, _selectedBox[j].y].GetEntrophy() == e)
                        {
                            yield return _cells[_selectedBox[j].x, _selectedBox[j].y].Collapse(this);
                            yield return ClearValueFromSodoku(_cells[_selectedBox[j].x, _selectedBox[j].y]);
                        }
                    }
                }
            }

            if (!CorrectValueAssingment())
            {
                _reRollCounter++;
                
                Debug.Log("Reset Generation Box");
                
                if (i > 0 && _reRollCounter >= 3)
                {
                    _reRollCounter = 0;
                    
                    _accelerateCounter++;

                    yield return ResetBoxCells();
                    
                    i--;

                    Debug.Log("BackTracking");
                }
            }
            else 
            {
                if (_accelerateCounter >= 1)
                {
                    _accelerateCounter = 0;

                    yield return ResetBoxCells();

                    i--;

                    Debug.Log("Double BackTracking");
                }
                else
                {
                    _reRollCounter = 0;
                    _accelerateCounter = 0;
                    
                    i++;

                    Debug.Log("Continue Generation");
                }
            }
        }

        Debug.Log("End Generation");
        
        yield break;
    }

    private bool IsAllBoxPrepare()
    {
        for (int j = 0; j < _selectedBox.Length; j++)
        {
            if (_cells[_selectedBox[j].x, _selectedBox[j].y].GetEntrophy() != 0) return false;
        }

        return true;
    }

    private bool CorrectValueAssingment()
    {
        for (int i = 0; i < _selectedBox.Length; i++)
        {
            if (_cells[_selectedBox[i].x, _selectedBox[i].y].GetCorrectValue() == 0) 
                return false;
        }

        return true;
    }

    private IEnumerator ResetBoxCells()
    {
        for (int i = 0; i < _selectedBox.Length; i++)
        {
            if (_cells[_selectedBox[i].x, _selectedBox[i].y].GetCorrectValue() != 0)
            {
                yield return AddValueFromSodoku(_cells[_selectedBox[i].x, _selectedBox[i].y]);
            }
        }

        yield break;
    }

    private IEnumerator ClearValueFromSodoku(Cell cell) 
    {
        GetSudokuBox(cell.Box);

        for (int i = 0; i < _checkerBox.Length; i++)
        {
            if (_checkerBox[i] != cell.Position)
                _cells[_checkerBox[i].x, _checkerBox[i].y].RemoveValue(cell.GetCorrectValue());
        }

        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            if (x != cell.Position.x)
                _cells[x, cell.Position.y].RemoveValue(cell.GetCorrectValue());
        }

        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            if (y != cell.Position.y)
                _cells[cell.Position.x, y].RemoveValue(cell.GetCorrectValue());
        }

        yield break;
    }

    private IEnumerator AddValueFromSodoku(Cell cell)
    {
        GetSudokuBox(cell.Box);

        for (int i = 0; i < _checkerBox.Length; i++)
        {
            if (_checkerBox[i] != cell.Position && _cells[_checkerBox[i].x, _checkerBox[i].y].GetCorrectValue() == 0)
                _cells[_checkerBox[i].x, _checkerBox[i].y].AddValue(cell.GetCorrectValue());
        }

        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            if (x != cell.Position.x && _cells[x, cell.Position.y].GetCorrectValue() == 0)
                _cells[x, cell.Position.y].AddValue(cell.GetCorrectValue());
        }

        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            if (y != cell.Position.y && _cells[cell.Position.x, y].GetCorrectValue() == 0)
                _cells[cell.Position.x, y].AddValue(cell.GetCorrectValue());
        }

        yield return cell.ResetCell(this);

        yield break;
    }

    public bool IsValueAssigned(Cell cell, int value) 
    {
        GetSudokuBox(cell.Box);

        for (int i = 0; i < _checkerBox.Length; i++)
        {
            if (_checkerBox[i] != cell.Position)
                if (_cells[_checkerBox[i].x, _checkerBox[i].y].GetCorrectValue() == value)
                    return true;
        }

        for (int x = 0; x < _cells.GetLength(0); x++)
        {
            if (x != cell.Position.x)
                if (_cells[x, cell.Position.y].GetCorrectValue() == value)
                    return true;
        }

        for (int y = 0; y < _cells.GetLength(1); y++)
        {
            if (y != cell.Position.y)
                if (_cells[cell.Position.x, y].GetCorrectValue() == value)
                    return true;
        }

        return false;
    }

    private SudokuBox GetSudokuBoxForCell(Vector2Int position) 
    {
        if (_upLeft.Contains(position))
            return SudokuBox.UpLeft;
        else if (_up.Contains(position))
            return SudokuBox.Up;
        else if (_upRight.Contains(position))
            return SudokuBox.UpRight;
        else if (_left.Contains(position))
            return SudokuBox.Left;
        else if (_center.Contains(position))
            return SudokuBox.Center;
        else if (_right.Contains(position))
            return SudokuBox.Right;
        else if (_downLeft.Contains(position))
            return SudokuBox.DownLeft;
        else if (_down.Contains(position))
            return SudokuBox.Down;
        else if (_downRight.Contains(position))
            return SudokuBox.DownRight;
        else
            return SudokuBox.Center;
    }

    private void GetSudokuBox(SudokuBox box)
    {
        switch (box)
        {
            case SudokuBox.UpLeft:
                _checkerBox = _upLeft;
                break;
            case SudokuBox.Up:
                _checkerBox = _up;
                break;
            case SudokuBox.UpRight:
                _checkerBox = _upRight;
                break;
            case SudokuBox.Left:
                _checkerBox = _left;
                break;
            case SudokuBox.Center:
                _checkerBox = _center;
                break;
            case SudokuBox.Right:
                _checkerBox = _right;
                break;
            case SudokuBox.DownLeft:
                _checkerBox = _downLeft;
                break;
            case SudokuBox.Down:
                _checkerBox = _down;
                break;
            case SudokuBox.DownRight:
                _checkerBox = _downRight;
                break;
        }
    }

    private void SetSelectedBox(SudokuBox box)
    {
        switch (box)
        {
            case SudokuBox.UpLeft:
                _selectedBox = _upLeft;
                break;
            case SudokuBox.Up:
                _selectedBox = _up;
                break;
            case SudokuBox.UpRight:
                _selectedBox = _upRight;
                break;
            case SudokuBox.Left:
                _selectedBox = _left;
                break;
            case SudokuBox.Center:
                _selectedBox = _center;
                break;
            case SudokuBox.Right:
                _selectedBox = _right;
                break;
            case SudokuBox.DownLeft:
                _selectedBox = _downLeft;
                break;
            case SudokuBox.Down:
                _selectedBox = _down;
                break;
            case SudokuBox.DownRight:
                _selectedBox = _downRight;
                break;
        }
    }

    private IEnumerator SelectStaticCells() 
    {
        _staticCells.Clear();
        _staticIndexes.Clear();

        _staticIndexes = new List<int>() 
        { 
            0,1,2,3,4,5,6,7,8
        };

        _staticCellsForBox = _staticNumbers / 9;

        if (_staticNumbers % 9 == 0)
        {
            for (int i = 0; i < _boxes.Count; i++)
            {
                for (int j = 0; j < _staticCellsForBox; j++)
                {
                    _randomNumber = _staticIndexes[Random.Range(0, _staticIndexes.Count)];
                    _newStaticCellPosition = _boxes[i].ElementAt(_randomNumber);
                    _staticCells.Add(_newStaticCellPosition);
                    _staticIndexes.Remove(_randomNumber);
                }

                _staticIndexes = new List<int>()
                {
                    0,1,2,3,4,5,6,7,8
                };
            }

            _staticIndexes.Clear();
        }
        else 
        {
            _restStaticCellsForBox = _staticCellsForBox + 1;

            _randomIndexesForRest = new List<int>()
            {
                    0,1,2,3,4,5,6,7,8
            };

            _timesToNoAddIndex = 9 - (_staticNumbers % 9);

            for (int i = 0; i < _timesToNoAddIndex; i++)
            {
                _randomIndexesForRest.RemoveAt(Random.Range(0, _randomIndexesForRest.Count));
            }

            Debug.Log(_randomIndexesForRest.Count);

            for (int i = 0; i < _boxes.Count; i++)
            {
                if (_randomIndexesForRest.Contains(i))
                {
                    for (int j = 0; j < _restStaticCellsForBox; j++)
                    {
                        _randomNumber = _staticIndexes[Random.Range(0, _staticIndexes.Count)];
                        _newStaticCellPosition = _boxes[i].ElementAt(_randomNumber);
                        _staticCells.Add(_newStaticCellPosition);
                        _staticIndexes.Remove(_randomNumber);
                    }

                    _staticIndexes = new List<int>()
                    {
                        0,1,2,3,4,5,6,7,8
                    };
                }
                else 
                {
                    for (int j = 0; j < _staticCellsForBox; j++)
                    {
                        _randomNumber = _staticIndexes[Random.Range(0, _staticIndexes.Count)];
                        Debug.Log(_randomNumber);
                        _newStaticCellPosition = _boxes[i].ElementAt(_randomNumber);
                        _staticCells.Add(_newStaticCellPosition);
                        _staticIndexes.Remove(_randomNumber);
                    }

                    _staticIndexes = new List<int>()
                    {
                        0,1,2,3,4,5,6,7,8
                    };
                }
            }

            _staticIndexes.Clear();
        }
        
        yield break;
    }

    private IEnumerator CleanSudoku() 
    {
        for (int i = 0; i < _staticCells.Count; i++)
        {
            _cellsViews[_staticCells[i].x, _staticCells[i].y].SetInteractable(false);
        }

        for (int x = 0; x < _cellsViews.GetLength(0); x++)
        {
            for (int y = 0; y < _cellsViews.GetLength(1); y++)
            {
                _cellsViews[x, y].ClearCellUI();
            }
        }

        yield break;
    }

    public void CheckSudoku()
    {
        _errors = 0;

        for (int x = 0; x < _cellsViews.GetLength(0); x++)
        {
            for (int y = 0; y < _cellsViews.GetLength(1); y++)
            {
                if (_cellsViews[x, y].IsCorrect())
                {
                    _cellsViews[x, y].SetInteractable(false);
                    _selectionGroupController.RemoveCell();
                }
                else
                {
                    _cellsViews[x, y].AssingValue(0);
                    _errors++;
                }
            }
        }

        if (_errors == 0)
            GetComponent<GameInstaller>().GoMainMenu();
    }
}
 