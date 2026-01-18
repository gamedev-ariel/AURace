//using UnityEngine;

//public class PlayerSpawnManager : MonoBehaviour
//{
//    public static PlayerSpawnManager Instance;

//    // --- נתונים לשמירה ---
//    public bool hasCoffee = false;
//    public float savedBoostDuration = 5f;
//    private Vector3? spawnPosition = null; // המיקום להשתגרות
//    public bool hasWinningItem = false;    // האם ניצחנו?

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // --- ניהול מיקום (התיקון כאן) ---

//    public void SetSpawnPosition(Vector3 pos)
//    {
//        spawnPosition = pos;
//    }

//    // הפונקציה הישנה (משאירים ליתר ביטחון)
//    public Vector3? GetSpawnPosition()
//    {
//        Vector3? tempPos = spawnPosition;
//        spawnPosition = null;
//        return tempPos;
//    }

//    // === הפונקציה החדשה שחסרה לך ===
//    // זה מה ש-PlayerSpawner שלך מחפש עכשיו
//    public bool TryGetSpawnPosition(out Vector3 position)
//    {
//        if (spawnPosition.HasValue)
//        {
//            position = spawnPosition.Value;
//            spawnPosition = null; // מאפסים כדי שלא נחזור לשם שוב
//            return true;
//        }

//        position = Vector3.zero;
//        return false;
//    }
//    // ================================

//    // --- ניהול קפה ---
//    public void SetCoffeeState(bool hasCoffee, float duration = 5f)
//    {
//        this.hasCoffee = hasCoffee;
//        this.savedBoostDuration = duration;
//    }

//    public bool TryGetCoffeeState(out bool hasCoffeeOut, out float durationOut)
//    {
//        hasCoffeeOut = this.hasCoffee;
//        durationOut = this.savedBoostDuration;
//        return true;
//    }

//    // --- ניהול חפץ ניצחון ---
//    public void SetWinningItemState(bool state)
//    {
//        hasWinningItem = state;
//        Debug.Log("Game Status Update: Has Winning Item = " + state);
//    }
//}



//using UnityEngine;

//public class PlayerSpawnManager : MonoBehaviour
//{
//    public static PlayerSpawnManager Instance;

//    // --- נתונים קיימים ---
//    public bool hasCoffee = false;
//    public float savedBoostDuration = 5f;
//    private Vector3? spawnPosition = null;
//    public bool hasWinningItem = false;

//    // --- נתונים חדשים לזמן ---
//    public float currentMatchTime = 300f; // ברירת מחדל 5 דקות
//    public bool isTimerRunning = false;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    // --- ניהול זמן (חדש) ---
//    public void ResetTimer()
//    {
//        currentMatchTime = 300f; // איפוס ל-5 דקות
//        isTimerRunning = true;
//    }

//    public void StopTimer()
//    {
//        isTimerRunning = false;
//    }

//    // --- שאר הפונקציות הישנות שלך (מיקום, קפה, ניצחון) נשארות אותו דבר ---
//    public void SetSpawnPosition(Vector3 pos) { spawnPosition = pos; }

//    public bool TryGetSpawnPosition(out Vector3 position)
//    {
//        if (spawnPosition.HasValue) { position = spawnPosition.Value; spawnPosition = null; return true; }
//        position = Vector3.zero; return false;
//    }

//    public void SetCoffeeState(bool hasCoffee, float duration = 5f) { this.hasCoffee = hasCoffee; this.savedBoostDuration = duration; }

//    public bool TryGetCoffeeState(out bool hasCoffeeOut, out float durationOut) { hasCoffeeOut = this.hasCoffee; durationOut = this.savedBoostDuration; return true; }

//    public void SetWinningItemState(bool state) { hasWinningItem = state; }
//}

using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    // --- נתוני שחקן ובוסטים ---
    public bool hasCoffee = false;
    public float savedBoostDuration = 5f;

    // --- מיקום ---
    private Vector3? spawnPosition = null;

    // --- ניצחון ---
    public bool hasWinningItem = false;

    // --- זמן ---
    public float currentMatchTime = 300f; // 5 דקות ברירת מחדל
    public bool isTimerRunning = false;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // --- ניהול זמן (כולל התיקון להקפאה) ---
    public void ResetTimer()
    {
        // התיקון הקריטי: מחזירים את הזמן לזוז רגיל (מבטלים את ההקפאה)
        Time.timeScale = 1f;

        currentMatchTime = 300f; // איפוס ל-5 דקות (או כל זמן אחר שתבחר)
        hasWinningItem = false;  // איפוס הניצחון כדי שלא ננצח ישר שוב
        isTimerRunning = true;   // מתחילים את השעון

        Debug.Log("Game Reset: Time scale set to 1, Timer restarted.");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // --- ניהול מיקום (Spawn) ---
    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    public bool TryGetSpawnPosition(out Vector3 position)
    {
        if (spawnPosition.HasValue)
        {
            position = spawnPosition.Value;
            spawnPosition = null; // מאפסים כדי שלא נחזור לשם שוב ושוב בטעות
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    // --- ניהול קפה (Boost) ---
    public void SetCoffeeState(bool hasCoffee, float duration = 5f)
    {
        this.hasCoffee = hasCoffee;
        this.savedBoostDuration = duration;
    }

    public bool TryGetCoffeeState(out bool hasCoffeeOut, out float durationOut)
    {
        hasCoffeeOut = this.hasCoffee;
        durationOut = this.savedBoostDuration;
        return true;
    }

    // --- ניהול חפץ ניצחון ---
    public void SetWinningItemState(bool state)
    {
        hasWinningItem = state;
        Debug.Log("Game Status Update: Has Winning Item = " + state);
    }
}