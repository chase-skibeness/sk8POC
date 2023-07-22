using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    private Vector3 respawnPosition;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                StartCoroutine(DieAndRespawn());
            }
        }
    }

    public IEnumerator DieAndRespawn()
    {
        isDead = true;

        yield return new WaitForSeconds(2);

        transform.position = respawnPosition;
        isDead = false;
    }
}
