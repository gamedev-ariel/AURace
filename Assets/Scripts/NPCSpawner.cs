using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab;     
    public int startAmount = 5;      
    public float spawnInterval = 10; 
    public int spawnPerWave = 1;     
    private float timer;

    void Start()
    {
    
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
            timer = 0f; 
        }
    }

    void SpawnNPC()
    {
        
        Vector2 spawnPos = Camera.main.ViewportToWorldPoint(
            new Vector2(Random.value, Random.value)
        );

        Instantiate(npcPrefab, spawnPos, Quaternion.identity);
    }
}
