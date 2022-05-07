using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    new Rigidbody2D rigidbody;
    Animator animator;

    // Stats
    public float stamina;
    private float staminaRequirement;

    // Horizontal movement
    float horizontalMove;
    public float runSpeed;
    public float accelaration;
    public float minSpeed;
    public float maxSpeed;
    public float boostSpeed;
    public float boostAccelaration;

    // Jump
    private float verticalMove;
    public float jumpForce;

    public Transform groundCheck;
    internal bool isGrounded;
    public float checkRadius;
    public LayerMask whatIsGround;

    // Double Jump
    public int extraJumpsValue;
    private int extraJumps;

    // Dash
    bool dashing;
    public float dashSpeed;
    public float dashTime;
    private float dashTimeCounter;
    public float dashCoolDown;
    private float dashCoolDownTimer;

    private void init()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Awake()
    {
        init();
        extraJumps = extraJumpsValue;
        dashTimeCounter = dashTime;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (!PlayerHealthSystem.dead)
        {
            if (!PlayerHealthSystem.hit && !PlayerSkillSystem.usingSkill)
            {
                if (!dashing)
                {
                    Move();
                    SetAnimationsParams();
                    Flip();
                }
                Jump();
                Dash();
            }

            StaminaRegen();
        }
        else
        {
            if (!isGrounded)
            {
                verticalMove = rigidbody.velocity.y;
                animator.SetFloat("verticalSpeed", verticalMove);
            }
        }
    }

    void Move()
    {
        if (horizontalMove == 0)
        {
            runSpeed = 3;
        }
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.C))
        {
            staminaRequirement = 5;
            if (stamina > staminaRequirement)
            {
                runSpeed = Mathf.Clamp(runSpeed, minSpeed, boostSpeed);
                runSpeed += boostAccelaration * Time.deltaTime;
                stamina -= 5 * Time.deltaTime;
            }
        }
        else
        {
            runSpeed = Mathf.Clamp(runSpeed, minSpeed, maxSpeed);
            runSpeed += accelaration * Time.deltaTime;
        }

        rigidbody.velocity = new Vector2(horizontalMove * runSpeed, rigidbody.velocity.y);
    }

    void SetAnimationsParams()
    {
        // Running

        if (horizontalMove == 0)
        {
            animator.SetBool("running", false);
        }
        else
        {
            animator.SetBool("running", true);
        }

        // Jumping

        verticalMove = rigidbody.velocity.y;
        animator.SetFloat("verticalSpeed", verticalMove);
    }

    void Flip()
    {
        if (horizontalMove < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (horizontalMove > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            animator.SetBool("jumping", false);
            extraJumps = extraJumpsValue;
        }
        else
        {
            animator.SetBool("jumping", true);
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
            extraJumps--;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }
    }

    public void Dash()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            staminaRequirement = 33f;

            if (dashCoolDownTimer < 0 && stamina > staminaRequirement)
            {
                dashing = true;
                dashCoolDownTimer = dashCoolDown;
                dashTimeCounter = dashTime;
                stamina -= staminaRequirement;
                animator.SetBool("dashing", dashing);
            }
        }
        else
        {
            dashCoolDownTimer -= Time.deltaTime;
        }

        if (dashing)
        {
            if (dashTimeCounter > 0)
            {
                dashTimeCounter -= Time.deltaTime;
                if (transform.eulerAngles == new Vector3(0, 180, 0))
                {
                    rigidbody.velocity = Vector2.left * dashSpeed;
                }
                else
                {
                    rigidbody.velocity = Vector2.right * dashSpeed;
                }
            }
            else
            {
                dashing = false;
                animator.SetBool("dashing", dashing);
            }
        }
    }

    public void StaminaRegen()
    {
        if (stamina < 100)
        {
            stamina += 10 * Time.deltaTime;
        }else if (stamina > 100)
        {
            stamina = 100;
        }
    }
}