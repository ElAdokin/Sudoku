using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SudokuState
{
    public List<int> Sudoku = new List<int>();
    public List<Vector2Int> StaticCells = new List<Vector2Int>();
    public List<Vector2Int> SolveCells = new List<Vector2Int>();
    public int Duration;
    public int Errors;
    public int CheckTries;
}
