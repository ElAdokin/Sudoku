using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(SudokuController), typeof(SceneController))]
public class GameInstaller : MonoBehaviour
{
    [SerializeField] private GlobalData _globalData;
    [SerializeField] private Fader _fader;
    [SerializeField] private GameObject _finalResult;
    [SerializeField] private Button _inGameMainMenu;
    [SerializeField] private Button _finalMainMenu;
    [SerializeField] private Button _newSudoku;

    [SerializeField] private Text _pointsText;
    [SerializeField] private Text _infoText;
    
    private SudokuController _controller;
    private SceneController _sceneController;
    private ScoreController _scoreController;

    private bool _retry;

    private DateTime _beginGameTimeStamp;
    private TimeSpan _duration;

    private int _finalPoints;
    private string _finalResults;

    private void Awake()
    {
        _beginGameTimeStamp = DateTime.Now;
        
        _scoreController = new ScoreController();
        
        _sceneController = GetComponent<SceneController>();
        _fader.OnFadeEnd += CheckFadeState;

        _controller = gameObject.GetComponent<SudokuController>();
        _controller.Initialize(_globalData);

        _inGameMainMenu.onClick.AddListener(GoMainMenu);
        _finalMainMenu.onClick.AddListener(FinalGoMainMenu);
        _newSudoku.onClick.AddListener(NewSudoku);

        _finalResult.SetActive(false);
    }

    public void CheckFadeState(bool isFadeIn, bool fadeEnd)
    {
        if (fadeEnd && isFadeIn)
        {
            if (_retry)
                ReLoadScene();
            else
                LoadMainMenu();
        }
    }

    public void GoMainMenu()
    {
        SaveSudokuState();
        _retry = false;
        _fader.SetFade(true, _globalData.FadeDuration);
    }

    public void FinalGoMainMenu()
    {
        _controller.StateController.ResetState();
        _retry = false;
        _fader.SetFade(true, _globalData.FadeDuration);
    }

    public void NewSudoku()
    {
        _controller.StateController.ResetState();
        _retry = true;
        _fader.SetFade(true, _globalData.FadeDuration);
    }

    private void LoadMainMenu() 
    {
        _sceneController.GoMainMenu();
    }

    private void ReLoadScene()
    {
        _sceneController.GoGame();
    }

    public void StartGame() 
    {
        _fader.SetFade(false, _globalData.FadeDuration);
    }

    private int GetDurationGame()
    {
        _duration = DateTime.Now - _beginGameTimeStamp;
        _beginGameTimeStamp = DateTime.Now;

        return _duration.Seconds;
    }

    private int GetFinalPoints(SudokuState state)
    {
        return ((_globalData.BasePoints - (state.Errors * _globalData.ErrorsMultiplier)) - ((int)(state.Duration * _globalData.DurationMultiplier)) * (_globalData.Difficulty + 1)) / (state.CheckTries * _globalData.ChecksMultiplier);
    }

    public void SaveSudokuState()
    {
        _controller.StateController.State.Duration += GetDurationGame();
        _controller.StateController.InGameSaveState();
    }

    public void ActivateFinalResult()
    {
        _finalPoints = GetFinalPoints(_controller.StateController.State);

        _pointsText.text = string.Empty;
        _pointsText.text = "Points: " + _finalPoints.ToString();

        _controller.StateController.State.Duration += GetDurationGame();
        TimeSpan time = TimeSpan.FromSeconds(_controller.StateController.State.Duration);

        _infoText.text = string.Empty;
        _infoText.text = "Time: " + time.ToString(@"hh\:mm\:ss") + "\n" +
                        "Errors: " + _controller.StateController.State.Errors.ToString() + "\n" +
                        "Checks: " + _controller.StateController.State.CheckTries.ToString();

        _finalResult.SetActive(true);
        
        _scoreController.AddScore(new ScoreInfo(_finalPoints, DateTime.Now.ToString("dd/MM/yyyy HH:mm")));
    }
}
