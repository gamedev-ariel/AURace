using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;

    [Header("Settings")]
    public float timeRemaining = 300f; // 5 דקות = 300 שניות
    public bool timerIsRunning = false;
    public string gameOverSceneName = "GameOver"; // שם הסצנה שתקפוץ כשנגמר הזמן

    private void Awake()
    {
        // Singleton: מוודא שיש רק שעון אחד שנשאר בין סצנות
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            timerIsRunning = true; // מתחילים את הזמן מיד
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                DoGameOver();
            }
        }
    }

    void DoGameOver()
    {
        Debug.Log("Time is up! Game Over.");
        SceneManager.LoadScene(gameOverSceneName);
    }

    // פונקציה לעצירת הזמן (נקרא לזה כשננצח)
    public void StopTimer()
    {
        timerIsRunning = false;
    }
}