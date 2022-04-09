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
    private void Escape()
    {
        pause_menu.SetActive(!pause_menu.activeSelf);
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ?
            GameState.Paused : GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);

    }

    private void Resume()
    {
        pause_menu.SetActive(false);
        // the code here is the same as in Escape() method for now
        // but we should probably just set the gamestate instead of switching it
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ?
            GameState.Paused : GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);
    }
    private void Start()
    {
        pause_menu = GameObject.FindGameObjectWithTag("PauseMenu");
        pause_menu.SetActive(false);
        playerInput = GetComponent<PlayerInput>();
        hp_bar = GameObject.FindGameObjectWithTag("HPBar");
    }
    private void Update()
    {
        //if (playerInput.actions["Escape"].triggered)
        //{
        //    Escape();
        //}
    }
}
