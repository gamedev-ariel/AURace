using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;     // ה־Prefab של ה־NPC
    public int startAmount = 5;      // X - כמה NPCים להתחלה
    public float spawnInterval = 10; // Y - כל כמה שניות להוסיף עוד
    public int spawnPerWave = 1;     // כמה NPCים להוסיף בכל פעם

    private float timer;

    void Start()
    {
        // יצירה ראשונית של X NPCs
        for (int i = 0; i < startAmount; i++)
        {
            SpawnNPC();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            for (int i = 0; i < spawnPerWave; i++)
            {
                SpawnNPC();
            }
            timer = 0f; // איפוס הטיימר
        }
    }

    void SpawnNPC()
    {
        // מיקום רנדומלי במסך
        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(
            new Vector2(Random.value, Random.value)
        );

        Instantiate(npcPrefab, spawnPos, Quaternion.identity);
    }
}
