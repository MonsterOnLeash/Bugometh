using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damageAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        LivingThing victim = collision.gameObject.GetComponent<LivingThing>();
        if (victim)
        {
            victim.DamageFixed(damageAmount);
        }
    }
}
