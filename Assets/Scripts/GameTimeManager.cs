using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance; // כדי ש-TimerUI יוכל למצוא אותו

    [Header("Settings")]
    public string gameOverSceneName = "GameOver";

    // משתנה מקומי לשימוש ה-UI
    public float timeRemaining;

    private void Awake()
    {
        // הפעם זה סינגלטון מקומי (רק לסצנה הזו)
        Instance = this;
    }

    private void Start()
    {
        // 1. טעינת הזמן מהמנהל הראשי בתחילת הסצנה
        if (PlayerSpawnManager.Instance != null)
        {
            timeRemaining = PlayerSpawnManager.Instance.currentMatchTime;

            // אם זו הפעם הראשונה והטיימר לא רץ, נפעיל אותו (למשל בתחילת משחק)
            if (!PlayerSpawnManager.Instance.isTimerRunning && timeRemaining == 300f)
            {
                PlayerSpawnManager.Instance.isTimerRunning = true;
            }
        }
        else
        {
            // גיבוי למקרה שאין מנהל (למשל טסטים)
            timeRemaining = 300f;
        }
    }

    private void Update()
    {
        // בודקים מול המנהל הראשי אם הטיימר אמור לרוץ
        if (PlayerSpawnManager.Instance != null && PlayerSpawnManager.Instance.isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                // הפחתת הזמן
                timeRemaining -= Time.deltaTime;

                // עדכון המנהל הראשי בזמן אמת (כדי שישמר למעבר סצנה הבא)
                PlayerSpawnManager.Instance.currentMatchTime = timeRemaining;
            }
            else
            {
                // נגמר הזמן
                timeRemaining = 0;
                PlayerSpawnManager.Instance.currentMatchTime = 0;
                PlayerSpawnManager.Instance.isTimerRunning = false;
                DoGameOver();
            }
        }
    }

    void DoGameOver()
    {
        Debug.Log("Time is up!");
        SceneManager.LoadScene(gameOverSceneName);
    }

    public void StopTimer()
    {
        if (PlayerSpawnManager.Instance != null)
            PlayerSpawnManager.Instance.StopTimer();
    }

    // פונקציה לאיפוס (עבור כפתור Try Again)
    public void ResetTimer()
    {
        if (PlayerSpawnManager.Instance != null)
            PlayerSpawnManager.Instance.ResetTimer();

        timeRemaining = 300f;
    }
}