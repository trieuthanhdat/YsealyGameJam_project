using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggle : MonoBehaviour
{
    GameStateManager gameStateManager;
    public KeyCode toggleKey = KeyCode.Escape;
    public GameObject togglePanel;

    public bool shouldPause = false;

    void Start()
    {
        togglePanel.SetActive(false);
        gameStateManager = GameObject.FindObjectOfType<GameStateManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
            togglePanel.SetActive(!togglePanel.activeInHierarchy);
            gameStateManager.CurrentGameState = shouldPause == true ? GameStateManager.GameStates.GAMEPAUSE : GameStateManager.GameStates.GAMEPLAY;
    }
}
