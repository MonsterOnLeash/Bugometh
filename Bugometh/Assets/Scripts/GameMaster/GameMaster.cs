using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private PanelManager panelManager;
    private UIControls uiControls;
    public static void KillPlayer(GameObject player)
    {
        Destroy(player);
        Debug.Log("you died");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Debug.Log("game quit from game master");
        Application.Quit();
    }
    public void Escape()
    {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ?
            GameState.Paused : GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);
        if (newGameState == GameState.Gameplay)
        {
            Resume();
        } else
        {
            Pause();
        }
    }
    private void Pause()
    {
        Time.timeScale = 0;
        panelManager.ShowPanel("PauseMenuPanel");
    }
    private void Resume()
    {
        Time.timeScale = 1;
        panelManager.HideLastPanel();

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
