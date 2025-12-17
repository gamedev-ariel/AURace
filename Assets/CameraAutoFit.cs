using UnityEngine;

public class CameraAutoFit : MonoBehaviour
{
    [Header("Drag your Map object here")]
    public Renderer mapRenderer; // גרור לפה את האובייקט של המפה (Layer1)

    void Start()
    {
        if (mapRenderer == null)
        {
            Debug.LogError("לא גררת את המפה לסקריפט במצלמה!");
            return;
        }

        Camera cam = GetComponent<Camera>();

        // 1. קבלת הגבולות המדויקים של המפה מהמחשב
        Bounds bounds = mapRenderer.bounds;

        // 2. הזזת המצלמה למרכז המפה בדיוק
        transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);

        // 3. חישוב הזום (Size) הנדרש
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            // אם המסך רחב מספיק, מתאמים לפי גובה המפה
            cam.orthographicSize = bounds.size.y / 2f;
        }
        else
        {
            // אם המסך צר (למשל בטלפון), מתאמים לפי הרוחב
            float differenceInSize = targetRatio / screenRatio;
            cam.orthographicSize = (bounds.size.y / 2f) * differenceInSize;
        }
    }
}