using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroNPC : MonoBehaviour
{
    [SerializeField]
    private GameObject wizard;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            wizard.SetActive(true);
            GetComponent<DialogueTrigger>().TriggerDialogue();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!GameMaster.OngoingDialogue())
            {
                Destroy(wizard);
                Destroy(gameObject);
            }
        }
    }

}
