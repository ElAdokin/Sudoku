using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SceneController))]
public class LoadScreenController : MonoBehaviour
{
    [SerializeField] private GlobalData _globalData;

    private SceneController _sceneController;

    private void Awake()
    {
        SetGameSettings();
        
        _sceneController = GetComponent<SceneController>();
        _sceneController.InitializeSceneController(_globalData.SceneData);
    }

    private void SetGameSettings() 
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }
}
