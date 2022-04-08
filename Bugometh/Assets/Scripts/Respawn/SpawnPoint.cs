using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    //private GameObject player;
    private BoxCollider2D bc;
    
    public void Save(GameObject player)
    {
        SpawnManager.NewPoint(transform.position);
        player.GetComponent<PlayerControls>().SaveSettings();
        player.GetComponent<PlayerBasic>().SaveSettings();
        Debug.Log("Saved");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("SaveArea");
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerControls>().ActionRequired())
        {
            Save(collision.gameObject);
        }
    }
}
