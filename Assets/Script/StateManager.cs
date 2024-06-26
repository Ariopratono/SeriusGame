using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public static StateManager Instance { get; private set; }

    public enum GameState
    {
        start, ingame, paused, end
    }
    public GameState gameState;

    public GameObject objectivePanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        ChangeState(GameState.start);
    }

    private void Update()
    {
        CheckResumeInput();
    }

    private void CheckResumeInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.ingame)
            {
                PauseButton();
            }
            else if (gameState == GameState.paused)
            {
                ResumeButton();
            }
        }
    }

    public void ChangeState(GameState state)
    {
        gameState = state;
        switch(gameState)
        {
            case GameState.start:
                Time.timeScale = 0;
                objectivePanel.SetActive(true);
                break;
            case GameState.ingame:
                Time.timeScale = 1;
                pausePanel.SetActive(false);
                objectivePanel.SetActive(false);
                break;
            case GameState.paused:
                Time.timeScale = 0;
                pausePanel.SetActive(true);
                break;
            case GameState.end:
                Time.timeScale = 0;
                if(StatsManager.Instance.objectiveComplete == true)
                {
                    winPanel.SetActive(true);
                }
                else
                {
                    losePanel.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("Game state could not be determined!");
                break;
        }
    }

    public void PauseButton()
    {
        ChangeState(GameState.paused);
    }

    public void ResumeButton()
    {
        ChangeState(GameState.ingame);
    }

    public void RestartButton()
    {
        FindObjectOfType<GameManager>().ReloadScene();
    }

    public void QuitButton()
    {
        FindObjectOfType<GameManager>().QuitApplication();
    }
}
