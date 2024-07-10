using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "SudokuData", menuName = "ScriptableObjects/Data/SudokuData", order = 2)]
public class SudokuData : ScriptableObject
{
    [SerializeField] private GridData _gridData;

    public List<int> NextSudoku = new();

    private SavedData _savedData;
    private readonly string _sudokuSave = "SudokuSave";
    private string _dataString;
    private string _dataStringSaved;

    public GridData GridData => _gridData;

    public IEnumerator SaveSudoku()
    {
        _savedData = new SavedData(NextSudoku);

        _dataString = JsonUtility.ToJson(_savedData);
        _dataStringSaved = PlayerPrefs.GetString(_sudokuSave);
        
        if (!_dataString.Equals(_dataStringSaved))
        {
            PlayerPrefs.SetString(_sudokuSave, _dataString);
            PlayerPrefs.Save();
        }

        NextSudoku.Clear();

        yield break;
    }

    public IEnumerator LoadSudoku()
    {
        if (!CheckFileExistance()) yield break;

        _dataString = PlayerPrefs.GetString(_sudokuSave);

        _savedData = new SavedData(JsonUtility.FromJson<SavedData>(_dataString).NextSudoku);

        Debug.Log(_savedData);

        NextSudoku.Clear();

        for (int i = 0; i < _savedData.NextSudoku.Count; i++)
        {
            NextSudoku.Add(_savedData.NextSudoku[i]);
        }

        yield break;
    }

    public bool CheckFileExistance() 
    {
        return PlayerPrefs.HasKey(_sudokuSave);
    }
}