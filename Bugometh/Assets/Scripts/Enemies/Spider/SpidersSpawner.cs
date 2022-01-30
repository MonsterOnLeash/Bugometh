using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpidersSpawner : MonoBehaviour
{
    private float since_last_spawn;
    public float TimeBetweenSpawns;
    public GameObject SpiderPprefab;
    private void Start()
    {
        since_last_spawn = float.MaxValue;
    }

    void Update()
    {
        if (since_last_spawn >= TimeBetweenSpawns)
        {
            GameObject new_spider = Instantiate(SpiderPprefab, transform.position, Quaternion.identity);
            new_spider.SetActive(true);
            since_last_spawn = 0;
        }
        else if (since_last_spawn < float.MaxValue)
        {
            since_last_spawn += Time.deltaTime;
        }
    }
}
