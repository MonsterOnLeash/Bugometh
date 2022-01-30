using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBasic : LivingThing
{
    private GameObject player;
    public override void OnDeath()
    {
        GameMaster.KillPlayer(player);
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateGhanged;
    }
    // Start is called before the first frame update

    public override void OnGameStateGhanged(GameState gameState)
    {
        Debug.Log("player's pause state changed");
        able_to_move = gameState == GameState.Gameplay;
    }
    void Start()
    {
        CurrentHP = MaxHP;
        able_to_move = true;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
