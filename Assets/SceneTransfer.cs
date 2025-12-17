using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransfer : MonoBehaviour
{
    public string sceneName; // השם של הסצנה הבאה (לכתוב באינספקטור)

    private void OnTriggerEnter(Collider other)
    {
        // בדיקה שהאובייקט שנכנס הוא השחקן
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}