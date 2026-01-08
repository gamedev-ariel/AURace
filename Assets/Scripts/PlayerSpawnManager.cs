//using UnityEngine;

//public class PlayerSpawnManager : MonoBehaviour
//{
//    public static PlayerSpawnManager Instance { get; private set; }

//    private Vector3 spawnPosition;
//    private bool hasSpawnPosition = false;

//    public bool hasCoffee = false;
//    //private bool BoostActive = false;
//    private float coffeeBoostDuration = 0f;

//    private void Awake()
//    {
//        // Singleton pattern
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

//    public void SetSpawnPosition(Vector3 position)
//    {
//        spawnPosition = position;
//        hasSpawnPosition = true;
//    }

//    public bool TryGetSpawnPosition(out Vector3 position)
//    {
//        position = spawnPosition;
//        bool hadPosition = hasSpawnPosition;
//        hasSpawnPosition = false;
//        return hadPosition;
//    }

//    // **%%%שמירה על מצב כוס הקפה בין סצנות%%%**
//    public void SetCoffeeState(bool coffeeState, float boostDuration = 0f)
//    {
//        hasCoffee = coffeeState;
//        //BoostActive = isBoostActive;
//        coffeeBoostDuration = boostDuration;
//    }

//    public bool TryGetCoffeeState(out bool coffeeState, out float boostDuration)
//    {
//        coffeeState = hasCoffee;
//        boostDuration = coffeeBoostDuration;
//        //isBoostActive = BoostActive;
//        bool hadCoffeeState = hasCoffee;
//        //hasCoffee = false; // Reset after using the state
//        return coffeeState;
//    }
//}


using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance;

    // --- נתונים לשמירה ---
    public bool hasCoffee = false;
    public float savedBoostDuration = 5f;
    private Vector3? spawnPosition = null; // המיקום להשתגרות
    public bool hasWinningItem = false;    // האם ניצחנו?

    private void Awake()
    {
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

    // --- ניהול מיקום (התיקון כאן) ---

    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    // הפונקציה הישנה (משאירים ליתר ביטחון)
    public Vector3? GetSpawnPosition()
    {
        Vector3? tempPos = spawnPosition;
        spawnPosition = null;
        return tempPos;
    }

    // === הפונקציה החדשה שחסרה לך ===
    // זה מה ש-PlayerSpawner שלך מחפש עכשיו
    public bool TryGetSpawnPosition(out Vector3 position)
    {
        if (spawnPosition.HasValue)
        {
            position = spawnPosition.Value;
            spawnPosition = null; // מאפסים כדי שלא נחזור לשם שוב
            return true;
        }

        position = Vector3.zero;
        return false;
    }
    // ================================

    // --- ניהול קפה ---
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