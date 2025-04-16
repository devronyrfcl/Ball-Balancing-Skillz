using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of ball movement
    public float jumpForce = 7f;  // Force applied for jumping
    public float maxTiltAngle = 2.5f; // Maximum tilt angle allowed for platform
    public LayerMask groundLayer; // LayerMask to check for ground

    public Joystick joystick; // Reference to the dynamic joystick
    private Rigidbody rb;
    private bool isGrounded;

    public Transform cameraTransform; // Reference to the camera
    public float cameraTiltSpeed = 5f; // Speed at which the camera will tilt

    public GameObject[] Background_Platform_Object; // Array of platform objects that will tilt

    public float platformTiltAmountX = 5f; // Tilt amount for X axis
    public float platformTiltAmountZ = 5f; // Tilt amount for Z axis
    public float platformTiltSpeed = 5f;   // Speed of platform tilt

    private void Start()
    {
        // Get the Rigidbody component attached to the ball
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the ball is grounded before jumping
        CheckIfGrounded();

        // Handle movement using joystick input
        MoveBall();

        // Adjust the camera tilt based on joystick input
        //AdjustCameraTilt();

        // Tilt the background platform objects based on joystick input
        TiltBackgroundPlatforms();
    }

    private void MoveBall()
    {
        // Joystick input for movement (x-axis and z-axis)
        Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);

        // Apply force to move the ball based on joystick direction
        rb.AddForce(direction * moveSpeed);
    }

    public void Jump()
    {
        // Only jump if the ball is grounded
        if (isGrounded)
        {
            // Apply upward force to the Rigidbody for jumping
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CheckIfGrounded()
    {
        // Cast a ray downwards from the ball to check if it's on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    /*private void AdjustCameraTilt()
    {
        // Get the joystick input
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Calculate the target camera tilt angles based on joystick input
        float targetXAngle = 45f; // Default angle for neutral position
        float targetZAngle = 0f;  // Default Z angle

        // Forward (verticalInput > 0) => X angle = 40, Backward (verticalInput < 0) => X angle = 50
        if (verticalInput > 0.1f)
        {
            targetXAngle = 40f; // Moving forward
        }
        else if (verticalInput < -0.1f)
        {
            targetXAngle = 50f; // Moving backward
        }

        // Left (horizontalInput < 0) => Z angle = -5, Right (horizontalInput > 0) => Z angle = +5
        if (horizontalInput < -0.1f)
        {
            targetZAngle = -0f; // Moving left
        }
        else if (horizontalInput > 0.1f)
        {
            targetZAngle = 0f; // Moving right
        }

        // Smoothly rotate the camera to the target angles
        Quaternion targetRotation = Quaternion.Euler(targetXAngle, cameraTransform.eulerAngles.y, targetZAngle);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetRotation, Time.deltaTime * cameraTiltSpeed);
    }*/

    private void TiltBackgroundPlatforms()
    {
        // Get the joystick input
        float horizontalInput = joystick.Horizontal;
        float verticalInput = joystick.Vertical;

        // Calculate the target tilt angles for the platforms based on joystick input
        float targetXAngle = 0f;
        float targetZAngle = 0f;

        // Apply tilt on the X axis (forward/backward)
        if (verticalInput > 0.1f) // Moving forward
        {
            targetXAngle = platformTiltAmountX; // Tilt forward
        }
        else if (verticalInput < -0.1f) // Moving backward
        {
            targetXAngle = -platformTiltAmountX; // Tilt backward
        }

        // Apply tilt on the Z axis (left/right)
        if (horizontalInput > 0.1f) // Moving right
        {
            targetZAngle = platformTiltAmountZ; // Tilt right
        }
        else if (horizontalInput < -0.1f) // Moving left
        {
            targetZAngle = -platformTiltAmountZ; // Tilt left
        }

        // Apply the tilt to each platform in the array
        foreach (GameObject platform in Background_Platform_Object)
        {
            // Smoothly rotate the platform to the target tilt
            Quaternion targetRotation = Quaternion.Euler(targetXAngle, platform.transform.eulerAngles.y, targetZAngle);
            platform.transform.rotation = Quaternion.Slerp(platform.transform.rotation, targetRotation, Time.deltaTime * platformTiltSpeed);
        }
    }
}
