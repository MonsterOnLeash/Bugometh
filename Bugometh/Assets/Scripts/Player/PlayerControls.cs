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
    
    private List<float> jumpForceList = new List<float> {0, 0, 0};
    private List<float> speedList = new List<float> { 0, 0, 0 };
    private List<float> attackRangeList = new List<float> { 0, 0, 0 };
    private List<float> jumpTimeList = new List<float> { 0, 0, 0 };
    private List<float> checkRadiusList = new List<float> { 0, 0, 0 };
    public List<int> attackPowerList = new List<int> { 0, 0, 0 };

    private List<float> jumpForceDefault = new List<float> { 5.3f, 5.3f, 5.3f };
    private List<float> speedDefault = new List<float> { 4, 3, 5 };
    private List<float> attackRangeDefault = new List<float> { 0.5f, 0.5f, 0 };
    private List<float> jumpTimeDefault = new List<float> { 0.35f, 0.35f, 0.35f };
    private List<float> checkRadiusDefault = new List<float> { 0.15f, 0.15f, 0.15f };
    private List<int> attackPowerDefault = new List<int> { 2, 4, 0 };

    public float jumpForce;
    public bool isGrounded;
    private bool facingRight;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public int extraJumps;
    public int availableJumps;
    public Transform attackPoint;
    public float attackRange;
    public int attackPower;
    public float coyoteTime;
    public float coyoteTimeCounter;

    public int colorIndex;
    public int shotEnabled;

    public Rigidbody2D rb;
    public BoxCollider2D bc;

    public Animator animator;
    public Animator attackAnimator;
    public LayerMask enemyLayers;

    private bool is_force_applied;
    private Vector2 force_applied;
    private float force_duration;
    private float actionRequired;
    private float switchColorRequired;
    public List<bool> unlockedColors;

    public CameraManager cm;

    public bool is_gameplay; // true if gamestate == gameblay

    [SerializeField]
    GameObject blueShot;
    [SerializeField]
    GameObject greenShot;

    private void Awake()
    {
        colorIndex = 0;
        jumpForceList = new List<float> { 0, 0, 0 };
        speedList = new List<float> { 0, 0, 0 };
        attackRangeList = new List<float> { 0, 0, 0 };
        jumpTimeList = new List<float> { 0, 0, 0 };
        checkRadiusList = new List<float> { 0, 0, 0 };
        unlockedColors = new List<bool> { false, false, false };
        actionRequired = 0;
        facingRight = true;
        isJumping = false;
        LoadSettings();
        availableJumps = extraJumps + 1;
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        bc = GetComponent<BoxCollider2D>();
        jumpTimeCounter = jumpTime;
        coyoteTimeCounter = coyoteTime;
        coyoteTime = 0.05f;
        force_duration = 0;
        is_force_applied = false;
        is_gameplay = true;
    }

    // Update is called once per frame
    void Update()
    {
        //change values according to color used
        jumpForce = jumpForceList[colorIndex];
        speed = speedList[colorIndex];
        attackRange = attackRangeList[colorIndex];
        jumpTime = jumpTimeList[colorIndex];
        checkRadius = checkRadiusList[colorIndex];
        attackPower = attackPowerList[colorIndex];

        if (is_gameplay)
        {
            //choosing direction
            if (playerInput.actions["Left"].ReadValue<float>() > 0)
            {
                input = -1;
            }
            else if (playerInput.actions["Right"].ReadValue<float>() > 0)
            {
                input = 1;
            } else
            {
                input = 0;
            }

            //read from input
            jump = playerInput.actions["Jump"].ReadValue<float>();
            actionRequired = playerInput.actions["Action"].ReadValue<float>();

            //change colors
            if (playerInput.actions["Switch"].triggered)
            {
                SwitchColor();
            }

            //player animation switch
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

            //true if stands on ground
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            //trigger for attack
            if (playerInput.actions["Shoot"].triggered)
                Attack();

            //coyote time ana extra jumps management
            if (isGrounded)
            {
                coyoteTimeCounter = coyoteTime;
                availableJumps = extraJumps + 1;
            }
            else
                coyoteTimeCounter -= Time.deltaTime;

            if (playerInput.actions["Jump"].triggered && jump == 1)
                availableJumps -= 1;

            //jump mechanic
            Debug.Log(availableJumps);
            if (playerInput.actions["Jump"].triggered && jump == 1 && (coyoteTimeCounter > 0 || availableJumps > 0))
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

            if (playerInput.actions["Jump"].triggered && jump == 0)
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
        switch (colorIndex)
        {
            case 0:
                BlueAttack();
                break;
            case 1:
                RedAttack();
                break;
            case 2:
                GreenAttack();
                break;
        }
    }

    // applies force to player for some time
    public void ApplyForce(Vector2 force, float duration) 
    {
        force_duration = duration;
        force_applied = force;
        is_force_applied = true;
    }

    public void LoadSettings()
    {
        List<string> colors = new List<string> { "Blue", "Red", "Green" };
        for (int i = 0; i < 3; i++)
        {
            jumpForceList[i] = PlayerPrefs.GetFloat("jumpForce" + colors[i], jumpForceDefault[i]);
            speedList[i] = PlayerPrefs.GetFloat("speed" + colors[i], speedDefault[i]);
            attackRangeList[i] = PlayerPrefs.GetFloat("attack" + colors[i], attackRangeDefault[i]);
            jumpTimeList[i] = PlayerPrefs.GetFloat("jumpTime" + colors[i], jumpTimeDefault[i]);
            checkRadiusList[i] = PlayerPrefs.GetFloat("checkRadius" + colors[i], checkRadiusDefault[i]);
            attackPowerList[i] = PlayerPrefs.GetInt("attackPower" + colors[i], attackPowerDefault[i]);
            transform.position = new Vector2(PlayerPrefs.GetFloat("x", 0), PlayerPrefs.GetFloat("y", 0));
            unlockedColors[i] = PlayerPrefs.GetInt("unlocked" + colors[i], 0) != 0;
            cm.LoadActiveCamera();
            
        }
        unlockedColors[0] = true;
        extraJumps = PlayerPrefs.GetInt("extraJumps", 0);
        shotEnabled = PlayerPrefs.GetInt("shotEnabled", 0);
    }

    public void SaveSettings()
    {
        List<string> colors = new List<string>{"Blue", "Red", "Green"};
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetFloat("jumpForce" + colors[i], jumpForceList[i]);
            PlayerPrefs.SetFloat("speed" + colors[i], speedList[i]);
            PlayerPrefs.SetFloat("attackRange" + colors[i], attackRangeList[i]);
            PlayerPrefs.SetFloat("jumpTime" + colors[i], jumpTimeList[i]);
            PlayerPrefs.SetFloat("checkRadius" + colors[i], checkRadiusList[i]);
            PlayerPrefs.SetInt("attackPower" + colors[i], attackPowerList[i]);
            if (unlockedColors[i] == false)
                PlayerPrefs.SetInt("unlocked" + colors[i], 0);
            else
                PlayerPrefs.SetInt("unlocked" + colors[i], 1);
        }
        PlayerPrefs.SetInt("shotEnabled", shotEnabled);
        PlayerPrefs.SetInt("extraJumps", extraJumps);
    }

    public bool ActionRequired()
    {
        return actionRequired == 1;
    }

    public void SwitchColor()
    {
        colorIndex = (colorIndex + 1) % 3;
        while (!unlockedColors[colorIndex])
        {
            colorIndex = (colorIndex + 1) % 3;
        }
        animator.SetInteger("Color", colorIndex);
    }

    void BlueAttack()
    {
        if (shotEnabled == 0)

        {
            attackAnimator.SetTrigger("Attack");

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.gameObject.GetComponent<Enemy>().DamageFixed(attackPower);
            }
        }
        else if (BlueShot.numberOfShots < 5)
        {
            GameObject shot = Instantiate(blueShot, attackPoint.position, attackPoint.rotation);
            int xAxis = 1;
            if (attackPoint.position.x < transform.position.x)
                xAxis = -1;
            shot.gameObject.GetComponent<BlueShot>().SetProps(xAxis, 0, attackPower);
        }
    }

    void RedAttack()
    {
        attackAnimator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.gameObject.GetComponent<Enemy>().DamageFixed(attackPower, true);
        }
    }

    void GreenAttack()
    {
        Debug.Log(GreenShot.numberOfShots);
        if (GreenShot.numberOfShots == 0)
        {
            GameObject shot = Instantiate(greenShot, attackPoint.position, attackPoint.rotation);
            int xAxis = 1;
            if (attackPoint.position.x < transform.position.x)
                xAxis = -1;
            shot.gameObject.GetComponent<GreenShot>().SetProps(xAxis, 0);

        }
    }
}
