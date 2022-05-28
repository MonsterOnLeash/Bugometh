using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : Enemy
{
    [SerializeField]
    float timeBetweenShots;
    [SerializeField]
    float laserDuration;
    float currentTimePeace;
    float currentTimeAction;
    [SerializeField]
    Animator anim;
    [SerializeField]
    List<GameObject> laserBeam;
    bool attack;


    void Awake()
    {
        MaxHP = 5;
        CurrentHP = MaxHP;
        timeBetweenShots = 3;
        currentTimePeace = timeBetweenShots;
        currentTimeAction = 0;
        laserDuration = 1;
        attack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTimePeace > 0)
        {
            currentTimePeace -= Time.deltaTime;
            return;
        }
        if (currentTimeAction > 0)
        {
            currentTimeAction -= Time.deltaTime;
            return;
        }
        if (!attack)
        {
            attack = true;
            anim.Play("LaserShooter");
            currentTimeAction = laserDuration;
            for (int i = 0; i < laserBeam.Capacity; i++)
                laserBeam[i].SetActive(true);
            return;
        }
        attack = false;
        anim.Play("DefaultLaser");
        currentTimePeace = timeBetweenShots;
        for (int i = 0; i < laserBeam.Capacity; i++)
            laserBeam[i].SetActive(false);
    }
}
