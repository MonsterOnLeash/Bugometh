using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : Enemy
{
    private float direction;
    public Transform touchPoint;
    public float checkRadius;
    public LayerMask whatIsWall;

    public LayerMask whatToAttack;
    public float StrikeRange;
    public float TriggerRange;
    public float ThowbackDuration; // how much time will the player be thown back after stike
    public float ThrowbackDistance;
    public float TimeBetweenStrikes; // time in seconds between strikes, if player is within range for a long time

    private float since_last_strike; // time in seconds since last strike
    private bool is_thowing_back = false; // shows if we are trying to thow player back or not
    private float thowback_power;

    private PlayerBasic player;


    void Flip()
    {
        direction *= -1;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
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
        if (Physics2D.OverlapCircle(touchPoint.position, checkRadius, whatIsWall))
        {
            Flip();
        }
        if (able_to_move)
        {
            transform.Translate(new Vector2(direction * speed * Time.deltaTime, 0));
            Collider2D collider = Physics2D.OverlapCircle(transform.position, StrikeRange, whatToAttack);
            if (collider)
            {
                if ((player = collider.gameObject.GetComponent<PlayerBasic>()) && since_last_strike >= TimeBetweenStrikes)
                {
                    player.DamageFixed(1);
                    since_last_strike = 0;
                    thowback_power = ThrowbackDistance / ThowbackDuration;
                    is_thowing_back = true;
                }
                else if (since_last_strike < float.MaxValue)
                {
                    since_last_strike += Time.deltaTime;
                }
            }
            else if (since_last_strike < float.MaxValue)
            {
                since_last_strike += Time.deltaTime;
            }
            if (is_thowing_back && collider && since_last_strike < ThowbackDuration)
            {
                Vector2 initial_vector = collider.gameObject.transform.position - transform.position; // discarding z coordinate here
                if (initial_vector.magnitude < StrikeRange + 0.4) //0.4f is approximately half the hight of our dude
                {
                    collider.gameObject.transform.Translate(initial_vector.normalized * Time.deltaTime * thowback_power);
                    gameObject.transform.Translate(new Vector2(initial_vector.normalized.x * (-1) * Time.deltaTime * thowback_power, 0));
                }
                else
                {
                    is_thowing_back = false;
                }
            }
        }
    }
}
