using UnityEngine;

public class CoffeeBoost : MonoBehaviour
{
    [Header("UI & References")]
    public GameObject coffeeIcon;
    public GameObject player;

    [Tooltip("גרור לכאן את הבועה עם הכיתוב Press R")]
    public GameObject pressRPrompt; // --- המשתנה החדש לבועה ---

    [Header("Settings")]
    public float speedMultiplier = 2f;
    public float boostDuration = 5f;
    public int maxCoffeeCount = 3;

    private bool hasCoffee = false;
    private float originalSpeed;
    private PlayerMovement playerMovement;
    private int currentCoffeeCount = 0;
    private bool isBoostActive = false;

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
        originalSpeed = playerMovement.moveSpeed;

        // שחזור מצב מהסצנה הקודמת (PlayerSpawnManager)
        if (PlayerSpawnManager.Instance != null && PlayerSpawnManager.Instance.TryGetCoffeeState(out bool coffeeState, out float savedDuration))
        {
            hasCoffee = coffeeState;
            this.boostDuration = savedDuration;
        }

        // עדכון התצוגה בהתחלה
        UpdateVisuals();
    }

    private void Update()
    {
        if (hasCoffee && !isBoostActive && Input.GetKeyDown(KeyCode.R))
        {
            UseCoffee();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == "BlockedZone12" || collision.gameObject.name == "BlockedZone13")
            && currentCoffeeCount < maxCoffeeCount
            && !isBoostActive)
        {
            AcquireCoffee();
        }
    }

    private void AcquireCoffee()
    {
        if (!isBoostActive && currentCoffeeCount < maxCoffeeCount)
        {
            hasCoffee = true;
            currentCoffeeCount++;

            // עדכון הוויזואליה (אייקון + בועה)
            UpdateVisuals();

            // שמירה למנג'ר
            if (PlayerSpawnManager.Instance != null)
                PlayerSpawnManager.Instance.SetCoffeeState(true, boostDuration);
        }
    }

    private void UseCoffee()
    {
        hasCoffee = false;

        // עדכון הוויזואליה (הכל נעלם)
        UpdateVisuals();

        playerMovement.SetSpeed(originalSpeed * speedMultiplier);
        isBoostActive = true;

        Invoke("EndBoost", boostDuration);

        if (PlayerSpawnManager.Instance != null)
            PlayerSpawnManager.Instance.SetCoffeeState(false);
    }

    private void EndBoost()
    {
        playerMovement.SetSpeed(originalSpeed);
        isBoostActive = false;

        // אין צורך לעדכן ויזואליה כי hasCoffee עדיין false
    }

    // --- הפונקציה שמסדרת את התצוגה ---
    private void UpdateVisuals()
    {
        // התנאי: מציגים רק אם יש קפה ואין בוסט פעיל
        bool shouldShow = hasCoffee && !isBoostActive;

        if (coffeeIcon != null)
            coffeeIcon.SetActive(shouldShow);

        if (pressRPrompt != null)
            pressRPrompt.SetActive(shouldShow);
    }
}