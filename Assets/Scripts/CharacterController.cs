using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private FixedJoint2D skateboardAttachment;
    public UIController uiController;

    private void Start()
    {
        skateboardAttachment = GetComponent<FixedJoint2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            uiController.isGameOver = true;
            skateboardAttachment.enabled = false;
        }
    }
}
