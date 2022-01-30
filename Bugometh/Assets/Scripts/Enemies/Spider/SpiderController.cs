using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : Enemy
{
    private float direction;
    public Transform touchPoint;
    public float checkRadius;
    public LayerMask whatIsWall;

    private float since_last_strike; // time in seconds since last strike
    public float TimeBetweenStrikes; // time in seconds between strikes, if player is within range for a long time
    private bool is_in_conatact;
    private PlayerBasic player;


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
            is_in_conatact = true;
            player = collision.gameObject.GetComponent<PlayerBasic>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            since_last_strike = float.MaxValue;
            is_in_conatact = false;
        }
    }
    // Start is called before the first frame update
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
    }
    // Update is called once per frame
    private void Update()
    {
        //if (Physics2D.OverlapBox(touchPoint.position, new Vector2(2 * checkRadius, 2 * checkRadius), whatIsWall.value))
        //{
        //    Flip();
        //}
        if (Physics2D.OverlapCircle(touchPoint.position, checkRadius, whatIsWall))
        {
            Flip();
        }
        if (able_to_move)
        {
            transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
        }
        if (is_in_conatact)
        {
            if (since_last_strike >= TimeBetweenStrikes)
            {
                player.DamageFixed(1);
                since_last_strike = 0;
            } else if (since_last_strike < float.MaxValue)
            {
                since_last_strike += Time.deltaTime;
            }
        }
    }
}
