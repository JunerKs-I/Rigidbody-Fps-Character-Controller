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
    [SerializeField] float sprintAccel = 30f;
    float accel;
    bool sprinting;
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
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

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
        ControlAccel();
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
        if (Input.GetKeyDown(jumpKey) && isGrounded) Jump();
        sprinting = Input.GetKey(sprintKey);

        movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //Magic stuff
        var forward = new Vector3(-cam.right.z, 0f, cam.right.x);

        moveDirection = forward * movement.y + cam.right * movement.x;

    }

    #endregion

    #region Movement

    void Move()
    {
        rb.AddForce(moveDirection.normalized * accel, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    void ControlAccel() => accel = !sprinting ? runAccel : sprintAccel;

    #endregion
}