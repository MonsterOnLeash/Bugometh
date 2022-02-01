using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : Enemy
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

    void Flip()
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
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            is_in_contact = false;
        }
    }
    private void Awake()
    {
        able_to_move = true;
        CurrentHP = MaxHP;
        direction = -1;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
        if (Random.Range(0, 2) == 1)
        {
            Flip();
        }
        since_last_strike = float.MaxValue;
        is_in_contact = false;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (able_to_move)
        {
            // flip if hit the wall
            if (Physics2D.OverlapCircle(touchPoint.position, checkRadius, whatIsWall))
            {
                Flip();
            }

            // flip if got to the edge
            if (Physics2D.OverlapCircle(edgePoint.position, checkRadius, whatIsGround) == null)
            {
                Flip();
            }

            // move the spider
            transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
            
            if (since_last_strike < float.MaxValue)
            {
                since_last_strike += Time.deltaTime;
            }
            // strike player
            if (is_in_contact && since_last_strike >= timeBetweenStrikes)
            {
                player.DamageFixed(1);
                since_last_strike = 0;
                float throwback_direction = (player_obj.transform.position - transform.position).x < 0 ? -1 : 1;
                PlayerControls player_controls = player_obj.GetComponent<PlayerControls>();
                player_controls.ApplyForce(new Vector2(throwback_direction * xThrowbackForce, yThrowbackForce), thowbackDuration);
            }
        }
    }
}
