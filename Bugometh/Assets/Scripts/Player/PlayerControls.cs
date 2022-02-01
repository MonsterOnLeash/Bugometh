using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public float speed;
    private PlayerInput playerInput;
    public float input;
    public float jump;
    public float shoot;
    public float jumpTime;
    public float jumpTimeCounter;
    public bool isJumping;
    public float fallMultiplier = 1.5f;
    public float lowJumpMultiplier = 1;

    public float jumpForce;
    public bool isGrounded;
    private bool facingRight;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public int extraJumps;
    public Transform attackPoint;
    public float attackRange;
    public int attackPower;
    public float coyoteTime;
    public float coyoteTimeCounter;

    public Rigidbody2D rb;
    public BoxCollider2D bc;

    public Animator animator;
    public Animator attackAnimator;
    public LayerMask enemyLayers;

    private bool is_force_applied;
    private Vector2 force_applied;
    private float force_duration;

    private void Awake()
    {
        facingRight = true;
        isJumping = false;
        jumpForce = 5.3f;
        speed = 4;
        attackRange = 0.5f;
        jumpTime = 0.37f;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        bc = GetComponent<BoxCollider2D>();
        jumpTimeCounter = jumpTime;
        coyoteTimeCounter = coyoteTime;
        checkRadius = 0.15f;
        coyoteTime = 0.1f;
        force_duration = 0;
        is_force_applied = false;
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<float>();
        jump = playerInput.actions["Jump"].ReadValue<float>();

        if (input != 0 && isGrounded)
        {
            animator.SetFloat("Speed", 2);
            animator.SetBool("Jump", false);
        }
        else if (!isGrounded)
            animator.SetBool("Jump", true);
        else
        {
            animator.SetBool("Jump", false);
            animator.SetFloat("Speed", 0);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        //rb.velocity = new Vector2(input * speed, rb.velocity.y);
        //transform.Translate(new Vector2(input * speed * Time.deltaTime, 0));

        if (playerInput.actions["Shoot"].triggered)
            Attack();

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (playerInput.actions["Jump"].triggered && coyoteTimeCounter > 0)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;
        }

        if (jump == 1 && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (playerInput.actions["JumpRelease"].triggered)
        {
            coyoteTimeCounter = 0;
            isJumping = false;
        }

        float x_speed = input * speed;
        float y_speed = rb.velocity.y;
        if (is_force_applied)
        {
            x_speed += force_applied.x;
            y_speed += force_applied.y;
        }
        rb.velocity = new Vector2(x_speed, y_speed);

        force_duration -= Time.deltaTime;
        if (force_duration <= 0)
        {
            is_force_applied = false; // from now force is no longer applied
        }

        if ((!facingRight && input > 0) || (facingRight && input < 0))
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    void Attack()
    {
        attackAnimator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); 
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().DamageFixed(attackPower);
        }
    }

    // applies force to player for some time
    public void ApplyForce(Vector2 force, float duration) 
    {
        force_duration = duration;
        force_applied = force;
        is_force_applied = true;
    }
}
