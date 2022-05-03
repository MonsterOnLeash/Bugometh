using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuPanel : MonoBehaviour
{
    private PanelManager panelManager;
    public void ResumeGameButton()
    {
        GameObject.Find("GameMaster").GetComponent<GameMaster>().Escape();
    }

    public void SettingsButton()
    {
        panelManager.ShowPanel("SettingsMenuPanel1", PanelShowBehaviour.HIDE_LAST);
    }

    public void MainMenuButton()
    {
        Debug.Log("back to main menu");
        SceneManager.LoadScene("MainMenu");
    }

    private void Start()
    {
        panelManager = PanelManager.Instance;
    }
}

