using UnityEngine;

public class PlatformTiltControllerWithLimits : MonoBehaviour
{
    // Public reference to the Dynamic Joystick (assign in the Inspector)
    public DynamicJoystick dynamicJoystick;

    // Reference to the player or ball object
    public Transform player;

    // Rotation limits for tilting the platform
    public float maxTiltAngleX = 2.5f;  // Maximum tilt in the X-axis (forward-backward)
    public float maxTiltAngleZ = 2.5f;  // Maximum tilt in the Z-axis (left-right)
    public float tiltSpeed = 5f;        // Speed of tilting
    public float returnSpeed = 3f;      // Speed at which the platform returns to zero position

    private Quaternion initialRotation; // Store the initial flat rotation of the platform

    void Start()
    {
        // Save the original flat rotation of the platform
        initialRotation = transform.rotation;
    }

    void Update()
    {
        // Get joystick input (values between -1 and 1)
        float horizontalInput = dynamicJoystick.Horizontal;
        float verticalInput = dynamicJoystick.Vertical;

        // Calculate the player's position (dynamic pivot) around which the platform will rotate
        Vector3 pivotPoint = new Vector3(player.position.x, transform.position.y, player.position.z);

        // Only tilt when there's input from the joystick
        if (Mathf.Abs(horizontalInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f)
        {
            // Calculate the new tilt based on joystick input
            float tiltX = Mathf.Clamp(verticalInput * maxTiltAngleX, -maxTiltAngleX, maxTiltAngleX); // Tilt on X-axis (forward-backward)
            float tiltZ = Mathf.Clamp(horizontalInput * maxTiltAngleZ, -maxTiltAngleZ, maxTiltAngleZ); // Tilt on Z-axis (left-right)

            // Create the tilt rotation based on the joystick input
            Vector3 tiltDirection = new Vector3(tiltX, 0f, tiltZ);

            // Rotate the platform around the player's X and Z position
            transform.RotateAround(pivotPoint, Vector3.right, tiltDirection.x * tiltSpeed * Time.deltaTime); // Tilt around X-axis
            transform.RotateAround(pivotPoint, Vector3.forward, tiltDirection.z * tiltSpeed * Time.deltaTime); // Tilt around Z-axis

            // Clamp the rotation to the maximum allowed tilt angles
            ClampRotation();
        }
        else
        {
            // Smoothly return to the flat (initial) position when no input is detected
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * returnSpeed);
        }
    }

    // Clamps the platform's rotation to prevent exceeding max tilt angles
    private void ClampRotation()
    {
        // Get current rotation in Euler angles
        Vector3 currentRotation = transform.localRotation.eulerAngles;

        // Convert values from 360 to -180 range
        if (currentRotation.x > 180) currentRotation.x -= 360;
        if (currentRotation.z > 180) currentRotation.z -= 360;

        // Clamp the X and Z rotation angles within the specified tilt range
        currentRotation.x = Mathf.Clamp(currentRotation.x, -maxTiltAngleX, maxTiltAngleX);
        currentRotation.z = Mathf.Clamp(currentRotation.z, -maxTiltAngleZ, maxTiltAngleZ);

        // Apply the clamped rotation back to the platform
        transform.localRotation = Quaternion.Euler(currentRotation.x, 0f, currentRotation.z);
    }
}
