using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void PlayGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartTheGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
