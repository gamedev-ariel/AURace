using UnityEngine;
using Fusion; // 1. הוספנו את הספרייה של פיוז'ן

// 2. ירושה מ-NetworkBehaviour במקום MonoBehaviour
public class PlayerMovement_Fusion : NetworkBehaviour
{
    [Header("Animations")]
    public Sprite[] ACharDown;
    public Sprite[] ACharUp;
    public Sprite[] ACharLeft;
    public Sprite[] ACharRight;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float animationSpeed = 0.2f;

    private SpriteRenderer spriteRenderer;
    private Vector2 moveInput;
    private Sprite[] currentDirectionSprites;
    private int animationIndex = 0;
    private float animationTimer = 0f;
    private Rigidbody2D rb;

    // הפונקציה Spawned נקראת כשנוצר אובייקט ברשת (מחליף את Start בחלק מהמקרים)
    public override void Spawned()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        currentDirectionSprites = ACharDown;
    }

    // משתמשים ב-FixedUpdateNetwork במקום Update רגיל לתנועה ברשת
    public override void FixedUpdateNetwork()
    {
        // 3. בדיקת שליטה: האם הדמות הזו שייכת למחשב שלי?
        // אם לא - אנחנו יוצאים מהפונקציה ולא נותנים למקלדת להשפיע
        if (!HasStateAuthority) return;

        // --- מכאן הכל נשאר כמעט אותו דבר ---

        // קריאת מקלדת
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // תנועה פיזיקלית
        rb.linearVelocity = moveInput.normalized * moveSpeed;

        // --- לוגיקת אנימציה (מתוך הקוד שלך) ---
        if (moveInput.x > 0) currentDirectionSprites = ACharRight;
        else if (moveInput.x < 0) currentDirectionSprites = ACharLeft;
        else if (moveInput.y > 0) currentDirectionSprites = ACharUp;
        else if (moveInput.y < 0) currentDirectionSprites = ACharDown;

        if (moveInput != Vector2.zero)
        {
            AnimateMovement();
        }
        else
        {
            animationIndex = 0;
            spriteRenderer.sprite = currentDirectionSprites[0];
        }
    }

    private void AnimateMovement()
    {
        animationTimer += Runner.DeltaTime; // שימוש בשעון הרשת
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            animationIndex = (animationIndex + 1) % currentDirectionSprites.Length;
            spriteRenderer.sprite = currentDirectionSprites[animationIndex];
        }
    }

    public void SetSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }
}