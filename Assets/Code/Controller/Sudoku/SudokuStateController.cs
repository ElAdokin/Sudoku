using System.Collections;
using UnityEngine;

public class SudokuStateController 
{
    private SudokuState _state = new SudokuState();
    
    private readonly string _sudokuSaveFolder = "SudokuSave";
    
    public SudokuState State { get => _state; set => _state = value; }
    
    public IEnumerator SaveState()
    {
        InGameSaveState();

        yield break;
    }

    public void InGameSaveState()
    {
        SaveLoad<SudokuState>.Save(_state, _sudokuSaveFolder, "SudokuState");
    }

    public IEnumerator LoadState()
    {
        _state = SaveLoad<SudokuState>.Load(_sudokuSaveFolder, "SudokuState");

        Debug.Log(_state);

        yield break;
    }

    public void ResetState()
    {
        _state = null;
        SaveLoad<SudokuState>.DeleteSaveData(_sudokuSaveFolder, "SudokuState");
    }
    
    public bool CheckStateExistance()
    {
        return SaveLoad<SudokuState>.CheckSaveDataExits(_sudokuSaveFolder, "SudokuState");
    }
}