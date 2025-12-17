//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public CharacterController controller;
//    public float speed = 12f;
//    public float gravity = -9.81f;
//    public float jumpHeight = 3f;

//    public Transform cameraTransform;
//    public float mouseSensitivity = 100f;

//    Vector3 velocity;
//    bool isGrounded;
//    float xRotation = 0f;

//    void Start()
//    {
//        // ����� �� ��� ����� ����� ���� ����� ����
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Update()
//    {
//        // --- ����� ��� (����) ---
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ����� ����� ����� (�� ������ ����� ������)

//        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ����� ������ �����/����
//        transform.Rotate(Vector3.up * mouseX); // ����� ���� �����/�����

//        // --- ����� ��� (�����) ---
//        isGrounded = controller.isGrounded;

//        if (isGrounded && velocity.y < 0)
//        {
//            velocity.y = -2f; // ����� ������ ����� ������� �� �����
//        }

//        float x = Input.GetAxis("Horizontal");
//        float z = Input.GetAxis("Vertical");

//        Vector3 move = transform.right * x + transform.forward * z;
//        controller.Move(move * speed * Time.deltaTime);

//        // --- ����� ---
//        if (Input.GetButtonDown("Jump") && isGrounded)
//        {
//            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
//        }

//        // --- �������� ---
//        velocity.y += gravity * Time.deltaTime;
//        controller.Move(velocity * Time.deltaTime);
//    }
//}


using UnityEngine;

public class PlayerMovement3D : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 8f;            // ������ �����
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float pushPower = 2.0f;      // ��� ����� �� �����

    public Transform cameraTransform;   // ������ ���� �����/����
    public float mouseSensitivity = 100f;

    Vector3 velocity;
    bool isGrounded;
    float xRotation = 0f;

    void Start()
    {
        // ����� �� ��� ����� ����� ���� ����� ����
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. ������ ���� (Look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ����� ��� ���� ����� �� ������ (90 ����� �����/����)

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // ������ ��� �����/����
        transform.Rotate(Vector3.up * mouseX); // �� ���� �� ����� ������ �����/�����

        // 2. ������ ������ (Move)
        isGrounded = controller.isGrounded; // ����� ��� ������ �����

        // ����� ������ ����� �� ����� �� �����
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // ��� ����� ��� ��� ������ �� ����� ����� ������ "�����"
        }

        float x = Input.GetAxis("Horizontal"); // A, D
        float z = Input.GetAxis("Vertical");   // W, S

        // ����� ����� ����� ���� ������ ������ ����� ����
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 3. ����� (Jump)
        // ����� ������ ��������: v = sqrt(h * -2 * g)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 4. ���� ����� (Gravity)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // 5. ���������� �������� (Push)
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // ����� ��� �������� Rigidbody ����� �� Kinematic (����)
        if (body == null || body.isKinematic)
        {
            return;
        }

        // ����� ����� ���� ��� (��� �� ����� ����� �� �����)
        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        // ����� ����� ������ ������ ���
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushPower;
    }
}