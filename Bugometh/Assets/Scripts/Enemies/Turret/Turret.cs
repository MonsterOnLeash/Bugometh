using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [SerializeField]
    private Transform projectileSpawnPoint;

    [SerializeField]
    private GameObject projectile;

    private float shotInterval;
    private float beforeNextShot;

    private Vector2 direction;

    private void Start()
    {
        shotInterval = 1;
        beforeNextShot = 0;
        if (projectileSpawnPoint.position.x > transform.position.x)
            direction = new Vector2(1, 0);
        else
            direction = new Vector2(-1, 0);
        MaxHP = 5;
        CurrentHP = MaxHP;
    }

    void Update()
    {
        if (Physics2D.Raycast(projectileSpawnPoint.position, direction, 20, LayerMask.GetMask("Player")))
        {
            if (beforeNextShot <= 0)
            {
                GameObject bullet = Instantiate(projectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation);

                bullet.GetComponent<TurretProjectile>().SetSpeed(direction.x);
                beforeNextShot = shotInterval;
            }
            else
            {
                beforeNextShot -= Time.deltaTime;
            }
        }
        else
        {
            beforeNextShot = 0;
        }
    }
}
