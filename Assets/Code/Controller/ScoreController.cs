using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreController
{
    private ScoresContainer _scoresContainer = new ScoresContainer();
    private List<ScoreInfo> _orderScores = new List<ScoreInfo>();

    public ScoresContainer ScoresContainer => _scoresContainer;

    public void AddScore(ScoreInfo scoreInfo)
    {
        Debug.Log("New Score: " + scoreInfo.Points + " " + scoreInfo.Date);

        LoadScores();

        _orderScores.Clear();

        if (_scoresContainer.Scores.Count > 1)
            _orderScores = _scoresContainer.Scores.OrderByDescending(scoreInfo => scoreInfo.Points).ToList();

        if (_orderScores.Count >= 10 && scoreInfo.Points <= _orderScores[_orderScores.Count - 1].Points) return;

        if (_orderScores.Count < 10)
            _orderScores.Add(scoreInfo);
        else
        {
            _orderScores.RemoveAt(_orderScores.Count - 1);
            _orderScores.Add(scoreInfo);
        }

        if (_orderScores.Count > 1)
        {
            _scoresContainer.Scores = _orderScores.OrderByDescending(scoreInfo => scoreInfo.Points).ToList();
        }
        else
        {
            _scoresContainer.Scores.Add(scoreInfo);
        }
        
        SaveScores();
    }

    private void SaveScores()
    {
        SaveLoad<ScoresContainer>.Save(_scoresContainer, "ScoresFolder", "Scores");
    }

    public void LoadScores()
    {
        _scoresContainer = SaveLoad<ScoresContainer>.Load("ScoresFolder", "Scores");

        if (_scoresContainer is null)
            _scoresContainer = new ScoresContainer();
    }
}
