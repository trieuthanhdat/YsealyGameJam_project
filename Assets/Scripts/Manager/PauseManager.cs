using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject quitButton;
    public Text timerText;

    private bool isPaused = false;
    private float timeScaleBeforePause;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused)
        {
            timerText.text = "Time: " + Mathf.RoundToInt(Time.realtimeSinceStartup).ToString();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        Time.timeScale = 0f;
    }

    private void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        Time.timeScale = timeScaleBeforePause;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        // Reload current scene or reset game state
    }

    public void Quit()
    {
        Application.Quit();
    }
}
