using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell 
{
    private Vector2Int _position;
    private SudokuBox _box;

    private List<int> _values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private int _value = 0;

    private int _randomIndex;
    private Randomizer _randomizer = new Randomizer();

    public Cell(Vector2Int position, SudokuBox box)
    {
        _position = position;
        _box = box;
    }

    public SudokuBox Box => _box;
    public Vector2Int Position => _position;

    public void RemoveValue(int value)
    {
        if (_values.Contains(value))
            _values.Remove(value);
    }

    public void AddValue(int value)
    {
        if (!_values.Contains(value))
            _values.Add(value);
    }

    public int GetCorrectValue()
    {
        return _value;
    }

    public IEnumerator Collapse(GridGenerator generator)
    {
        _randomIndex = _values.Count + 1;

        if (_values.Count > 0)
        {
            do
            {
                if (_randomIndex < _values.Count)
                    _values.RemoveAt(_randomIndex);

                _randomIndex = _randomizer.GetRamdonInt(0, _values.Count);
            }
            while (_values.Count > 0 && generator.IsValueAssigned(this, _values[_randomIndex]));

            if (_values.Count == 0)
            {
                _value = 0;
            }
            else
            {
                _value = _values[_randomIndex];
            }
        }
        else
        {
            _value = 0;
        }

        _values.Clear();

        yield break;
    }

    public IEnumerator ResetCell(GridGenerator generator)
    {
        for (int i = 1; i < 10; i++)
        {
            if (!generator.IsValueAssigned(this, i))
                _values.Add(i);
        }

        _value = 0;
        
        yield break;
    }
    
    public int GetEntrophy()
    {
        return _values.Count;
    }
}
