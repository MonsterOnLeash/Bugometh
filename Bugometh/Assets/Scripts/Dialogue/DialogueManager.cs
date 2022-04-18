using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueSentence> sentences;

    private GameObject dialoguePanel;

    public Text nameText;
    public Text dialogueText;
    public Image dialogueImage;

    private string currentDialogueID;

    private void Awake()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
        sentences = new Queue<DialogueSentence>();
    }
    void Start()
    {
        dialoguePanel.transform.localScale = new Vector3(0, 0, 0);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        ChangeGameState();
        dialoguePanel.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("Starting dialogue " + dialogue.id);

        currentDialogueID = dialogue.id;

        foreach (DialogueSentence s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
       
        DialogueSentence current_sentence = sentences.Dequeue();
        dialogueImage.sprite = current_sentence.image;
        nameText.text = current_sentence.name;
        StopAllCoroutines();
        StartCoroutine(printSentence(current_sentence.text));
    }

    IEnumerator printSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char c in sentence.ToCharArray())
        {
            dialogueText.text += c;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        Debug.Log("Ending dialogue " + currentDialogueID);
        currentDialogueID = "";
        sentences.Clear();
        dialoguePanel.transform.localScale = new Vector3(0, 0, 0);
        ChangeGameState();
    }
    private void ChangeGameState()
    {
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.Gameplay ?
            GameState.Paused : GameState.Gameplay;
        GameStateManager.Instance.SetState(newGameState);
        if (newGameState == GameState.Gameplay)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
