using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GridGenerator))]
public class SudokuController : MonoBehaviour
{
    private SudokuData _sudokuData;
    
    private GridGenerator _generator;

    private Optional<IEnumerator> _initialization = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;
    private Yielders _yielders = new Yielders();
    
    public void Initialize(SudokuData sudokuData) 
    {
        if (_initialization.IfPresent()) return;

        _sudokuData = sudokuData;

        _initialization = new Optional<IEnumerator>(Initialization());
        _coroutineController = new CoroutineController(_initialization, this);
        _coroutineController.StartCurrentCoroutine();
    }

    private IEnumerator Initialization() 
    {
        _generator = gameObject.GetComponent<GridGenerator>();

        yield return _generator.InitializeGridGenerator(_sudokuData);

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
