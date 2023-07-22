using UnityEngine;

public class WheelController : MonoBehaviour
{
    public SkateboardController skateboardController;
    public PlayerController playerController;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            skateboardController.WheelTouchesGround();
            Debug.Log(rb.velocity.magnitude);
            if (rb.velocity.magnitude > skateboardController.maxSpeed && !playerController.isDead)
            {
                StartCoroutine(playerController.DieAndRespawn());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            skateboardController.WheelLeavesGround();
        }
    }
}
