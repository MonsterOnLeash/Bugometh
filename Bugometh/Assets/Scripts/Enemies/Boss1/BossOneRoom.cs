using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneRoom : MonoBehaviour
{
    public GameObject boss;
    private bool fightStarted;
    public void StartBossFight()
    {
        fightStarted = true;
        boss.SetActive(true);
        boss.GetComponent<BossOneController>().StartFight();
        // TODO instantiate walls
    }

    public void EndFight() // called when Boss1 is defeated
    {
        PlayerPrefs.SetFloat("BossOneResult", 1f);
        // TODO destroy walls
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!fightStarted && PlayerPrefs.GetFloat("BossOneResult", 0f) < 0.5)
            {
                StartBossFight();
            }
        }
    }
    private void Start()
    {
        fightStarted = false;
    }
}
