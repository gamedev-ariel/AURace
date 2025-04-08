using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private void Start()
    {
        // Check if there's a stored spawn position
        if (PlayerSpawnManager.Instance.TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            transform.position = spawnPosition;
        }
        //if (PlayerSpawnManager.Instance.TryGetCoffeeState(out bool coffeeState, out float boostDuration))
        //{
        //    CoffeeBoost.
        //}
    }
}
