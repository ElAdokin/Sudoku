using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private SceneData _sceneData;

    private AsyncOperation _asyncLoad;

    private Optional<IEnumerator> _loadingCoroutine = new Optional<IEnumerator>();
    private CoroutineController _coroutineController;

    private Fader _fader;

    public void InitializeSceneController(SceneData data) 
    { 
        _sceneData = data;
        Subscribe();
    }

    private void Subscribe()
    {
        if (GameObject.Find("Fader").TryGetComponent(out Fader fader))
        {
            _fader = fader;
            _fader.OnFadeEnd += UpdateFade;
            _fader.SetFade(true, 1f);
        }
    }

    private void UpdateFade(bool isFadeIn, bool fadeEnd)
    {
        if (isFadeIn)
        {
            if (fadeEnd)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    LoadSceneAsync();
                }
            }
        }
        else
        {
            if (fadeEnd)
                if (SceneManager.GetActiveScene().buildIndex == 0)
                    ActivateScene();
                else
                    GoToLoadScreen();
        }
    }

    private void GoToLoadScreen() 
    {
        SceneManager.LoadScene(0);
    }

    private void LoadSceneAsync()
    {
        if (_loadingCoroutine.IfPresent()) StopLoadingScene();

        _loadingCoroutine = new Optional<IEnumerator>(LoadYourAsyncScene(_sceneData.Scene));
        _coroutineController = new CoroutineController(_loadingCoroutine, this);
        _coroutineController.StartCurrentCoroutine();
    }

    private IEnumerator LoadYourAsyncScene(Scenes scene)
    {
        Debug.Log("Loading Scene " + scene.ToString());

        _asyncLoad = SceneManager.LoadSceneAsync((int)scene + 1, LoadSceneMode.Single);
        _asyncLoad.allowSceneActivation = false;

        while (_asyncLoad.progress < 0.9f)
        {
            Debug.Log("Waiting...");
            yield return null;
        }

        Debug.Log("Scene Loaded");

        if (scene != Scenes.Game)
            _fader.SetFade(false, 1f);
        else
            ActivateScene();
        
        StopLoadingScene();
        yield break;
    }

    private void StopLoadingScene()
    {
        if (_loadingCoroutine.IfPresent())
        {
            _coroutineController.StopCurrentCoroutine();
            _loadingCoroutine = new Optional<IEnumerator>();
        }
    }

    private void ActivateScene()
    {
        _asyncLoad.allowSceneActivation = true;
    }

    public void NextScene(Scenes scene) 
    {
        _sceneData.AssingSceneToGo(scene);
        _fader.SetFade(false, 1f);
    }
}
