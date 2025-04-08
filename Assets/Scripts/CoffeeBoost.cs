using UnityEngine;

public class CoffeeBoost : MonoBehaviour
{
    public GameObject coffeeIcon;
    public GameObject player; 
    public float speedMultiplier = 2f;
    public float boostDuration = 5f; 
    public int maxCoffeeCount = 3; 

    private bool hasCoffee = false; // player acquired coffee cup
    private float originalSpeed; // original speed
    private PlayerMovement playerMovement; // playerMovement script access 
    private int currentCoffeeCount = 0; // number of coffees player acquired
    private bool isBoostActive = false; // Is the coffee boost active

    private void Start()
    {
        // Get playerMovement script
        playerMovement = player.GetComponent<PlayerMovement>();
        originalSpeed = playerMovement.moveSpeed;

        // Hide coffee icon at the start
        if (coffeeIcon != null)
        {
            coffeeIcon.SetActive(false);
        }

        // **%%%שחזור מצב כוס הקפה מהסצנה הקודמת%%%**: 
        if (PlayerSpawnManager.Instance.TryGetCoffeeState(out bool coffeeState, out float boostDuration))
        {
            hasCoffee = coffeeState;
            if (hasCoffee && coffeeIcon != null)
            {
                coffeeIcon.SetActive(true);
            }
            this.boostDuration = boostDuration;  // אם הבוסט פעיל, נשחזר את משך הזמן שלו
            //if (isBoostActive)
            //{
            //    UseCoffee();
            //}
        }
    }

    private void Update()
    {
        // If player has coffee and the boost is not active, press R to activate boost
        if (hasCoffee && !isBoostActive && Input.GetKeyDown(KeyCode.R))
        {
            UseCoffee();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If player enters the cafeteria zone & not reached the max coffee count & boost is not active = can acquire coffee
        if ((collision.gameObject.name == "BlockedZone12" || collision.gameObject.name == "BlockedZone13") && currentCoffeeCount < maxCoffeeCount && !isBoostActive)
        {
            AcquireCoffee();
        }
    }

    private void AcquireCoffee()
    {
        // acquire coffee cup if the boost is not active
        if (!isBoostActive && currentCoffeeCount < maxCoffeeCount)
        {
            hasCoffee = true;
            currentCoffeeCount++;

            if (coffeeIcon != null)
            {
                coffeeIcon.SetActive(true); // icon pop
            }

            // **%%%שמירת מצב כוס הקפה%%%**: נשמור את מצב כוס הקפה בסצנה
            PlayerSpawnManager.Instance.SetCoffeeState(true, boostDuration);
        }
    }

    private void UseCoffee()
    {
        // uses R coffee, activating the boost
        hasCoffee = false;
        if (coffeeIcon != null)
        {
            coffeeIcon.SetActive(false); // Hide icon
        }

        // speed multiplier
        playerMovement.SetSpeed(originalSpeed * speedMultiplier);

        // boost is active
        isBoostActive = true;
        //PlayerSpawnManager.Instance.SetCoffeeState(false, boostDuration, isBoostActive);

        // Reset player speed after the boost ends
        Invoke("EndBoost", boostDuration);

        // **%%%שמירת מצב כוס הקפה לאחר השימוש: הגדרת מצב הקפה%%%**
        PlayerSpawnManager.Instance.SetCoffeeState(false);  // הגדר כוס קפה כלא פעילה
    }

    private void EndBoost()
    {
        // Restore to original speed
        playerMovement.SetSpeed(originalSpeed);

        // After boost ends, allow to acquire coffee again
        isBoostActive = false;
    }
}
