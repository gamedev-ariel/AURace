using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform cameraTransform;
    public float mouseSensitivity = 100f;

    Vector3 velocity;
    bool isGrounded;
    float xRotation = 0f;

    void Start()
    {
        // מעלים את סמן העכבר ונועל אותו למרכז המסך
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // --- תזוזת מבט (עכבר) ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // מניעת שבירת מפרקת (לא להסתכל אחורה מלמעלה)

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // סיבוב המצלמה למעלה/למטה
        transform.Rotate(Vector3.up * mouseX); // סיבוב הגוף ימינה/שמאלה

        // --- תזוזת גוף (מקלדת) ---
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // איפוס מהירות נפילה כשאנחנו על הרצפה
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // --- קפיצה ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- גרביטציה ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}