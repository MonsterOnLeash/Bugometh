using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomSpikes : MonoBehaviour
{
    [SerializeField]
    Transform revivePos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBasic>().DamageFixed(1);
            collision.transform.position = revivePos.position;
        }
    }
}
