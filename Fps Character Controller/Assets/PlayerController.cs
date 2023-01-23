using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Looking variables

    [Header("Looking")]
    [SerializeField] float mouseSens = 0.2f;
    [SerializeField] Transform cam;
    [SerializeField] Transform body;
    float multiplier = 10f;
    float mouseX, mouseY;
    float xRot, yRot;

    #endregion

    #region Movement variables

    [Header("Movement")]
    [SerializeField] float maxSpeed = 7f;
    [SerializeField] float runAccel = 25f;
    float accel;
    Vector3 moveDirection;
    Vector2 movement;
    Rigidbody rb;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 15f;
    [SerializeField] float distanceToGround = 0.4f;
    [SerializeField] Transform feetPos;
    [SerializeField] LayerMask walkableLayer;
    bool isGrounded;

    [Header("Keys")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    #endregion

    void Awake() => rb = GetComponent<Rigidbody>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        accel = runAccel;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(feetPos.position, distanceToGround, walkableLayer);

        GetMouseInput();
        GetKeyboardInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    #region Input

    void GetMouseInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * multiplier * Time.deltaTime;
        yRot += mouseX;
        mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * multiplier * Time.deltaTime;
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        cam.rotation = Quaternion.Euler(xRot, yRot, 0f);
        body.rotation = Quaternion.Euler(0f, yRot, 0f);
    }

    void GetKeyboardInput()
    {
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        moveDirection = cam.forward * movement.y + cam.right * movement.x;

        if (Input.GetKeyDown(jumpKey) && isGrounded) Jump();
    }

    #endregion

    #region Movement

    void Move()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        rb.AddForce(moveDirection.normalized * accel, ForceMode.Force);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    #endregion
}