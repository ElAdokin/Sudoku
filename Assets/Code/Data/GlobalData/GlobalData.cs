using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "ScriptableObjects/Data/GlobalData", order = 0)]
public class GlobalData : ScriptableObject, IOptions
{
    [SerializeField] private SudokuData _sudokuData;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private int _basePoints;
    [SerializeField] private int _checksMultiplier;
    [SerializeField] private int _errorsMultiplier;
    [SerializeField] private float _durationMultiplier;

    [SerializeField] private OptionsState _options;
    private readonly string _optionsSaveFolder = "OptionsSave";

    public event Action<int> NotifyDifficulty;
    public event Action<bool> NotifyRemoveSolveNumbers;
    public event Action<bool> NotifySoundState;

    public SudokuData SudokuData => _sudokuData;
    public float FadeDuration => _fadeDuration;
    public int BasePoints => _basePoints; 
    public int ChecksMultiplier => _checksMultiplier; 
    public int ErrorsMultiplier => _errorsMultiplier;
    public float DurationMultiplier => _durationMultiplier;

    public int Difficulty => _options.Difficulty;
    public bool RemoveSolveNumbers => _options.RemoveSolveNumbers;
    public bool Sound => _options.SoundActive;

    private void SaveOptions() 
    {
        SaveLoad<OptionsState>.Save(_options, _optionsSaveFolder, "OptionState");
    }

    public void LoadOptions()
    {
        _options = SaveLoad<OptionsState>.Load(_optionsSaveFolder, "OptionState");

        if (_options == null)
        {
            _options = new OptionsState();
            _options.Difficulty = 1;
            _options.RemoveSolveNumbers = false;
            _options.SoundActive = true;

            SaveOptions();
        }

        NotifyOptions();
    }

    private void NotifyOptions() 
    {
        NotifyDifficulty?.Invoke(_options.Difficulty);
        NotifyRemoveSolveNumbers?.Invoke(_options.RemoveSolveNumbers);
        NotifySoundState?.Invoke(_options.SoundActive);
    }

    public void SetDifficulty(int difficulty) 
    { 
        _options.Difficulty = difficulty;
        SaveOptions();
    }

    public void SetRemoveSolveNumbersState(bool state) 
    { 
        _options.RemoveSolveNumbers = state;
        SaveOptions();
    }

    public void SetSoundState(bool state)
    {
        _options.SoundActive = state;
        SaveOptions();
    }
}
