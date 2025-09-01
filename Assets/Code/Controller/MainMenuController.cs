using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneController))]
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GlobalData _globalData;
    [SerializeField] private Fader _fader;
    [SerializeField] private Text _scoreText;

    [SerializeField] private Slider _difficultySlider;
    [SerializeField] private Text _difficultyText;
    [SerializeField] private Toggle _removeSolveNumbersToggle;
    [SerializeField] private Toggle _soundToggle;

    [SerializeField] private GameObject[] _initialTurnOffMenus;


    private SceneController _sceneController;
    private ScoreController _scoreController;
    private SudokuStateController _stateController;

    private void Awake()
    {
        _sceneController = GetComponent<SceneController>();
        _stateController = new SudokuStateController();
    }

    private void Start()
    {
        _globalData.NotifyDifficulty += AssingDifficulty;
        _globalData.NotifyRemoveSolveNumbers += AssingRemoveSolveNumbersState;
        _globalData.NotifySoundState += AssingSoundState;

        if (GameObject.Find("ContinueButton").TryGetComponent(out Button continueButton))
            continueButton.onClick.AddListener(ContinueSudoku);

        continueButton.gameObject.SetActive(_stateController.CheckStateExistance());

        if (GameObject.Find("StartButton").TryGetComponent(out Button startButton))
            startButton.onClick.AddListener(StartSudoku);

#if UNITY_WEBGL

        GameObject.Find("ExitButton")?.SetActive(false);

#else  
        
        if (GameObject.Find("ExitButton").TryGetComponent(out Button exitButton))
            exitButton.onClick.AddListener(ExitGame);

#endif

        InitializeScores();

        _globalData.LoadOptions();

        _difficultySlider.onValueChanged.AddListener(SetDifficulty);
        _removeSolveNumbersToggle.onValueChanged.AddListener(SetRemoveSolveNumbersState);
        _soundToggle.onValueChanged.AddListener(SetSoundState);

        foreach (GameObject menu in _initialTurnOffMenus)
        {
            menu.SetActive(false);
        }

        SetFader();
    }

    private void InitializeScores()
    {
        _scoreController = new ScoreController();
        _scoreController.LoadScores();
        
        _scoreText.text = string.Empty;

        if (_scoreController.ScoresContainer is null || _scoreController.ScoresContainer.Scores.Count == 0) return;

        for (int i = 0; i < _scoreController.ScoresContainer.Scores.Count; i++)
        {
            if (i == _scoreController.ScoresContainer.Scores.Count - 1)
            {
                _scoreText.text += (i + 1).ToString() + ". " + _scoreController.ScoresContainer.Scores[i].Points.ToString() + " " + _scoreController.ScoresContainer.Scores[i].Date;
            }
            else 
            {
                _scoreText.text += (i + 1).ToString() + ". " + _scoreController.ScoresContainer.Scores[i].Points.ToString() + " " + _scoreController.ScoresContainer.Scores[i].Date + "\n";
            }
        }
    }

    private void SetFader() 
    {
        _fader.OnFadeEnd += CheckFadeState;
        _fader.SetFade(false, _globalData.FadeDuration);
    }

    public void CheckFadeState(bool isFadeIn, bool fadeEnd) 
    {
        if (fadeEnd && isFadeIn)
            LoadGameScene();
    }

    private void ContinueSudoku() 
    {
        GoGameScene();
    }

    private void StartSudoku()
    {
        _stateController.ResetState();
        GoGameScene();
    }
    
    private void GoGameScene() 
    {
        _fader.SetFade(true, _globalData.FadeDuration);
    }

    private void LoadGameScene() 
    {
        _sceneController.GoGame();
    }
    
    private void ExitGame() 
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }

    public void AssingDifficulty(int difficulty) 
    { 
        _difficultySlider.value = difficulty;
        
        if (_difficultyText != null) 
            _difficultyText.text = GetDifficultyText(difficulty);
    }

    public void SetDifficulty(float difficulty)
    {
        _globalData.SetDifficulty((int)difficulty);
        
        if(_difficultyText != null)
            _difficultyText.text = GetDifficultyText((int)difficulty);
    }

    public void AssingRemoveSolveNumbersState(bool state) 
    { 
        _removeSolveNumbersToggle.isOn = state;
    }

    public void SetRemoveSolveNumbersState(bool state)
    {
        _globalData.SetRemoveSolveNumbersState(state);
    }

    public void AssingSoundState(bool state)
    {
        _soundToggle.isOn = state;
    }

    public void SetSoundState(bool state)
    {
        _globalData.SetSoundState(state);
    }

    private string GetDifficultyText(int difficulty) => difficulty switch
    {
        0 => "Easy",
        1 => "Normal",
        2 => "Hard",
        _ => string.Empty,
    };
}
