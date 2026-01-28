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
