using UnityEngine;

public class WheelController : MonoBehaviour
{
    public SkateboardController skateboardController;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            skateboardController.WheelTouchesGround();
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
