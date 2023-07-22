using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float timeSinceLastPush = 0f;
    private Vector2 lastStickPosition = Vector2.zero;
    private float lastFlickTime = 0f;
    private float flickTimeWindow = 0.15f; // The time window for the flick action.
    private float joystickThreshold = 0.5f; // The joystick movement threshold for registering a flick.
    private int lastStickDirection = 0;
    private bool isGrounded = false;
    private int numWheelsTouchingGround = 0;

    public float ollieTimerThreshold = 0.8f;
    public float ollieForce = 500f;
    public float pushSpeed = 500f;
    public float rotationSpeed = 5f;
    public float pushCooldown = 0.7f;

    public Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Gamepad.current != null)
        {
            Vector2 stickPosition = Gamepad.current.leftStick.ReadValue();
            int currentStickDirection = 0;

            // Determine the current stick direction.
            if (stickPosition.x > joystickThreshold)
            {
                currentStickDirection = 1;
            }
            else if (stickPosition.x < -joystickThreshold)
            {
                currentStickDirection = -1;
            }

            // Check if stick was flicked to the right or left.
            if (currentStickDirection != 0 && currentStickDirection != lastStickDirection && Time.time - lastFlickTime < flickTimeWindow)
            {
                PerformOllie();
            }



            if (Gamepad.current.leftStick.left.isPressed)
            {
                rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);


            }

            if (Gamepad.current.leftStick.right.isPressed)
            {
                rb.AddTorque(-rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Gamepad.current.aButton.wasReleasedThisFrame)
            {
                PushSkateboardForward();
            }

            if (Gamepad.current.bButton.wasReleasedThisFrame)
            {
                PerformOllie();
            }

            // Update the last stick position and direction.
            lastStickPosition = stickPosition;
            if (currentStickDirection != 0)
            {
                lastStickDirection = currentStickDirection;
                lastFlickTime = Time.time;
            }
        }
        else
        {
            if (Keyboard.current.aKey.isPressed)
            {
                rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
                if (Keyboard.current.dKey.wasReleasedThisFrame)
                {
                    PerformOllie();
                }
            }

            if (Keyboard.current.dKey.isPressed)
            {
                rb.AddTorque(-rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
                if (Keyboard.current.aKey.wasReleasedThisFrame)
                {
                    PerformOllie();
                }
            }

            if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            {
                PushSkateboardForward();
            }
        }
    }

    public void WheelTouchesGround()
    {
        numWheelsTouchingGround++;
        isGrounded = IsGrounded();
    }

    public void WheelLeavesGround()
    {
        numWheelsTouchingGround--;
        isGrounded = IsGrounded();
    }

    private bool IsGrounded()
    {
        return numWheelsTouchingGround > 0;
    }

    private void PerformOllie()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector2(0, ollieForce), ForceMode2D.Force);
        }
    }

    private void PushSkateboardForward()
    {
        // Add a cooldown to prevent spamming the push.
        if (Time.time - timeSinceLastPush >= pushCooldown && isGrounded)
        {
            Vector2 pushDirection = new Vector2(transform.right.x, transform.right.y); // Use the right direction instead of forward.
            rb.AddForce(pushDirection * pushSpeed, ForceMode2D.Force);

            timeSinceLastPush = Time.time;
        }
    }
}