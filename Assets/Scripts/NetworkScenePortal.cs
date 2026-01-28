using UnityEngine;
using Fusion;

public class NetworkScenePortal : NetworkBehaviour
{
    [Header("Transition Settings")]
    public string targetSceneName;

    [Tooltip("הקואורדינטה החדשה (X או Y) בסצנה הבאה")]
    public float targetSpawnCoordinate;

    [Tooltip("האם המעבר הוא אופקי (משנה X) או אנכי (משנה Y)?")]
    public bool isHorizontalTransition = true;

    [Tooltip("תוספת קטנה למיקום כדי לא להיתקע בקיר")]
    public float spawnOffset = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. זיהוי השחקן שנגע בפורטל
        var playerScript = other.GetComponent<CoffeeBoostFusion>();
        if (playerScript == null) return;

        // 2. רק ה-Master Client מוסמך להעביר סצנה לכולם במצב Shared
        // (או: אם אתה רוצה שכל אחד יוכל להעביר את כולם, תמחק את השורה הבאה)
        if (!Runner.IsSharedModeMasterClient) return;

        // 3. חישוב המיקום החדש (לוגיקה מהסקריפט המקורי שלך)
        Vector3 playerPosition = other.transform.position;
        Vector3 newPosition;

        if (isHorizontalTransition)
        {
            // משנים את X, שומרים על Y ו-Z
            newPosition = new Vector3(targetSpawnCoordinate + spawnOffset, playerPosition.y, playerPosition.z);
        }
        else
        {
            // משנים את Y, שומרים על X ו-Z
            newPosition = new Vector3(playerPosition.x, targetSpawnCoordinate + spawnOffset, playerPosition.z);
        }

        // 4. שמירת הנתונים לגיבוי (כדי שיוחזרו ב-Spawned בסצנה הבאה)
        // שים לב: זה שומר את המידע עבור השחקן הספציפי שנגע בפורטל
        PlayerDataBackup.Save(
            playerScript.Object.InputAuthority,
            playerScript.HasCoffee,
            playerScript.IsBoostActive,
            newPosition
        );

        Debug.Log($"Master Client Loading Scene: {targetSceneName}");

        // 5. טעינת הסצנה לכולם (Fusion מסנכרן את זה אוטומטית)
        Runner.LoadScene(SceneRef.FromIndex(GetSceneIndex(targetSceneName)));
    }

    // פונקציית עזר למציאת האינדקס של הסצנה
    private int GetSceneIndex(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(sceneName)) return i;
        }
        Debug.LogError($"Scene '{sceneName}' not found in Build Settings!");
        return 0;
    }
}