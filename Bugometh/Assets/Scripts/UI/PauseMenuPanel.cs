using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPanel : MonoBehaviour
{
    public void ResumeGameButton()
    {
        GameObject.Find("GameMaster").GetComponent<GameMaster>().Escape();
    }

    public void MainMenuButton()
    {
        Debug.Log("back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}

