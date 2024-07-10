using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SudokuController), typeof(SceneController))]
public class GameInstaller : MonoBehaviour
{
    [SerializeField] private GlobalData _globalData;
    
    private SudokuController _controller;
    private SceneController _sceneController;

    private void Awake()
    {
        SetGameSettings();

        if (GameObject.Find("GoMainMenuButton").TryGetComponent(out Button mainMenuButton))
            mainMenuButton.onClick.AddListener(GoMainMenu);

        _controller = gameObject.GetComponent<SudokuController>();
        _controller.Initialize(_globalData.SudokuData);
    }

    private void SetGameSettings()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    public void GoMainMenu()
    {
        _sceneController.NextScene(Scenes.MainMenu);
    }

    public void InitializeSceneController() 
    {
        GameObject.Find("LoadingObject").SetActive(false);
        _sceneController = GetComponent<SceneController>();
        _sceneController.InitializeSceneController(_globalData.SceneData);
    }
}
