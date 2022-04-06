using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private GameObject pause_menu;
    private GameObject hp_bar;
    private PlayerInput playerInput;
    private PanelManager panelManager;
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
        //pause_menu.SetActive(!pause_menu.activeSelf);
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
        //pause_menu.SetActive(false);
        panelManager.HideLastPanel();

    }
    private void Awake()
    {
        //pause_menu = GameObject.FindGameObjectWithTag("PauseMenu");
        //pause_menu.SetActive(false);
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        //hp_bar = GameObject.FindGameObjectWithTag("HPBar");
        panelManager = GameObject.Find("PanelManager").GetComponent<PanelManager>();
    }
    private void Update()
    {
        if (playerInput.actions["Escape"].triggered)
        {
            Debug.Log("Escape");
            Escape();
        }
    }
}
