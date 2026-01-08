//using UnityEngine;
//using Fusion;

//public class CoffeeBoostFusion : NetworkBehaviour
//{
//    [Header("UI & References")]
//    public GameObject coffeeIcon;
//    public GameObject playerModel; // ��������� - �� �� �� ���� ����

//    [Header("Settings")]
//    public float speedMultiplier = 2f;
//    public float boostDuration = 5f;
//    public int maxCoffeeCount = 3;

//    // ����� ���
//    [Networked] public NetworkBool HasCoffee { get; set; }
//    [Networked] public NetworkBool IsBoostActive { get; set; }
//    [Networked] private TickTimer BoostTimer { get; set; }

//    private float originalSpeed;
//    private PlayerMovement_Fusion playerMovement;
//    private int currentCoffeeCount = 0;

//    public override void Spawned()
//    {
//        playerMovement = GetComponent<PlayerMovement_Fusion>();

//        // ����� ������ ���� (����� ����� �������� ����� ����� �� ����)
//        originalSpeed = 5f;
//        if (playerMovement != null) originalSpeed = playerMovement.moveSpeed;

//        // --- ����� ������ ������� (���� �����) ---
//        if (Object.HasStateAuthority)
//        {
//            // ����� ����� ������ ������ �����
//            if (PlayerDataBackup.TryLoad(Object.InputAuthority, out var savedData))
//            {
//                // 1. ����� ��� ���� ������
//                HasCoffee = savedData.HasCoffee;
//                IsBoostActive = savedData.IsBoostActive;

//                // 2. ����� ������ (������ �� ��� �')
//                if (savedData.CustomSpawnPosition.HasValue)
//                {
//                    Debug.Log($"Teleporting player to saved position: {savedData.CustomSpawnPosition.Value}");

//                    // ����� ����� ������ ����
//                    transform.position = savedData.CustomSpawnPosition.Value;

//                    // �� �� Rigidbody2D, ����� ���� ���� ��� ����� ��������� �� ����� ����� ������
//                    if (TryGetComponent<Rigidbody2D>(out var rb))
//                    {
//                        rb.position = savedData.CustomSpawnPosition.Value;
//                        rb.linearVelocity = Vector2.zero; // Unity 6 / 2023+ (���� rb.velocity)
//                    }
//                }

//                // 3. ����� ���� �� ����� �� ��� ��� ����
//                if (IsBoostActive)
//                {
//                    ActivateBoost();
//                }
//            }
//        }

//        UpdateVisuals();
//    }

//    // ���� ������� ���� (���� ������ ����� �� �����)
//    // ����: ����� ���� ��� ������, ������ ���� �� ����� ���� ��������� ��� ���
//    public override void Despawned(NetworkRunner runner, bool hasState)
//    {
//        if (Object.HasStateAuthority)
//        {
//            // ����� ����� ����� (��� ����� ������, ��� �� ���� ����� �� ������ ������)
//            PlayerDataBackup.Save(Object.InputAuthority, HasCoffee, IsBoostActive);
//        }
//    }

//    public override void FixedUpdateNetwork()
//    {
//        // ���� ��� (Input)
//        if (GetInput(out NetworkInputData data))
//        {
//            // ����� ���� (R)
//            if (data.buttons.IsSet(MyButtons.Boost))
//            {
//                if (HasCoffee && !IsBoostActive)
//                {
//                    ActivateBoost();
//                }
//            }
//        }

//        // ����� ����� �����
//        if (IsBoostActive)
//        {
//            if (BoostTimer.Expired(Runner))
//            {
//                EndBoost();
//            }
//        }

//        UpdateVisuals();
//    }

//    private void UpdateVisuals()
//    {
//        if (coffeeIcon != null)
//        {
//            coffeeIcon.SetActive(HasCoffee && !IsBoostActive);
//        }
//    }

//    private void ActivateBoost()
//    {
//        HasCoffee = false;
//        IsBoostActive = true;

//        // ����� ������
//        BoostTimer = TickTimer.CreateFromSeconds(Runner, boostDuration);

//        // ����� ������
//        if (playerMovement != null)
//        {
//            playerMovement.moveSpeed = originalSpeed * speedMultiplier;
//        }
//    }

