using System;
using UnityEngine;

public class Evader : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    private float hInput;
    private float vInput;    
    private float mouseX;
    private float mouseY;

    [SerializeField] private float mouseSens = 10;

    private Vector3 velocity;

    public Vector3 Velocity { get => velocity; set => velocity = value; }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetAxis();

        //Apply Mouse Rotation
        transform.Rotate(new Vector3(-mouseY, mouseX,   0), Space.Self);

        //Calculate movement Velocity
        Vector3 eulerAngle = transform.eulerAngles * (Mathf.PI / 180);

        Vector3 forwardDirection = MathLib.EulerAnglestoDirection(eulerAngle);
        forwardDirection.y = -forwardDirection.y;

        //Debug.Log(forwardDirection);
        Debug.DrawRay(transform.position, forwardDirection, Color.red, 0.1f);

        velocity = forwardDirection * vInput + MathLib.VectorCrossProduct(Vector3.up , forwardDirection) * hInput;
        transform.position += velocity * moveSpeed * Time.deltaTime;
    }

    private void GetAxis()
    {
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
    }
}
