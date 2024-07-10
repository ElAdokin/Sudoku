using System.Collections.Generic;

[System.Serializable]
public class SavedData 
{
    public List<int> NextSudoku = new List<int>();

    public SavedData(List<int> nextSudoku)
    {
        NextSudoku.Clear();

        foreach (var item in nextSudoku) 
        { 
            NextSudoku.Add(item);
        }
    }
}