[System.Serializable]
public class ScoreInfo
{
    public int Points;
    public string Date;

    public ScoreInfo(int points, string date)
    {
        Points = points;
        Date = date;
    }
}