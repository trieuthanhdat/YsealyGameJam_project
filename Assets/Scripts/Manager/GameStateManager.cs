using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
   
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject gameWinPanel;
    public GameObject tutorialPanel;

    private bool isPaused;
    private bool isGameOver;
    private bool isGameWin;
    private bool isGameHard;

    public enum GameStates
    {
        GAMEPLAY,
        GAMEPAUSE,
        GAMEOVER,
        GAMEWON,
        NONE
    }
    
    private GameStates currentGameState;
    public GameStates CurrentGameState {get => currentGameState; set => currentGameState = value;}

    void Start()
    {
        isPaused = false;
        isGameOver = false;
        isGameWin = false;

        if(!pausePanel)
            pausePanel = GameObject.FindGameObjectWithTag("PausePanel");
        pausePanel .SetActive(false);
        if(!gameOverPanel)
            gameOverPanel = GameObject.FindGameObjectWithTag("LosePanel");
        gameOverPanel.SetActive(false);
        if(!gameWinPanel)
            gameWinPanel = GameObject.FindGameObjectWithTag("WinPanel");
        gameWinPanel.SetActive(false);
        
        int temp = PlayerPrefs.GetInt("IsDoneTutorial", 0);
        if(temp ==  0)
            OpenTutorial();
        else
            tutorialPanel.SetActive(false);

    }

    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
       switch(CurrentGameState)
       {
            case GameStates.GAMEPLAY:
                ResumeGame();
                break;
            case GameStates.GAMEPAUSE:
                PauseGame();
                break;
            case GameStates.GAMEOVER:
                GameOver();
                break;
            case GameStates.GAMEWON:
                GameWin();
                break;
       }
    }

    public void OpenTutorial()
    {
        isPaused = true;
        Time.timeScale =  0;
        tutorialPanel.SetActive(true);
        PlayerPrefs.SetInt("IsDoneTutorial", 1);
    }
    public void CloseTutorial()
    {
        isPaused = false;
        Time.timeScale =  1;
        tutorialPanel.SetActive(false);
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale =  0;
        pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void GameWin()
    {
        isGameWin = true;
        gameWinPanel.SetActive(true);
    }
    // public void Restart()
    // {
    //     Time.timeScale = 1;
    // }
    // public void BackToMainMenu()
    // {
    //     Time.timeScale = 1;
    // }
}
