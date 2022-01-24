using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string SceneToLoad = "MainScene";
    public void StartGame()
    {
        Debug.Log("start game");
        SceneManager.LoadScene(SceneToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
