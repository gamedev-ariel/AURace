//using UnityEngine;

//public class NPCMovement : MonoBehaviour
//{
//    public float speed = 2f; // ������ ������
//    public Vector2 minBounds; // ���� ����� ����� �� �����
//    public Vector2 maxBounds; // ���� ����� ���� �� �����
//    public SpriteRenderer spriteRenderer; // ����� �-SpriteRenderer
//    public Sprite upSprite, downSprite, leftSprite, rightSprite; // ������ ����� ������

//    private Vector2 targetPosition;
//    private int stuckCounter = 0; // ���� ����� ������ �� �����

//    void Start()
//    {
//        if (spriteRenderer == null)
//        {
//            spriteRenderer = GetComponent<SpriteRenderer>();
//        }
//        SetNewTargetPosition();
//    }

//    void Update()
//    {
//        if (Vector2.Distance(transform.position, targetPosition) < 0.1f || stuckCounter > 5)
//        {
//            SetNewTargetPosition();
//        }
//        else
//        {
//            MoveToTarget();
//        }
//    }

//    void MoveToTarget()
//    {
//        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
//        transform.position = targetPosition; // ����� ������ ����
//        UpdateSprite(direction);
//    }

//    void SetNewTargetPosition()
//    {
//        float randomX = Random.Range(minBounds.x, maxBounds.x);
//        float randomY = Random.Range(minBounds.y, maxBounds.y);
//        targetPosition = new Vector2(randomX, randomY);
//        stuckCounter = 0;
//    }

//    void UpdateSprite(Vector2 direction)
//    {
//        if (spriteRenderer == null) return;

//        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
//        {
//            spriteRenderer.sprite = direction.x > 0 ? rightSprite : leftSprite;
//        }
//        else
//        {
//            spriteRenderer.sprite = direction.y > 0 ? upSprite : downSprite;
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        stuckCounter++;
//        SetNewTargetPosition(); // �� ����, ��� ���� ��� ���
//    }
//}

//using UnityEngine;

//public class NPCMovement : MonoBehaviour
//{
//    public float speed = 2f; // מהירות התנועה
//    public Vector2 minBounds; // גבול תחתון שמאלי של האזור
//    public Vector2 maxBounds; // גבול עליון ימני של האזור
//    public SpriteRenderer spriteRenderer; // רפרנס ל-SpriteRenderer
//    public Sprite upSprite, downSprite, leftSprite, rightSprite; // תמונות בהתאם לכיוון

//    private Vector2 targetPosition;
//    private Rigidbody2D rb;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRenderer = GetComponent<SpriteRenderer>();
//        SetNewTargetPosition();
//    }

//    void Update()
//    {
//        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
//        {
//            SetNewTargetPosition();
//        }
//        else
//        {
//            MoveToTarget();
//        }
//    }

//    void MoveToTarget()
//    {
//        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
//        rb.linearVelocity = direction * speed;
//        UpdateSprite(direction);
//    }

//    void SetNewTargetPosition()
//    {
//        Vector2 previousPosition = targetPosition;

//        do
//        {
//            float randomX = Random.Range(minBounds.x, maxBounds.x);
//            float randomY = Random.Range(minBounds.y, maxBounds.y);
//            targetPosition = new Vector2(randomX, randomY);
//        }
//        while (Vector2.Distance(previousPosition, targetPosition) < 1f); // מבטיח שהיעד החדש לא יהיה קרוב מדי לקודם
//    }

//    void UpdateSprite(Vector2 direction)
//    {
//        if (spriteRenderer == null) return;

//        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
//        {
//            spriteRenderer.sprite = direction.x > 0 ? rightSprite : leftSprite;
//        }
//        else
//        {
//            spriteRenderer.sprite = direction.y > 0 ? upSprite : downSprite;
//        }
//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {
//        SetNewTargetPosition(); // אם נתקע, מיד יבחר יעד חדש
//    }
//}






using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public float speed = 2f;
    private Vector2 direction;
    private Rigidbody2D rb;
    private float changeDirectionTime = 2f;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            PickRandomDirection();
            timer = 0f;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void PickRandomDirection()
    {
        // כיוון אקראי (נורמלי) במישור
        direction = Random.insideUnitCircle.normalized;
    }
}
