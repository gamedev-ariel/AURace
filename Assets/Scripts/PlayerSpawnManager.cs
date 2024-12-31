using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{
    public static PlayerSpawnManager Instance { get; private set; }

    private Vector3 spawnPosition;
    private bool hasSpawnPosition = false;

    private bool hasCoffee = false;
    private float coffeeBoostDuration = 0f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetSpawnPosition(Vector3 position)
    {
        spawnPosition = position;
        hasSpawnPosition = true;
    }

    public bool TryGetSpawnPosition(out Vector3 position)
    {
        position = spawnPosition;
        bool hadPosition = hasSpawnPosition;
        hasSpawnPosition = false;
        return hadPosition;
    }

    // **%%%שמירה על מצב כוס הקפה בין סצנות%%%**
    public void SetCoffeeState(bool coffeeState, float boostDuration = 0f)
    {
        hasCoffee = coffeeState;
        coffeeBoostDuration = boostDuration;
    }

    public bool TryGetCoffeeState(out bool coffeeState, out float boostDuration)
    {
        coffeeState = hasCoffee;
        boostDuration = coffeeBoostDuration;
        bool hadCoffeeState = hasCoffee;
        //hasCoffee = false; // Reset after using the state
        return hadCoffeeState;
    }
}