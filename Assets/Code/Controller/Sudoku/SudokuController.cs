using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GridGenerator))]
public class SudokuController : MonoBehaviour
{
    private SudokuData _sudokuData;
    private GlobalData _globalData;

    private SavedSudokuController _savedSudokuController;
    private SudokuStateController _stateController;

    private GridGenerator _generator;

    private Optional<IEnumerator> _initialization = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;
    private Yielders _yielders = new Yielders();

    public SavedSudokuController SavedSudokuController => _savedSudokuController;
    public SudokuStateController StateController => _stateController;

    public SudokuData SudokuData => _sudokuData;
    public int Difficulty => _globalData.Difficulty;
    public bool RemoveSolveNumbers => _globalData.RemoveSolveNumbers;
    public bool Sound => _globalData.Sound;

    public void Initialize(GlobalData globalData) 
    {
        if (_initialization.IfPresent()) return;

        _globalData = globalData;
        _sudokuData = _globalData.SudokuData;
        
        _savedSudokuController = new SavedSudokuController();
        _stateController = new SudokuStateController();
        
        _initialization = new Optional<IEnumerator>(Initialization());
        _coroutineController = new CoroutineController(_initialization, this);
        _coroutineController.StartCurrentCoroutine();
    }

    private IEnumerator Initialization() 
    {
        _generator = gameObject.GetComponent<GridGenerator>();

        yield return _generator.InitializeGridGenerator(this);

        StopInitialization();
        
        yield break;
    }

    private void StopInitialization() 
    {
        if (_initialization.IfPresent()) 
        {
            _coroutineController.StopCurrentCoroutine();
            _initialization = new Optional<IEnumerator>();
        }
    }
}
