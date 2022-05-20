using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOneController : Enemy
{
    private float direction; // -1 or 1 direction of movement on x axis
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float checkRadius;
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    private bool isGrounded; // value from ground checker
    private bool previouslyGrounded; // value from previous frame

    private Rigidbody2D rb;

    [SerializeField]
    private float walkSpeed; // value by which vector (direction, 0) is multiplied to define walk speed

    private float timeSinceLastHop; // seconds since last hop
    [SerializeField]
    private float timeBetweenHops; // minimal time in seconds between hops
    [SerializeField]
    private float hopForce; // value by which vector (cos(hopAngle), sin(hopAngle)) is multiplied to define hop speed
    [SerializeField]
    private float hopAngle; // angle in radians at which hop is performed

    [SerializeField]
    private float attackForce;
    private bool attackedOnThisHop;
    private Vector2 attackPosition;

    [SerializeField]
    private float thowbackDuration; // how much time will the player be thown back after stike
    [SerializeField]
    private float xThrowbackForce; // throwback force on x axis
    [SerializeField]
    private float yThrowbackForce; // throwback force on y axis
    [SerializeField]
    private float timeBetweenStrikes; // time in seconds between strikes, if player is within range for a long time

    private float since_last_strike; // time in seconds since last strike
    private bool is_in_contact;

    private PlayerBasic player;
    private GameObject player_obj;

    private BossOne.BossOneBehaviour currentBehaviour;
    private bool newBehaviourRequired; // shows if the behaviour has to be changed as soon as the current action ends
    private BossOne.BossOneBehaviour newBehaviour; // the new behaviour variable

    private bool isBehaviourActionGoingOn; // shows if some behaviour-specific action is in progress
    [SerializeField]
    private float timeForEachAttack;
    private float sinceLastAttackChange;

    private bool fightStarted;
    public BossOneRoom room;
    public GameObject rewardPrefab;

    private float yRewardSpawnPoint;

    private Animator animator;

    private void Flip()
    {
        direction *= -1;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
    private void PerformHop(float direction)
    {
        Vector2 hop_direction = new Vector2(Mathf.Cos(hopAngle) * direction, Mathf.Sin(hopAngle));
        Debug.Log("performing hop with direction " + hop_direction);
        hop_direction *= hopForce;
        rb.velocity = hop_direction;
    }
    private void AttackOnTheFly(Vector2 position)
    {
        Vector2 attack_direction = new Vector2(position.x - transform.position.x,
            position.y - transform.position.y);
        attack_direction.Normalize();
        Debug.Log("performing attack with direction " + attack_direction);
        rb.velocity = attack_direction * attackForce;
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
    private void SetBehaviour(BossOne.BossOneBehaviour behaviour)
    {
        if (behaviour != currentBehaviour)
        {
            newBehaviourRequired = true;
            newBehaviour = behaviour;
        }
    }

    public override void OnDeath()
    {
        Debug.Log("BossOne got killed");
        Destroy(gameObject);
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateGhanged;
        // spawn reward
        Vector3 spawnPoint = transform.position;
        spawnPoint.y = yRewardSpawnPoint - 0.5f;
        GameObject reward = Instantiate(rewardPrefab, spawnPoint, Quaternion.identity);
        reward.SetActive(true);
        spawnPoint.y = yRewardSpawnPoint;
        room.EndFight(spawnPoint);
    }

    private void HitWithThrowback(int damage)
    {
        player.DamageFixed(damage);
        float throwback_direction = (player_obj.transform.position - transform.position).x < 0 ? -1 : 1;
        PlayerControls player_controls = player_obj.GetComponent<PlayerControls>();
        player_controls.ApplyForce(new Vector2(throwback_direction * xThrowbackForce, yThrowbackForce), thowbackDuration);
    }

    public void StartFight()
    {
        fightStarted = true;
    }
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateGhanged;
        CurrentHP = MaxHP;
        isGrounded = false;
        able_to_move = true;
        timeSinceLastHop = 0f;
        rb = GameObject.Find("RigidBody").GetComponent<Rigidbody2D>();
        direction = -1f;
        attackedOnThisHop = false;
        player_obj = GameObject.FindGameObjectWithTag("Player");
        currentBehaviour = BossOne.BossOneBehaviour.HOP_SIDEWAYS;
        isBehaviourActionGoingOn = false;
        animator = GetComponent<Animator>();
        animator.SetBool("Walk", false);
        animator.SetBool("JumpUp", false);
        animator.SetBool("JumpDown", false);
        animator.SetBool("Attack", false);
        sinceLastAttackChange = 0f;
        fightStarted = false;
        gameObject.SetActive(false);
        yRewardSpawnPoint = transform.position.y;
    }

    void Update()
    {
        if (fightStarted && able_to_move)
        {
            if (sinceLastAttackChange > timeForEachAttack)
            {
                int newBehaviourIndex = (int)Random.Range(1f, 3.9f);
                Debug.Log(newBehaviourIndex + " " + " " + (BossOne.BossOneBehaviour)newBehaviourIndex);
                SetBehaviour((BossOne.BossOneBehaviour)newBehaviourIndex);
                sinceLastAttackChange = 0f;
            }

            // trying to change behaviour if setBehaviour was called
            if (!isBehaviourActionGoingOn && newBehaviourRequired)
            {
                newBehaviourRequired = false;
                currentBehaviour = newBehaviour;
                animator.SetBool("Walk", false); // in case behaviour changed before rb.velocity = 0
                Debug.Log("new behaviour " + newBehaviour);
            }
            //true if stands on ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
            //Debug.Log(isGrounded);

            // flip if player and direction are not on the same side
            if ((player_obj.transform.position.x - transform.position.x) * direction < 0 &&
                Mathf.Abs(player_obj.transform.position.x - transform.position.x) > 0.1f)
            {
                Flip();
            }

            since_last_strike += Time.deltaTime;
            timeSinceLastHop += Time.deltaTime;
            if (!newBehaviourRequired) // means that no behaviour is in queue
            {
                sinceLastAttackChange += Time.deltaTime;
            }

            switch (currentBehaviour)
            {
                case BossOne.BossOneBehaviour.STAY_STILL:
                    break;
                case BossOne.BossOneBehaviour.WALK_SIDEWAYS:
                    animator.SetBool("Bite", false);
                    if (is_in_contact && since_last_strike >= timeBetweenStrikes)
                    {
                        animator.SetBool("Bite", true);
                        HitWithThrowback(1);
                        since_last_strike = 0;
                    }
                    if (Mathf.Abs(player_obj.transform.position.x - transform.position.x) > 0.1f)
                    {
                        rb.velocity = Vector2.right * direction * walkSpeed;
                        animator.SetBool("Walk", true);
                    } else
                    {
                        rb.velocity = Vector2.zero;
                        animator.SetBool("Walk", false);
                    }
                    break;
                case BossOne.BossOneBehaviour.HOP_SIDEWAYS:
                    //hop
                    if (isGrounded && timeSinceLastHop >= timeBetweenHops)
                    {
                        animator.SetBool("JumpUp", true);
                        isBehaviourActionGoingOn = true;
                        PerformHop(direction);
                        timeSinceLastHop = 0f;
                    }
                    if (!isGrounded && Mathf.Abs(rb.velocity.y) < 0.01f)
                    {
                        animator.SetBool("JumpUp", false);
                        animator.SetBool("JumpDown", true);
                    }
                    // check if the landing happend
                    if (isGrounded && !previouslyGrounded)
                    {

                        animator.SetBool("JumpDown", false);
                        Debug.Log("landed after hop");
                        //if (is_in_contact)
                        //{
                        //   HitWithThrowback(1);
                        //}
                        isBehaviourActionGoingOn = false;
                        timeSinceLastHop = 0f;
                    }
                    break;
                case BossOne.BossOneBehaviour.HOP_AND_ATTACK:
                    // hop
                    if (isGrounded && timeSinceLastHop >= timeBetweenHops)
                    {
                        animator.SetBool("JumpUp", true);
                        isBehaviourActionGoingOn = true;
                        attackPosition = player_obj.transform.position;
                        PerformHop(direction);
                        timeSinceLastHop = 0f;
                    }
                    //attack
                    if (!isGrounded && Mathf.Abs(rb.velocity.y) < 0.01f && !attackedOnThisHop)
                    {
                        animator.SetBool("Attack", true);
                        animator.SetBool("JumpUp", false);
                        attackedOnThisHop = true;
                        AttackOnTheFly(attackPosition);
                    }
                    // check if the landing happend
                    if (isGrounded && !previouslyGrounded)
                    {
                        animator.SetBool("Attack", false);
                        Debug.Log("landed after hop&attack");
                        if (is_in_contact)
                        {
                            HitWithThrowback(1);
                        }
                        isBehaviourActionGoingOn = false;
                        attackedOnThisHop = false;
                        timeSinceLastHop = 0f;
                    }
                    break;
            }
            previouslyGrounded = isGrounded;
        }
    }
}
