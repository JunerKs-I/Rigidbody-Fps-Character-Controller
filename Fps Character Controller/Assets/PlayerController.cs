using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Looking variables

    [SerializeField] Transform cam;
    [SerializeField] Transform body;
    [SerializeField] float mouseSens = 0.2f;
    float multiplier = 10f;
    float mouseX, mouseY;
    float xRot, yRot;

    #endregion

    void Start()
    {

    }

    void Update()
    {
        GetMouseInput();
    }

    void FixedUpdate()
    {

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

    #endregion
}