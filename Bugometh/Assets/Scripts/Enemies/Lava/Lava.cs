using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LivingThing victim = collision.gameObject.GetComponent<LivingThing>();
        if (victim)
        {
            victim.OnDeath();
            Debug.Log("collided with lava");
        }
    }
}
