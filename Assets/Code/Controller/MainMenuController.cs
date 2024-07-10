using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneController))]
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GlobalData _globalData;

    private SceneController _sceneController;

    private void Awake()
    {
        SetGameSettings();
        
        _sceneController = GetComponent<SceneController>();
        _sceneController.InitializeSceneController(_globalData.SceneData);

        if (GameObject.Find("StartButton").TryGetComponent(out Button startButton))
            startButton.onClick.AddListener(GoGameScene);

        if (GameObject.Find("ExitButton").TryGetComponent(out Button exitButton))
            exitButton.onClick.AddListener(ExitGame);
    }

    private void GoGameScene() 
    {
        _sceneController.NextScene(Scenes.Game);
    }

    private void SetGameSettings()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    private void ExitGame() 
    {
        Debug.Log("ExitGame");
        Application.Quit();
    }
}
