using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("class3D");
    }

    public void JumpMenu()
    {
        SceneManager.LoadScene("JumpMenu");
    }

    public void hallway()
    {
        SceneManager.LoadScene("hallway");
    }

    public void ShowVideo()
    {
        SceneManager.LoadScene("VideoScene");
    }

    public void ShowInfo()
    {
        SceneManager.LoadScene("InfoScene");
    }
    public void NET()
    {
        SceneManager.LoadScene("515253_NET");
    }

    public void HOME()
    {
        SceneManager.LoadScene("515253");
    }

    public void class3D()
    {
        SceneManager.LoadScene("class3D");
    }
    public void Kitchen()
    {
        SceneManager.LoadScene("MemoryGame");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("main");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
