using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    public float speed;

    public int damage;

    // Start is called before the first frame update
    void Awake()
    {
        damage = 1;
        speed = 2;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void SetSpeed(float x)
    {
        speed = x;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer + " " + LayerMask.GetMask("Player"));

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<PlayerBasic>().DamageFixed(damage);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(gameObject);
    }
}
