//using UnityEngine;
//using UnityEngine.UI; // חובה בשביל טקסט רגיל

//public class TimerUI : MonoBehaviour
//{
//    public Text timeText; // גרור לכאן את הטקסט מה-Canvas

//    void Update()
//    {
//        // אם המנהל קיים, נעדכן את הטקסט
//        if (GameTimeManager.Instance != null)
//        {
//            float timeToDisplay = GameTimeManager.Instance.timeRemaining;

//            // חישוב דקות ושניות
//            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
//            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

//            // עדכון הטקסט בפורמט MM:SS
//            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

//            // אופציונלי: צבע אדום כשנשאר קצת זמן (פחות מ-30 שניות)
//            if (timeToDisplay < 30)
//            {
//                timeText.color = Color.red;
//            }
//        }
//    }
//}


using UnityEngine;
using TMPro; // <--- הוספנו את הספריה של TextMeshPro

public class TimerUI : MonoBehaviour
{
    // שינינו את סוג המשתנה מ-Text ל-TMP_Text כדי שיתאים למה שיצרת
    public TMP_Text timeText;

    void Update()
    {
        if (GameTimeManager.Instance != null)
        {
            float timeToDisplay = GameTimeManager.Instance.timeRemaining;

            // חישוב דקות ושניות
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);

            // עדכון הטקסט
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // צבע אדום כשנשאר קצת זמן
            if (timeToDisplay < 30)
            {
                timeText.color = Color.red;
            }
            else
            {
                // חשוב להחזיר ללבן אם הזמן התאפס או התחיל מחדש
                timeText.color = Color.white;
            }
        }
    }
}