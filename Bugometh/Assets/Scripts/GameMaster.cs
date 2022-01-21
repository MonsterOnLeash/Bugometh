using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static void KillPlayer(PlayerBasic player)
    {
        Destroy(player);
        Debug.Log("you died");
        // show some respawn message
    }
}
