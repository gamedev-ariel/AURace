//using UnityEngine;
//using System.Linq; // חובה כדי למצוא את הקרוב ביותר בקלות

//public class SmartCrowdNPC : MonoBehaviour
//{
//    public enum State { Wander, BlockStation, Stunned }
//    public State currentState = State.Wander;

//    [Header("Setup (Drag Objects Here)")]
//    // --- כאן אתה גורר את האובייקטים שאתה רוצה שהם יחסמו ---
//    // אם זה ריק, הסקריפט יחפש אוטומטית אובייקטים עם התגית "Station"
//    public Transform[] guardPoints; 

//    [Header("Movement Settings")]
//    public float wanderSpeed = 2f;
//    public float blockSpeed = 5f;       // מהירות ריצה לחסימה
//    public float pushForce = 25f;       // עוצמת ההעפה
//    public float pushRadius = 3f;       // רדיוס ההדף
//    public float resetDistance = 5f;    // מרחק ביטחון (מתי הם נרגעים)

//    private Rigidbody2D rb;
//    private Transform player;
//    private Transform currentTarget; // התחנה הקרובה ביותר כרגע
//    private Vector2 wanderDirection;
//    private float timer;
//    private float stunTimer;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();

//        GameObject p = GameObject.FindGameObjectWithTag("Player");
//        if (p != null) player = p.transform;

//        // גיבוי: אם לא גררת אובייקטים ידנית, הוא יחפש לפי תגית
//        if (guardPoints == null || guardPoints.Length == 0)
//        {
//            GameObject[] stations = GameObject.FindGameObjectsWithTag("Station");
//            guardPoints = new Transform[stations.Length];
//            for(int i=0; i<stations.Length; i++)
//            {
//                guardPoints[i] = stations[i].transform;
//            }
//        }

//        PickRandomDirection();
//    }

//    void Update()
//    {
//        if (player == null) return;

//        // מציאת התחנה הכי קרובה לשחקן (כדי לדעת את מי לחסום)
//        currentTarget = GetClosestStation(player.position);
//        if (currentTarget == null) return;

//        bool hasCoffee = (PlayerSpawnManager.Instance != null && PlayerSpawnManager.Instance.hasCoffee);

//        // --- מנגנון הדיפה (Space) ---
//        if (hasCoffee && Input.GetKeyDown(KeyCode.Space))
//        {
//            float distToPlayer = Vector2.Distance(transform.position, player.position);
//            if (distToPlayer < pushRadius)
//            {
//                ApplyKnockback();
//            }
//        }

//        // --- מכונת מצבים ---

//        // 1. אם אני מעולף -> חכה
//        if (currentState == State.Stunned)
//        {
//            stunTimer -= Time.deltaTime;
//            if (stunTimer <= 0)
//            {
//                currentState = State.Wander;
//                rb.linearVelocity = Vector2.zero; // עצירה
//            }
//            return; 
//        }

//        // 2. בדיקה: האם לחסום או לשוטט?
//        float distPlayerToStation = Vector2.Distance(player.position, currentTarget.position);

//        // חוסמים רק אם: יש לך קפה + אתה עדיין קרוב לתחנה (לא ברחת)
//        if (hasCoffee && distPlayerToStation < resetDistance)
//        {
//            currentState = State.BlockStation;
//        }
//        else
//        {
//            currentState = State.Wander;
//        }

//        // טיימר לשינוי כיוון בשיטוט
//        if (currentState == State.Wander)
//        {
//            timer += Time.deltaTime;
//            if (timer >= 2f) { PickRandomDirection(); timer = 0f; }
//        }
//    }

//    void FixedUpdate()
//    {
//        if (currentState == State.Stunned) return; 

//        switch (currentState)
//        {
//            case State.Wander:
//                rb.MovePosition(rb.position + wanderDirection * wanderSpeed * Time.fixedDeltaTime);
//                break;

//            case State.BlockStation:
//                if (currentTarget != null)
//                {
//                    // רצים לחסום את התחנה הקרובה
//                    Vector2 dir = (currentTarget.position - transform.position).normalized;
//                    rb.MovePosition(rb.position + dir * blockSpeed * Time.fixedDeltaTime);
//                }
//                break;
//        }
//    }

//    void ApplyKnockback()
//    {
//        currentState = State.Stunned;
//        stunTimer = 1.0f; // זמן שיתוק בשניות

//        // כיוון ההעפה: מהשחקן החוצה
//        Vector2 pushDir = (transform.position - player.position).normalized;

//        rb.linearVelocity = Vector2.zero;
//        rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
//    }

//    Transform GetClosestStation(Vector2 position)
//    {
//        if (guardPoints == null || guardPoints.Length == 0) return null;

//        Transform closest = null;
//        float minDistance = Mathf.Infinity;

//        foreach (Transform station in guardPoints)
//        {
//            if (station == null) continue;
//            float dist = Vector2.Distance(position, station.position);
//            if (dist < minDistance)
//            {
//                minDistance = dist;
//                closest = station;
//            }
//        }
//        return closest;
//    }

