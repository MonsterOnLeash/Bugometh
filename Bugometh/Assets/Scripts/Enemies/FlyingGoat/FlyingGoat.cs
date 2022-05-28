using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingGoat : Enemy
{
    bool facingLeft;
    Vector2 direction;
    Collider2D col;

    private void Awake()
    {
        MaxHP = 2;
        CurrentHP = MaxHP;
        direction = new Vector2(-1, 1);
        col = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerBasic>().DamageFixed(1);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if ((collision.GetContact(0).point.x < col.transform.position.x) == facingLeft)
            {
                Flip();
            }
            Ricochet(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            Destroy(gameObject);
        }
    }

    void Flip()
    {
        facingLeft = !facingLeft;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Ricochet(Collision2D collision)
    {
        if (collision.GetContact(0).point.x != col.transform.position.x)
            direction.x *= -1;
        if (collision.GetContact(0).point.y != col.transform.position.y)
            direction.y *= -1;
    }
}
