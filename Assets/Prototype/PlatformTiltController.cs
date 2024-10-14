using UnityEngine;

public class PlatformTiltController : MonoBehaviour
{
    // Public reference to the Dynamic Joystick (assign in the Inspector)
    public DynamicJoystick dynamicJoystick;

    // Rotation limits for tilting the platform
    public float maxTiltAngleX = 2.5f;  // Maximum tilt in the X-axis (forward-backward)
    public float maxTiltAngleZ = 2.5f;  // Maximum tilt in the Z-axis (left-right)
    public float tiltSpeed = 5f;        // Speed of tilting

    // Update is called once per frame
    void Update()
    {
        // Get joystick input (values between -1 and 1)
        float horizontalInput = dynamicJoystick.Horizontal;
        float verticalInput = dynamicJoystick.Vertical;

        // Calculate the new rotation angles based on joystick input
        float tiltX = Mathf.Clamp(verticalInput * maxTiltAngleX, -maxTiltAngleX, maxTiltAngleX); // Tilt on X-axis (forward-backward)
        float tiltZ = Mathf.Clamp(horizontalInput * maxTiltAngleZ, -maxTiltAngleZ, maxTiltAngleZ); // Tilt on Z-axis (left-right)

        // Smoothly tilt the platform based on the input
        Quaternion targetRotation = Quaternion.Euler(tiltX, 0f, tiltZ);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
    }
}
