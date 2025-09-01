using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedSudokuController 
{
    private List<int> _nextSudoku = new List<int>();

    private SavedSudokuData _savedData;

    private readonly string _sudokuSaveFolder = "SudokuSave";

    private string _dataString;
    private string _dataStringSaved;
    
    public List<int> NextSudoku { get => _nextSudoku; set => _nextSudoku = value; }

    public IEnumerator SaveSudoku()
    {
        _savedData = new SavedSudokuData(_nextSudoku);

        SaveLoad<SavedSudokuData>.Save(_savedData, _sudokuSaveFolder, "NextSudoku");

        NextSudoku.Clear();

        yield break;
    }

    public IEnumerator LoadSudoku()
    {
        _savedData = SaveLoad<SavedSudokuData>.Load(_sudokuSaveFolder, "NextSudoku");

        Debug.Log(_savedData);

        _nextSudoku.Clear();

        for (int i = 0; i < _savedData.NextSudoku.Count; i++)
        {
            _nextSudoku.Add(_savedData.NextSudoku[i]);
        }

        yield break;
    }

    public bool CheckNextSudokuExistance()
    {
        return SaveLoad<SavedSudokuData>.CheckSaveDataExits(_sudokuSaveFolder, "NextSudoku");
    }
}