//    private void EndBoost()
//    {
//        IsBoostActive = false;

//        // ����� ������
//        if (playerMovement != null)
//        {
//            playerMovement.moveSpeed = originalSpeed;
//        }
//    }

//    private void OnTriggerEnter2D(Collider2D collision)
//    {
//        if (!Object.HasStateAuthority) return;

//        // ������ ����� ��� (��� �����)
//        if ((collision.gameObject.name.Contains("BlockedZone"))
//            && currentCoffeeCount < maxCoffeeCount
//            && !IsBoostActive)
//        {
//            HasCoffee = true;
//            currentCoffeeCount++;
//        }
//    }
//}


//---------------------------------------------------WORKING V -------------------------------------------------



using UnityEngine;
using Fusion;

public class CoffeeBoostFusion : NetworkBehaviour
{
    [Header("UI & References")]
    public GameObject coffeeIcon;
    public GameObject playerModel;

    // --- הוספנו את המשתנה הזה ---
    [Tooltip("גרור לכאן את הבועה עם הכיתוב Press R")]
    public GameObject pressRPrompt;
    // ----------------------------

    [Header("Settings")]
    public float speedMultiplier = 2f;
    public float boostDuration = 5f;
    public int maxCoffeeCount = 3;

    [Networked] public NetworkBool HasCoffee { get; set; }
    [Networked] public NetworkBool IsBoostActive { get; set; }
    [Networked] private TickTimer BoostTimer { get; set; }

    private float originalSpeed;
    private PlayerMovement_Fusion playerMovement;
    private int currentCoffeeCount = 0;

    public override void Spawned()
    {
        playerMovement = GetComponent<PlayerMovement_Fusion>();
        originalSpeed = 5f;
        if (playerMovement != null) originalSpeed = playerMovement.moveSpeed;

        if (Object.HasStateAuthority)
        {
            if (PlayerDataBackup.TryLoad(Object.InputAuthority, out var savedData))
            {
                HasCoffee = savedData.HasCoffee;
                IsBoostActive = savedData.IsBoostActive;

                if (savedData.CustomSpawnPosition.HasValue)
                {
                    transform.position = savedData.CustomSpawnPosition.Value;
                    if (TryGetComponent<Rigidbody2D>(out var rb))
                    {
                        rb.position = savedData.CustomSpawnPosition.Value;
                        rb.linearVelocity = Vector2.zero;
                    }
                }

                if (IsBoostActive) ActivateBoost();
            }
        }

        UpdateVisuals();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        if (Object.HasStateAuthority)
        {
            PlayerDataBackup.Save(Object.InputAuthority, HasCoffee, IsBoostActive);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.buttons.IsSet(MyButtons.Boost))
            {
                if (HasCoffee && !IsBoostActive) ActivateBoost();
            }
        }

        if (IsBoostActive && BoostTimer.Expired(Runner))
        {
            EndBoost();
        }

        UpdateVisuals();
    }

    // --- זו הפונקציה ששולטת על התצוגה ---
    private void UpdateVisuals()
    {
        // התנאי להצגה: יש קפה + הבוסט עדיין לא פעיל
        bool shouldShow = HasCoffee && !IsBoostActive;

        // עדכון אייקון הקפה
        if (coffeeIcon != null)
        {
            coffeeIcon.SetActive(shouldShow);
        }

        // עדכון הבועה החדשה (Press R)
        if (pressRPrompt != null)
        {
            pressRPrompt.SetActive(shouldShow);
        }
    }

    private void ActivateBoost()
    {
        HasCoffee = false;
        IsBoostActive = true;
        BoostTimer = TickTimer.CreateFromSeconds(Runner, boostDuration);
        if (playerMovement != null) playerMovement.moveSpeed = originalSpeed * speedMultiplier;
    }

    private void EndBoost()
    {
        IsBoostActive = false;
        if (playerMovement != null) playerMovement.moveSpeed = originalSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Object.HasStateAuthority) return;

        if ((collision.gameObject.name.Contains("BlockedZone"))
            && currentCoffeeCount < maxCoffeeCount
            && !IsBoostActive)
        {
            HasCoffee = true;
            currentCoffeeCount++;
        }
    }
}