using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private PanelManager panelManager;
    private UIControls uiControls;

    private static bool dialogueInProgress;
    private bool isPaused; // used to define state if dialogue is in progress
    public static void KillPlayer(GameObject player)
    {
        Destroy(player);
        Debug.Log("you died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public static void BeginDialogue()
    {
        dialogueInProgress = true; 
    }

    public static bool OngoingDialogue()
    {
        return dialogueInProgress;
    }

    public static void EndDialogue()
    {
        dialogueInProgress = false;
    }
    public void QuitGame()
    {
        Debug.Log("game quit from game master");
        Application.Quit();
    }
    public void Escape()
    {
        if (!dialogueInProgress)
        {
            // we have to change GameState
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay ?
                GameState.Paused : GameState.Gameplay;
            GameStateManager.Instance.SetState(newGameState);
            if (newGameState == GameState.Gameplay)
            {
                Resume();
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                Pause();
            }
        } else
        {
            // means that GameState is Paused, and we should only show or unshow the pauseMenuPanel
            if (isPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
        
    }
    private void Pause()
    {
        panelManager.ShowPanel("PauseMenuPanel");
        isPaused = true;
    }
    private void Resume()
    {
        panelManager.HideAllPanels();
        isPaused = false;

    }
    private void Awake()
    {
        uiControls = new UIControls();
        panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
    }
    private void Start()
    {
        uiControls.Basic.Escape.performed += _ => Escape();
        GameStateManager.Instance.SetState(GameState.Gameplay);
        Time.timeScale = 1;
        dialogueInProgress = false;
        isPaused = false;
    }
    private void OnEnable()
    {
        uiControls.Enable();
    }
    private void OnDisable()
    {
        uiControls.Disable();
    }
}
