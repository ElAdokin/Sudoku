using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void GoMainMenu()
    {
        Debug.Log("Go Main Menu");
        SceneManager.LoadScene("00_MainMenu");
    }

    public void GoGame()
    {
        Debug.Log("Go Game");
        SceneManager.LoadScene("01_Game");
    }
}
