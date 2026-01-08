using UnityEngine;

public class WinSceneChecker : MonoBehaviour
{
    [Header("UI References")]
    // גרור לכאן את אובייקט התמונה (VictoryImageScreen) מההיררכיה
    public GameObject victoryImage;

    void Start()
    {
        // וידוא שהתמונה מכובה בהתחלה (למקרה ששכחת לכבות ב-Inspector)
        if (victoryImage != null)
        {
            victoryImage.SetActive(false);
        }

        // בדיקה האם המנהל הראשי קיים והאם השגנו את החפץ
        if (PlayerSpawnManager.Instance != null)
        {
            if (PlayerSpawnManager.Instance.hasWinningItem)
            {
                DoWin();
            }
            else
            {
                Debug.Log("You are in the final scene, but you don't have the item yet.");
            }
        }
    }

    void DoWin()
    {
        Debug.Log("VICTORY! You have the item in scene 515253!");

        // 1. עצירת השעון (חשוב מאוד!)
        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.StopTimer();
        }

        // 2. הדלקת תמונת הניצחון
        if (victoryImage != null)
        {
            victoryImage.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Victory Image not assigned in inspector!");
        }
    }
}