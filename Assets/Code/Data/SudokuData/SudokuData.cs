using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SudokuData", menuName = "ScriptableObjects/Data/SudokuData", order = 1)]
public class SudokuData : ScriptableObject
{
    [SerializeField] private GridData _gridData;

    private readonly List<int> _initialSudoku = new List<int>() { 3,9,5,2,6,1,8,7,4,6,1,7,8,4,9,5,3,2,8,4,2,3,7,5,1,9,6,4,7,8,6,5,2,3,1,9,5,6,3,9,1,4,7,2,8,9,2,1,7,3,8,6,4,5,1,8,4,5,9,7,2,6,3,2,3,9,1,8,6,4,5,7,7,5,6,4,2,3,9,8,1};

    public GridData GridData => _gridData;
    public List<int> InitialSudoku => _initialSudoku;
}