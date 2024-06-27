using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameManager gameManager;
    public Joystick rotationJoystick;
    public float rotationSpeed;

    private bool ignoreMouse = false;

    void Update()
    {
        float horizontalInput = rotationJoystick.Horizontal != 0 || MouseIsIgnored() ? rotationJoystick.Horizontal : Input.GetAxis("Mouse X") / Screen.width * gameManager.mouseSensitivity;
        float verticalInput = rotationJoystick.Vertical != 0 || MouseIsIgnored() ? rotationJoystick.Vertical : Input.GetAxis("Mouse Y") / Screen.height * gameManager.mouseSensitivity;

        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.Rotate(0, horizontalInput * rotationSpeed * Time.deltaTime, 0, Space.World);

            float verticalRotation = -verticalInput * rotationSpeed * Time.deltaTime;

            if (transform.rotation.eulerAngles.x + verticalRotation < 90 || transform.rotation.eulerAngles.x + verticalRotation > 270)
            {
                transform.Rotate(-verticalInput * rotationSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    public void SetIgnoreMouse(bool ignore)
    {
        ignoreMouse = ignore;
    }

    public bool MouseIsIgnored()
    {
        return ignoreMouse;
    }
}