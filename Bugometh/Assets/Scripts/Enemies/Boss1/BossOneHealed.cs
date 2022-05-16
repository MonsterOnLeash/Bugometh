using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneHealed : MonoBehaviour
{
    private GameObject player_obj;
    private float direction;
    private void Flip()
    {
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        direction *= -1f;
    }
    private void Start()
    {
        player_obj = GameObject.FindGameObjectWithTag("Player");
        direction = -1f;
    }
    void Update()
    {
        if ((player_obj.transform.position.x - transform.position.x) * direction < 0 &&
                Mathf.Abs(player_obj.transform.position.x - transform.position.x) > 0.1f)
        {
            Flip();
        }
    }
}
