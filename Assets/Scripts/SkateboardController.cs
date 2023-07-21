using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkateboardController : MonoBehaviour
{
    private float rotationSpeed = 5f; // Adjust the rotation speed to your preference.
    private float pushSpeed = 500f;
    private float pushCooldown = 0.7f;
    private float timeSinceLastPush = 0.7f;
    private float ollieTimerThreshold = 0.8f;
    private float ollieForce = 500f;

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.leftStick.left.isPressed)
            {
                rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Gamepad.current.leftStick.right.isPressed)
            {
                rb.AddTorque(-rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Gamepad.current.aButton.wasPressedThisFrame)
            {
                PushSkateboardForward();
            }

            if (Gamepad.current.bButton.wasPressedThisFrame)
            {
                PerformOllie();
            }
        }
        else
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                PushSkateboardForward();
            }
        }
    }

    private void PerformOllie()
    {
        rb.AddForce(new Vector2(0, ollieForce), ForceMode2D.Force);
    }

    private void PushSkateboardForward()
    {
        // Add a cooldown to prevent spamming the push.
        if (Time.time - timeSinceLastPush >= pushCooldown)
        {
            Vector2 pushDirection = new Vector2(transform.right.x, transform.right.y); // Use the right direction instead of forward.
            rb.AddForce(pushDirection * pushSpeed, ForceMode2D.Force);

            timeSinceLastPush = Time.time;
        }
    }
}
