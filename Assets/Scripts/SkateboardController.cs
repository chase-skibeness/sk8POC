using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkateboardController : MonoBehaviour
{
    private float timeSinceLastPush = 0f;
    private Vector2 lastStickPosition = Vector2.zero;
    private float lastFlickTime = 0f;
    private float flickTimeWindow = 0.15f; // The time window for the flick action.
    private float flickTimeDeadzone = 0.03f;
    private float joystickThreshold = 0.5f; // The joystick movement threshold for registering a flick.
    private int lastStickDirection = 0;
    private bool isGrounded = false;
    private int numWheelsTouchingGround = 0;
    private int orientation = 1;
    public SpriteRenderer[] spriteRenderers;
    public GameObject Wizard;
    public Animator playerAnimationController;

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
        playerAnimationController.SetFloat("Speed", rb.velocity.magnitude);

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

            // Check if stick was flicked to the right or left.
            if (currentStickDirection != 0 && currentStickDirection == lastStickDirection && Time.time - lastFlickTime < flickTimeWindow && Time.time - lastFlickTime > flickTimeDeadzone)
            {
                if (orientation != currentStickDirection)
                {
                    foreach (SpriteRenderer spriteRenderer in spriteRenderers) { spriteRenderer.flipX = !spriteRenderer.flipX; }
                    orientation = currentStickDirection;
                }
                
                
            }

            if (Gamepad.current.leftStick.left.isPressed)
            {
                rb.AddTorque(rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Gamepad.current.leftStick.right.isPressed)
            {
                rb.AddTorque(-rotationSpeed * Time.deltaTime, ForceMode2D.Impulse);
            }

            if (Gamepad.current.leftShoulder.isPressed)
            {
                orientation = -1;
                Wizard.transform.localScale = new Vector3(1, -1, 1);
                foreach (SpriteRenderer spriteRenderer in spriteRenderers) { spriteRenderer.flipX = true; }
            }

            if (Gamepad.current.rightShoulder.isPressed)
            {
                orientation = 1;
                Wizard.transform.localScale = new Vector3(1, 1, 1);
                foreach (SpriteRenderer spriteRenderer in spriteRenderers) { spriteRenderer.flipX = false; }
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
            playerAnimationController.SetTrigger("OlliePerformed");
            rb.AddForce(new Vector2(0, ollieForce), ForceMode2D.Force);
        }
    }

    private void PushSkateboardForward()
    {
        // Add a cooldown to prevent spamming the push.
        if (Time.time - timeSinceLastPush >= pushCooldown && isGrounded)
        {
            playerAnimationController.SetTrigger("PushPressed");
            Vector2 pushDirection = new Vector2(orientation * transform.right.x, orientation * transform.right.y); // Use the right direction instead of forward.
            rb.AddForce(pushDirection * pushSpeed, ForceMode2D.Force);

            timeSinceLastPush = Time.time;
        }
    }
}
