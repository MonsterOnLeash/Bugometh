using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<DialogueSentence> sentences;

    private GameObject dialoguePanel;

    private UIControls uiControls;

    public Text nameText;
    public Text dialogueText;
    public Image dialogueImage;

    private string currentDialogueID;
    private bool dialogueInProgress;

    private void Awake()
    {
        dialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
        nameText = dialoguePanel.transform.Find("NameText").gameObject.GetComponent<Text>();
        dialogueText = dialoguePanel.transform.Find("DialogueText").gameObject.GetComponent<Text>();
        dialogueImage = dialoguePanel.transform.Find("Image").gameObject.GetComponent<Image>();
        sentences = new Queue<DialogueSentence>();
        uiControls = new UIControls();
    }
    void Start()
    {
        dialoguePanel.transform.localScale = new Vector3(0, 0, 0);
        uiControls.Basic.NextSentence.performed += _ => OnNextSentencePress();
        dialogueInProgress = false;
    }

    private void OnNextSentencePress()
    {
        if (dialogueInProgress)
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (GameMaster.OngoingDialogue())
            return;

        ChangeGameState();
        GameMaster.BeginDialogue();
        dialoguePanel.transform.localScale = new Vector3(1, 1, 1);
        Debug.Log("Starting dialogue " + dialogue.id);

        currentDialogueID = dialogue.id;
        dialogueInProgress = true;

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

    private IEnumerator printSentence(string sentence)
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
        dialogueInProgress = false;
        sentences.Clear();
        dialoguePanel.transform.localScale = new Vector3(0, 0, 0);
        GameMaster.EndDialogue();
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

    private void OnEnable()
    {
        uiControls.Enable();
    }
    private void OnDisable()
    {
        uiControls.Disable();
    }
}
