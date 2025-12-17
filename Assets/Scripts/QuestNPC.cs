//using UnityEngine;
//using System.Collections;

//public class QuestNPC : MonoBehaviour
//{
//    [Header("Goreira (Drag & Drop)")]
//    public GameObject speechBubble; // גרור לפה את הבועה שיצרת (הילד של ה-NPC)

//    void Start()
//    {
//        // מסתיר את הבועה בתחילת המשחק כדי שלא יראו אותה סתם
//        if (speechBubble != null)
//        {
//            speechBubble.SetActive(false);
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        // אם מי שנגע בי הוא השחקן
//        if (collision.gameObject.CompareTag("Player"))
//        {
//            // תפעיל את הבועה
//            StartCoroutine(ShowBubbleRoutine());
//        }
//    }

//    IEnumerator ShowBubbleRoutine()
//    {
//        if (speechBubble != null)
//        {
//            speechBubble.SetActive(true); // תדליק
//            yield return new WaitForSeconds(3f); // חכה 3 שניות
//            speechBubble.SetActive(false); // תכבה
//        }
//    }
//}

using UnityEngine;
using System.Collections;

public class QuestNPC : MonoBehaviour
{
    [Header("Settings")]
    public GameObject speechBubble; // גרור לפה את הבועה
    public float distanceFromCafe = 2.5f; // המרחק מהקפיטריה (כדי שלא יעמוד עליה)

    void Start()
    {
        // 1. הסתרת הבועה בהתחלה
        if (speechBubble != null) speechBubble.SetActive(false);

        // 2. איתור הקפיטריות לפי השם
        GameObject cafe1 = GameObject.Find("BlockedZone12");
        GameObject cafe2 = GameObject.Find("BlockedZone13");

        GameObject chosenCafe = null;

        // 3. בחירה אקראית ביניהן
        if (cafe1 != null && cafe2 != null)
        {
            // אם שתיהן קיימות, תטיל מטבע (50/50)
            chosenCafe = (Random.value > 0.5f) ? cafe1 : cafe2;
        }
        else if (cafe1 != null)
        {
            chosenCafe = cafe1; // יש רק את 12
        }
        else if (cafe2 != null)
        {
            chosenCafe = cafe2; // יש רק את 13
        }

        // 4. הזזת ה-NPC ליד הקפיטריה שנבחרה
        if (chosenCafe != null)
        {
            // יצירת כיוון אקראי סביב הקפיטריה
            Vector2 randomDir = Random.insideUnitCircle.normalized;

            // המיקום החדש = מיקום הקפיטריה + (כיוון * מרחק)
            transform.position = (Vector2)chosenCafe.transform.position + (randomDir * distanceFromCafe);
        }
        else
        {
            Debug.LogError("Error: Could not find 'BlockedZone12' or 'BlockedZone13' in the scene!");
        }
    }

    // --- לוגיקה של הבועה (בנגיעה) ---
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ShowBubbleRoutine());
        }
    }

    IEnumerator ShowBubbleRoutine()
    {
        if (speechBubble != null)
        {
            speechBubble.SetActive(true);
            yield return new WaitForSeconds(3f);
            speechBubble.SetActive(false);
        }
    }
}