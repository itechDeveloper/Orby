using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolMovement : MonoBehaviour
{
    Animator animator;
    GameObject player;
    new Rigidbody2D rigidbody;

    public float realizeDistance;
    internal bool realized;
    public bool canPatrol;

    internal bool patroling;
    public float patrolSpeed;
    float tempSpeed;
    public Transform groundCheckPos;
    public float groundCheckDistance;
    public LayerMask whatIsGround;
    private bool movingRight;

    // Randomize patrol movement
    public float startPatrolTimer;
    float patrolTimer;
    public float startPatrolCooldown;
    float patrolCooldown;
    bool makePatrolMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        movingRight = true;
        makePatrolMovement = true;
        tempSpeed = patrolSpeed;
    }

    void Update()
    {
        RealizePlayer();
        MakePatrolMovement();
    }

    void RealizePlayer()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < realizeDistance)
        {
            realized = true;
        }else if (canPatrol)
        {
            realized = false;
        }
    }

    void MakePatrolMovement()
    {
        if (canPatrol && !realized)
        {
            Collider2D groundInfo = Physics2D.OverlapCircle(groundCheckPos.position, groundCheckDistance, whatIsGround);

            if (patrolTimer > 0)
            {
                makePatrolMovement = true;
                patrolTimer -= Time.deltaTime;
            }
            else
            {
                if (patrolCooldown > 0)
                {
                    makePatrolMovement = false;
                    patrolCooldown -= Time.deltaTime;
                }
                else
                {
                    movingRight = RandomBoolean();
                    patrolTimer = startPatrolTimer;
                    patrolCooldown = startPatrolCooldown;
                }
            }

            if (!groundInfo)
            {
                if (movingRight)
                {
                    movingRight = false;
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    tempSpeed = -patrolSpeed;
                }
                else
                {
                    movingRight = true;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    tempSpeed = patrolSpeed;
                }
            }

            if (makePatrolMovement)
            {
                animator.SetBool("run", true);
                rigidbody.velocity = Vector2.right * tempSpeed * Time.deltaTime;
            }
            else
            {
                animator.SetBool("run", false);
            }
        }
    }

    bool RandomBoolean()
    {
        if (Random.value >= 0.5)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            tempSpeed = patrolSpeed;
            return true;
        }

        transform.eulerAngles = new Vector3(0, 180, 0);
        tempSpeed = -patrolSpeed;
        return false;
    }
}
