using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenShot : MonoBehaviour
{
    Vector2 direction;
    float speed;
    public static int numberOfShots = 0;
    [SerializeField]
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = 10;
        direction = new Vector2(1, 0);
        numberOfShots = 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
            numberOfShots = 0;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Teleport"))
        {
            player.transform.position = gameObject.transform.position;
            Destroy(gameObject);
            numberOfShots = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Room"))
        {
            Destroy(gameObject);
            numberOfShots = 0;
        }
    }

    public void SetProps(float x, float y)
    {
        direction = new Vector2(x, y);
    }
}
