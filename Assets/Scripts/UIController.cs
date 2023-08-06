using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject restartText;
    public bool isGameOver = false;

    void Start()
    {
        //Disables panel if active
        gameOverPanel.SetActive(false);
        restartText.gameObject.SetActive(false);
    }

    void Update()
    {
        if ((Gamepad.current.startButton.isPressed || Keyboard.current.rKey.isPressed) && !isGameOver)
        {
            isGameOver = true;

            StartCoroutine(GameOverSequence());
        }

        if (isGameOver)
        {   
            StartCoroutine(GameOverSequence());

            if (Keyboard.current.rKey.isPressed || Gamepad.current.startButton.isPressed)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    private IEnumerator GameOverSequence()
    {
        gameOverPanel.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        restartText.SetActive(true);
    }
}