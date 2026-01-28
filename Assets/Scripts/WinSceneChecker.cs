using UnityEngine;

public class WinSceneChecker : MonoBehaviour
{
    [Header("UI References")]
    public GameObject victoryImage;

    void Start()
    {
        if (victoryImage != null)
            victoryImage.SetActive(false);

        // בדיקה מול המנהל הראשי
        if (PlayerSpawnManager.Instance != null)
        {
            if (PlayerSpawnManager.Instance.hasWinningItem)
            {
                DoWin();
            }
        }
    }

    void DoWin()
    {
        Debug.Log("VICTORY! Freezing game...");

        // 1. הדלקת תמונת הניצחון
        if (victoryImage != null)
        {
            victoryImage.SetActive(true);
        }

        // 2. עצירת הטיימר הלוגית (במנהל)
        if (PlayerSpawnManager.Instance != null)
        {
            PlayerSpawnManager.Instance.StopTimer();
        }

        // 3. הקפאת העולם כולו! (זה החלק החדש)
        // שום דבר לא יזוז יותר, והשחקן לא יוכל ללכת
        Time.timeScale = 0f;
    }
}