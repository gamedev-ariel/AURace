using UnityEngine;
using UnityEngine;
using Fusion;

public class CoffeeBoostFusion : NetworkBehaviour
{
    [Header("UI & References")]
    public GameObject coffeeIcon; // גרור לכאן את האייקון שנמצא בתוך הפרה-פאב
    public GameObject playerModel; // המודל הוויזואלי (אופציונלי)

    [Header("Settings")]
    public float speedMultiplier = 2f;
    public float boostDuration = 5f;
    public int maxCoffeeCount = 3;

    // משתני רשת - מסתנכרנים אוטומטית לכולם
    [Networked] public NetworkBool HasCoffee { get; set; }
    [Networked] public NetworkBool IsBoostActive { get; set; }

    // טיימר רשת
    [Networked] private TickTimer BoostTimer { get; set; }

    private float originalSpeed;
    private PlayerMovement_Fusion playerMovement; // הסקריפט תנועה של המולטיפלייר שלך
    private int currentCoffeeCount = 0;

    public override void Spawned()
    {
        // משיכת רכיב התנועה (וודא שהשם תואם לסקריפט התנועה שלך)
        playerMovement = GetComponent<PlayerMovement_Fusion>();

        if (playerMovement != null)
        {
            // נניח שיש משתנה moveSpeed פומבי
            // originalSpeed = playerMovement.moveSpeed; 
            // במידה ואין גישה, תצטרך להגדיר ידנית או לחשוף אותו
            originalSpeed = 5f;
        }

        if (coffeeIcon != null) coffeeIcon.SetActive(false);
    }

    public override void FixedUpdateNetwork()
    {
        // 1. קבלת קלט מהשחקן
        if (GetInput(out NetworkInputData data))
        {
            // בדיקה אם לחצו R (Boost)
            if (data.buttons.IsSet(MyButtons.Boost))
            {
                if (HasCoffee && !IsBoostActive)
                {
                    ActivateBoost();
                }
            }
        }

        // 2. ניהול זמן הבוסט (רץ בשרת ובלקוחות)
        if (IsBoostActive)
        {
            if (BoostTimer.Expired(Runner))
            {
                EndBoost();
            }
        }

        // 3. עדכון ויזואלי (האייקון) לכולם
        if (coffeeIcon != null)
        {
            coffeeIcon.SetActive(HasCoffee && !IsBoostActive);
        }
    }

    private void ActivateBoost()
    {
        HasCoffee = false;
        IsBoostActive = true;

        // הפעלת טיימר רשת
        BoostTimer = TickTimer.CreateFromSeconds(Runner, boostDuration);

        // שינוי מהירות (לוגיקה תלויה בסקריפט התנועה שלך)
        if (playerMovement != null)
        {
            // דוגמה לשינוי מהירות
            playerMovement.moveSpeed = originalSpeed * speedMultiplier;
        }
    }

    private void EndBoost()
    {
        IsBoostActive = false;

        if (playerMovement != null)
        {
            playerMovement.moveSpeed = originalSpeed;
        }
    }

    // איסוף קפה (רק השרת מחליט)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Object.HasStateAuthority) return;

        // בדיקה מול אזורי הקפה (תואם לקוד המקורי שלך)
        if ((collision.gameObject.name.Contains("BlockedZone"))
            && currentCoffeeCount < maxCoffeeCount
            && !IsBoostActive)
        {
            HasCoffee = true;
            currentCoffeeCount++;
        }
    }
}