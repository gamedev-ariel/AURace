using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [Header("Scene Names")]
    public string firstLevelScene = "515253_NET"; // השם של השלב הראשון שלך
    public string mainMenuScene = "MainMenu";     // השם של התפריט הראשי

    // פונקציה לכפתור "Try Again"
    public void RestartGame()
    {
        // 1. איפוס הזמן במנהל הראשי (חשוב!)
        if (PlayerSpawnManager.Instance != null)
        {
            PlayerSpawnManager.Instance.ResetTimer();
        }

        // 2. טעינת השלב הראשון מחדש
        SceneManager.LoadScene(firstLevelScene);
    }

    // פונקציה לכפתור "Main Menu"
    public void GoToMainMenu()
    {
        // כשחוזרים לתפריט, כדאי לאפס גם
        if (PlayerSpawnManager.Instance != null)
        {
            PlayerSpawnManager.Instance.ResetTimer();
            PlayerSpawnManager.Instance.StopTimer(); // עוצרים את הזמן בתפריט
        }

        SceneManager.LoadScene(mainMenuScene);
    }

    // פונקציה לכפתור יציאה
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}