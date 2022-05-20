using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCaller : MonoBehaviour
{
    public int destroyAfterReadDefault;
    private int destroyAfterRead;
    [SerializeField]
    private int dialogueNumber;
    private void Awake()
    {
        destroyAfterRead = PlayerPrefs.GetInt("Dialogue" + dialogueNumber.ToString(), 0);
        if (destroyAfterRead == 1)
            Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (collision.gameObject.GetComponent<PlayerControls>().ActionRequired()))
        {
            Debug.Log("Call");
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
        if (destroyAfterReadDefault == 1)
            PlayerPrefs.SetInt("Dialogue" + dialogueNumber.ToString(), 1);
    }

}
