//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class Portal : MonoBehaviour
//{
//    public string sceneToLoad; // שם הסצנה לטעינה

//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player")) // בודק אם השחקן נכנס לפורטל
//        {
//            SceneManager.LoadScene(sceneToLoad);
//        }
//    }
//}



using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Settings")]
    public string sceneToLoad;

    [Header("Winning Condition")]
    public bool requireItemToWin = false;

    // כאן אתה כותב את השם של האובייקט (למשל "FinalProject")
    public string winningObjectName = "FinalProject";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (requireItemToWin)
            {
                CheckForWinningItem(other.gameObject);
            }

            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void CheckForWinningItem(GameObject player)
    {
        bool foundWinningItem = false;

        // --- בדיקה 1: האם זה ביד (עבור חפצים מסוג Pickup)? ---
        PlayerPickup pickupScript = player.GetComponent<PlayerPickup>();
        if (pickupScript != null)
        {
            GameObject itemInHand = pickupScript.GetHeldObject();
            if (itemInHand != null && itemInHand.name == winningObjectName)
            {
                foundWinningItem = true;
            }
        }

        // --- בדיקה 2: האם זה נאסף כבר (עבור חפצים מסוג Machine)? ---
        // כאן אנחנו בודקים ברשימה של המנהל הפנימי
        if (!foundWinningItem && PlayerSpawnManager2.Instance != null)
        {
            if (PlayerSpawnManager2.Instance.IsMachineCollected(winningObjectName))
            {
                foundWinningItem = true;
                Debug.Log("Found winning item in collected machines list!");
            }
        }

        // --- סיכום: אם מצאנו באחת הדרכים ---
        if (foundWinningItem)
        {
            Debug.Log("VICTORY! Item secured: " + winningObjectName);

            // מעדכנים את המנהל הראשי (זה שנשאר לכל המשחק)
            if (PlayerSpawnManager.Instance != null)
            {
                PlayerSpawnManager.Instance.SetWinningItemState(true);
            }
        }
    }
}