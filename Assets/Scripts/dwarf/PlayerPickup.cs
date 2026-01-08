//using UnityEngine;

//public class PlayerPickup : MonoBehaviour
//{
//    public Transform holdPosition;
//    private GameObject heldObject;
//    private Rigidbody2D heldRb;

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            if (heldObject == null)
//            {
//                TryPickup();
//            }
//            else
//            {
//                DropObject();
//            }
//        }
//    }

//    void TryPickup()
//    {
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
//        foreach (Collider2D collider in colliders)
//        {
//            string objectID = collider.gameObject.name;

//            // אם זה Machine - שמור אותו כ"אסוף" ומחק מהסצנה
//            if (collider.CompareTag("Machine"))
//            {
//                PlayerSpawnManager2.Instance.CollectMachine(objectID);
//                Destroy(collider.gameObject);
//                return;
//            }

//            // אם זה Pickup - אסוף והזז
//            if (collider.CompareTag("Pickup"))
//            {
//                heldObject = collider.gameObject;
//                heldRb = heldObject.GetComponent<Rigidbody2D>();

//                if (heldRb != null)
//                {
//                    heldRb.isKinematic = true;
//                    heldObject.transform.position = holdPosition.position;
//                    heldObject.transform.SetParent(transform);
//                }
//                break;
//            }
//        }
//    }

//    void DropObject()
//    {
//        if (heldObject != null)
//        {
//            heldRb.isKinematic = false;
//            heldObject.transform.SetParent(null);

//            // שמירת מיקום חדש
//            PlayerSpawnManager2.Instance.SavePickupPosition(heldObject.name, heldObject.transform.position);

//            heldObject = null;
//        }
//    }
//}



using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform holdPosition;
    private GameObject heldObject;
    private Rigidbody2D heldRb;

    // השם של החפץ המנצח (וודא שזה תואם לשם בהיררכיה)
    private string winningItemName = "FinalProject";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null) TryPickup();
            else DropObject();
        }
    }

    void TryPickup()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D collider in colliders)
        {
            string objectID = collider.gameObject.name;

            // --- טיפול ב-Machine (חפצים שנעלמים ונשמרים) ---
            if (collider.CompareTag("Machine"))
            {
                // 1. עדכון המנהל הפנימי של הבית (כרגיל)
                if (PlayerSpawnManager2.Instance != null)
                    PlayerSpawnManager2.Instance.CollectMachine(objectID);

                // 2. עדכון המנהל הראשי של המשחק (החלק החדש!)
                // אם זה החפץ המנצח - תעדכן את כולם עכשיו!
                if (objectID == winningItemName && PlayerSpawnManager.Instance != null)
                {
                    PlayerSpawnManager.Instance.SetWinningItemState(true);
                    Debug.Log("Item Saved Globally: " + objectID);
                }

                Destroy(collider.gameObject);
                return;
            }

            // --- טיפול ב-Pickup (חפצים שמחזיקים ביד) ---
            if (collider.CompareTag("Pickup"))
            {
                heldObject = collider.gameObject;
                heldRb = heldObject.GetComponent<Rigidbody2D>();

                if (heldRb != null)
                {
                    heldRb.isKinematic = true;
                    heldObject.transform.position = holdPosition.position;
                    heldObject.transform.SetParent(transform);
                }

                // בדיקה גם כאן - למקרה שהחפץ המנצח הוא מסוג Pickup
                if (objectID == winningItemName && PlayerSpawnManager.Instance != null)
                {
                    PlayerSpawnManager.Instance.SetWinningItemState(true);
                }
                break;
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldRb.isKinematic = false;
            heldObject.transform.SetParent(null);

            if (PlayerSpawnManager2.Instance != null)
                PlayerSpawnManager2.Instance.SavePickupPosition(heldObject.name, heldObject.transform.position);

            heldObject = null;
        }
    }

    // הפונקציה הזו נשארת למקרה שתצטרך אותה, אבל הפורטל כבר פחות תלוי בה
    public GameObject GetHeldObject() { return heldObject; }
}