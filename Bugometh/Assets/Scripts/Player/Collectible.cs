using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectible : MonoBehaviour
{
    [SerializeField]
    int blue;
    [SerializeField]
    bool red;
    [SerializeField]
    bool green;
    [SerializeField]
    int jump;
    [SerializeField]
    int damage;
    [SerializeField]
    int hp;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject player = collision.gameObject;

            player.GetComponent<PlayerControls>().shotEnabled = Math.Max(player.GetComponent<PlayerControls>().shotEnabled, blue);
            
            player.GetComponent<PlayerControls>().unlockedColors[1] |= red;
            
            player.GetComponent<PlayerControls>().unlockedColors[2] |= green;
            
            player.GetComponent<PlayerControls>().extraJumps += jump;
            
            for (int i = 0; i < 3; i++)
                player.GetComponent<PlayerControls>().attackPowerList[i] += damage;
            
            
            player.GetComponent<PlayerBasic>().IncreaseMaxHP(hp);
            player.GetComponent<PlayerBasic>().IncreaseCurrentHP(hp);
            
            Destroy(gameObject);
        }
    }
}
