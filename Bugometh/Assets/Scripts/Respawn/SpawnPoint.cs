using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    //private GameObject playerPrefab;
    //private GameObject player;
    private BoxCollider2D bc;
    [SerializeField]
    private CinemachineVirtualCamera cam;
    [SerializeField]
    private CameraManager cm;

    public void Save(GameObject player)
    {
        SpawnManager.NewPoint(transform.position);
        player.GetComponent<PlayerControls>().SaveSettings();
        player.GetComponent<PlayerBasic>().SaveSettings();
        cm.ChangeSpawnCamera(cam);
        Debug.Log("Saved");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("SaveArea");
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<PlayerControls>().ActionRequired())
        {
            Save(collision.gameObject);
        }
    }
}