//    void PickRandomDirection()
//    {
//        wanderDirection = Random.insideUnitCircle.normalized;
//    }
//}




using UnityEngine;
using Fusion; // שימוש ב-Fusion
using System.Linq;

public class SmartCrowdNPC : MonoBehaviour
{
    public enum State { Wander, BlockStation, Stunned }
    public State currentState = State.Wander;

    [Header("Setup")]
    public Transform[] guardPoints;

    [Header("Movement Settings")]
    public float wanderSpeed = 2f;
    public float blockSpeed = 5f;
    public float pushForce = 25f;
    public float pushRadius = 3f;
    public float resetDistance = 5f;

    private Rigidbody2D rb;
    private Transform currentTargetPlayer; // השחקן שאנחנו רודפים אחריו כרגע
    private Transform currentStationTarget; // התחנה לחסימה
    private Vector2 wanderDirection;
    private float timer;
    private float stunTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // מציאת תחנות אוטומטית
        if (guardPoints == null || guardPoints.Length == 0)
        {
            GameObject[] stations = GameObject.FindGameObjectsWithTag("Station");
            if (stations != null)
            {
                guardPoints = new Transform[stations.Length];
                for (int i = 0; i < stations.Length; i++) guardPoints[i] = stations[i].transform;
            }
        }
        PickRandomDirection();
    }

    void Update()
    {
        // 1. מציאת השחקן הרלוונטי ביותר (הכי קרוב שיש לו קפה)
        FindBestTarget();

        if (currentTargetPlayer == null)
        {
            currentState = State.Wander; // אם אין שחקנים רלוונטיים, שוטט
        }
        else
        {
            // מציאת התחנה הקרובה לשחקן הנבחר
            currentStationTarget = GetClosestStation(currentTargetPlayer.position);

            // בדיקת הדיפה: האם השחקן הזה מפעיל בוסט כרגע?
            CheckForRepel();
        }

        // 2. ניהול Stun
        if (currentState == State.Stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0)
            {
                currentState = State.Wander;
                rb.linearVelocity = Vector2.zero;
            }
            return;
        }

        // 3. לוגיקת מרדף/חסימה
        if (currentTargetPlayer != null && currentStationTarget != null)
        {
            float distPlayerToStation = Vector2.Distance(currentTargetPlayer.position, currentStationTarget.position);

            if (distPlayerToStation < resetDistance)
            {
                currentState = State.BlockStation;
            }
            else
            {
                currentState = State.Wander;
            }
        }

        // שיטוט אקראי
        if (currentState == State.Wander)
        {
            timer += Time.deltaTime;
            if (timer >= 2f) { PickRandomDirection(); timer = 0f; }
        }
    }

    void FixedUpdate()
    {
        if (currentState == State.Stunned) return;

        switch (currentState)
        {
            case State.Wander:
                rb.MovePosition(rb.position + wanderDirection * wanderSpeed * Time.fixedDeltaTime);
                break;

            case State.BlockStation:
                if (currentStationTarget != null)
                {
                    Vector2 dir = (currentStationTarget.position - transform.position).normalized;
                    rb.MovePosition(rb.position + dir * blockSpeed * Time.fixedDeltaTime);
                }
                break;
        }
    }

    // --- פונקציה חדשה למציאת שחקן במולטיפלייר ---
    void FindBestTarget()
    {
        currentTargetPlayer = null;
        float minDst = Mathf.Infinity;

        // מוצא את כל השחקנים שיש להם את הסקריפט החדש
        CoffeeBoostFusion[] allPlayers = FindObjectsByType<CoffeeBoostFusion>(FindObjectsSortMode.None);

        foreach (var p in allPlayers)
        {
            // רודפים רק אחרי מי שיש לו קפה
            if (p.HasCoffee)
            {
                float d = Vector2.Distance(transform.position, p.transform.position);
                if (d < minDst)
                {
                    minDst = d;
                    currentTargetPlayer = p.transform;
                }
            }
        }
    }

    void CheckForRepel()
    {
        if (currentTargetPlayer == null) return;

        // גישה לסקריפט של השחקן
        var boostScript = currentTargetPlayer.GetComponent<CoffeeBoostFusion>();

        // אם הבוסט פעיל והשחקן קרוב
        if (boostScript != null && boostScript.IsBoostActive)
        {
            float dist = Vector2.Distance(transform.position, currentTargetPlayer.position);
            if (dist < pushRadius)
            {
                ApplyKnockback();
            }
        }
    }

    void ApplyKnockback()
    {
        currentState = State.Stunned;
        stunTimer = 1.0f;
        Vector2 pushDir = (transform.position - currentTargetPlayer.position).normalized;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
    }

    Transform GetClosestStation(Vector2 position)
    {
        if (guardPoints == null || guardPoints.Length == 0) return null;
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform station in guardPoints)
        {
            if (station == null) continue;
            float dist = Vector2.Distance(position, station.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = station;
            }
        }
        return closest;
    }

    void PickRandomDirection()
    {
        wanderDirection = Random.insideUnitCircle.normalized;
    }
}