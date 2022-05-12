using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    // Components
    new Rigidbody2D rigidbody;
    Animator animator;

    // Stamina
    public Slider slider;
    internal float currentStamina;
    public float maxStamina;

    public float staminaRegen;
    public float speedRunStaminaRequirement;
    public float dashStaminaRequirement;

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
    internal bool dashing;
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
        SetMaxStaminaBar();
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
            else
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
            }

            StaminaRegen();
        }
        else
        {
            if (!isGrounded)
            {
                rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                verticalMove = rigidbody.velocity.y;
                animator.SetFloat("verticalSpeed", verticalMove);
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
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
            if (currentStamina > speedRunStaminaRequirement)
            {
                runSpeed = Mathf.Clamp(runSpeed, minSpeed, boostSpeed);
                runSpeed += boostAccelaration * Time.deltaTime;
                currentStamina -= speedRunStaminaRequirement * Time.deltaTime;
                SetStaminaBar();
            }
        }
        else
        {
            runSpeed = Mathf.Clamp(runSpeed, minSpeed, maxSpeed);
            runSpeed += accelaration * Time.deltaTime;
        }

        rigidbody.velocity = new Vector2(horizontalMove * runSpeed, rigidbody.velocity.y);
    }

    public void SetStaminaBar()
    {
        slider.value = currentStamina;
    }

    public void SetMaxStaminaBar()
    {
        currentStamina = maxStamina;
        slider.maxValue = maxStamina;
        slider.value = currentStamina;
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
            if (dashCoolDownTimer < 0 && currentStamina > dashStaminaRequirement)
            {
                dashing = true;
                dashCoolDownTimer = dashCoolDown;
                dashTimeCounter = dashTime;
                currentStamina -= dashStaminaRequirement;
                animator.SetBool("dashing", dashing);
                SetStaminaBar();
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
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            SetStaminaBar();
        }else if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
            SetMaxStaminaBar();
        }
    }
}