using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;
    [SerializeField]
    Transform spawnPoint;
    [SerializeField]
    Transform deathPoint;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > deathPoint.position.y)
            transform.position = spawnPoint.position;
        transform.Translate(direction * Time.deltaTime);
    }
}
