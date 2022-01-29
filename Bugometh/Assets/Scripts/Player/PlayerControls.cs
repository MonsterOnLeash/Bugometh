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

    public Rigidbody2D rb;
    public BoxCollider2D bc;

    public Animator animator;
    public Animator attackAnimator;
    public LayerMask enemyLayers;

    private void Awake()
    {
        facingRight = true;
        jumpForce = 420;
        speed = 4;
        attackRange = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<float>();

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
        transform.Translate(new Vector2(input * speed * Time.deltaTime, 0));

        if (playerInput.actions["Shoot"].triggered)
        {
            Attack();
        }

        if (playerInput.actions["Jump"].triggered && isGrounded)
        {
            Debug.Log("Jump");
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
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
}
