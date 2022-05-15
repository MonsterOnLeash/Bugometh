using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class BossOneRoom : MonoBehaviour
{
    public GameObject boss;
    public GameObject healedBossPrefab;
    private bool fightStarted;
    public void StartBossFight()
    {
        fightStarted = true;
        boss.SetActive(true);
        boss.GetComponent<BossOneController>().StartFight();
        // TODO instantiate walls
    }

    public void EndFight(Vector3 position) // called when Boss1 is defeated
    {
        position.y -= 1;
        PlayerPrefs.SetString("BossOneResult", Vector3Serializer.Serialize(position));

        SpawnHealed(position);
        // TODO destroy walls
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!fightStarted && PlayerPrefs.GetString("BossOneResult", "").Length == 0)
            {
                StartBossFight();
            }
        }
    }

    private void SpawnHealed(Vector3 position)
    {
        GameObject healed = Instantiate(healedBossPrefab, position, Quaternion.identity);
        healed.SetActive(true);
    }
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        fightStarted = false;
        string position_str = PlayerPrefs.GetString("BossOneResult", "");
        if (position_str.Length > 0)
        {
            Vector3 position = Vector3Serializer.Deserialize(position_str);
            SpawnHealed(position);
        }
    }
}
