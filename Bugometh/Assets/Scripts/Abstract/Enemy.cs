using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingThing
{
    public virtual void Move() { }

    public override void OnDeath()
    {
        Debug.Log("enemy got killed");
        Destroy(gameObject);
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateGhanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
