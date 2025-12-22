//using UnityEngine;
//using Fusion;

//public class NetworkScenePortal : NetworkBehaviour
//{
//    [Header("Transition Settings")]
//    public string targetSceneName;

//    [Tooltip("The x or y coordinate where the player should spawn in the new scene")]
//    public float targetSpawnCoordinate;

//    [Tooltip("Determines if the spawn coordinate is for X (horizontal) or Y (vertical) axis")]
//    public bool isHorizontalTransition = true;

//    [Tooltip("Offset to fine-tune the spawn position")]
//    public float spawnOffset = 0f;

//    // שיניתי ל-OnTriggerEnter2D כי זה יותר נכון למעברים, אבל הלוגיקה זהה
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        // רק השרת יכול לבצע מעבר סצנה
//        if (!Runner.IsServer) return;

//        // בדיקה אם מי שנכנס הוא השחקן שלנו
//        var playerScript = other.GetComponent<CoffeeBoostFusion>();

//        if (playerScript != null)
//        {
//            // 1. חישוב המיקום החדש (בדיוק כמו בסקריפט המקורי שלך)
//            Vector3 playerPosition = other.transform.position;
//            Vector3 newPosition;

//            if (isHorizontalTransition)
//            {
//                // שמירה על Y, שינוי X
//                newPosition = new Vector3(targetSpawnCoordinate + spawnOffset, playerPosition.y, playerPosition.z);
//            }
//            else
//            {
//                // שמירה על X, שינוי Y
//                newPosition = new Vector3(playerPosition.x, targetSpawnCoordinate + spawnOffset, playerPosition.z);
//            }

//            // 2. שמירת הנתונים בגיבוי (כולל המיקום החדש)
//            PlayerDataBackup.Save(
//                playerScript.Object.InputAuthority,
//                playerScript.HasCoffee,
//                playerScript.IsBoostActive,
//                newPosition // שולחים את המיקום שחישבנו
//            );

//            // 3. טעינת הסצנה החדשה דרך Fusion
//            Debug.Log($"Saving Pos: {newPosition} and Loading Scene: {targetSceneName}");
//            Runner.LoadScene(SceneRef.FromIndex(GetSceneIndex(targetSceneName)));
//        }
//    }

//    // פונקציית עזר למציאת האינדקס של הסצנה לפי השם
//    private int GetSceneIndex(string sceneName)
//    {
//        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
//        {
//            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
//            // בדיקה אם הנתיב מכיל את שם הסצנה
//            if (path.Contains(sceneName)) return i;
//        }
//        Debug.LogError($"Scene '{sceneName}' not found in Build Settings!");
//        return 0;
//    }
//}


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