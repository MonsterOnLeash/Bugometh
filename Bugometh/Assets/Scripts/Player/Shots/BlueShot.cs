using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueShot : MonoBehaviour
{
    int damage;
    Vector2 direction;
    float speed;
    public static int numberOfShots; 

    private void Awake()
    {
        damage = 2;
        direction = new Vector2(1, 0);
        speed = 3;
        numberOfShots += 1;
    }

    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    public void SetProps(float x, float y, int dam)
    {
        direction = new Vector2(x, y);
        damage = dam;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().DamageFixed(damage);
            Destroy(gameObject);
            numberOfShots -= 1;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            numberOfShots -= 1;
        }
    }
}
