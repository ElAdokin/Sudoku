using System.Collections.Generic;

[System.Serializable]
public class SavedSudokuData
{
    public List<int> NextSudoku = new List<int>();

    public SavedSudokuData(List<int> nextSudoku)
    {
        NextSudoku.Clear();

        foreach (var item in nextSudoku) 
        { 
            NextSudoku.Add(item);
        }
    }
}
