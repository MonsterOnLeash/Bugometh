using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneController : Enemy
{
    private float direction;
    public Transform touchPoint;
    public Transform edgePoint;
    public float checkRadius;
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    public LayerMask whatToAttack;
    public float thowbackDuration; // how much time will the player be thown back after stike
    public float xThrowbackForce; // throwback force on x axis
    public float yThrowbackForce; // throwback force on y axis
    public float timeBetweenStrikes; // time in seconds between strikes, if player is within range for a long time

    private float since_last_strike; // time in seconds since last strike
    private bool is_in_contact;

    private PlayerBasic player;
    private GameObject player_obj;

    private void Flip()
    {
        direction *= -1;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player_obj = collision.gameObject;
            player = collision.gameObject.GetComponent<PlayerBasic>();
            is_in_contact = true;
        }
    }

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
    }

    void Update()
    {
        
    }
}
