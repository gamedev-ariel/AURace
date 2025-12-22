using UnityEngine;
using Fusion;

public class ScenePortal : NetworkBehaviour
{
    public string nextSceneName = "Level2"; // השם של הסצנה הבאה (חייב להיות ב-Build Settings)

    private void OnTriggerEnter2D(Collider2D other)
    {
        // רק השרת מוסמך להחליט על מעבר סצנה
        if (!Runner.IsServer) return;

        // בדיקה אם מי שנכנס הוא שחקן
        if (other.GetComponent<NetworkObject>() != null)
        {
            Debug.Log($"Loading Scene: {nextSceneName}");
            Runner.LoadScene(SceneRef.FromIndex(GetSceneIndex(nextSceneName)));
        }
    }

    // פונקציית עזר למציאת אינדקס לפי שם (כי Fusion מעדיף אינדקסים)
    private int GetSceneIndex(string sceneName)
    {
        for (int i = 0; i < UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i);
            if (path.Contains(sceneName)) return i;
        }
        return 0;
    }
}