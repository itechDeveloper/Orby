using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonerManager : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    GameObject player;
    EnemyHealthSystem enemyHealthSystem;
    PatrolMovement patrolMovement;

    public float speed;
    public float runDistance;
    public float chaseDistance;
    bool shouldRun;
    bool noWayRight;
    bool noWayLeft;
    public float startRunningCooldown;
    float runningCooldown;
    public Transform groundCheckPos;
    public float checkGroundLenght;
    public LayerMask whatIsGround;

    public float startSummonCooldown;
    float summonCooldown;
    public GameObject summoner;
    public Transform firstSummonPos;
    public Transform secondSummonPos;
    bool summoning;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealthSystem = GetComponent<EnemyHealthSystem>();
        patrolMovement = GetComponent<PatrolMovement>();
    }

    void Update()
    {
        if (!enemyHealthSystem.dead)
        {
            if (patrolMovement.realized)
            {
                SummonCondition();
                RunFromPlayer();
            }
        }
    }

    public void RunFromPlayer()
    {
        if (!summoning)
        {
            if (player.GetComponent<PlayerMovement>().isGrounded)
            {
                if (Mathf.Abs(transform.position.x - player.transform.position.x) < runDistance && Mathf.Abs(transform.position.y - player.transform.position.y) < 1f)
                {
                    shouldRun = true;
                }
                else
                {
                    shouldRun = false;
                    animator.SetBool("run", false);
                    rigidbody.velocity = Vector2.zero;
                }
            }

            Collider2D groundInfo = Physics2D.OverlapCircle(groundCheckPos.position, checkGroundLenght, whatIsGround);

            if (shouldRun)
            {
                if (!noWayRight && !noWayLeft)
                {
                    if (transform.position.x < player.transform.position.x)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        rigidbody.velocity = Vector2.left * speed * Time.deltaTime;
                        animator.SetBool("run", true);
                        if (!groundInfo)
                        {
                            noWayLeft = true;
                            runningCooldown = startRunningCooldown;
                        }
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        rigidbody.velocity = Vector2.right * speed * Time.deltaTime;
                        animator.SetBool("run", true);
                        if (!groundInfo)
                        {
                            noWayRight = true;
                            runningCooldown = startRunningCooldown;
                        }
                    }
                }
                else if (noWayRight)
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    rigidbody.velocity = Vector2.left * speed * Time.deltaTime;
                    animator.SetBool("run", true);

                    if (player.transform.position.x < transform.position.x && runningCooldown <= 0)
                    {
                        noWayRight = false;
                    }
                    else if (runningCooldown > 0)
                    {
                        runningCooldown -= Time.deltaTime;
                    }
                }
                else if (noWayLeft)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    rigidbody.velocity = Vector2.right * speed * Time.deltaTime;
                    animator.SetBool("run", true);

                    if (player.transform.position.x > transform.position.x && runningCooldown <= 0)
                    {
                        noWayLeft = false;
                    }
                    else if (runningCooldown > 0)
                    {
                        runningCooldown -= Time.deltaTime;
                    }
                }
            }
            else if (Mathf.Abs(transform.position.x - player.transform.position.x) > chaseDistance && Mathf.Abs(transform.position.y - player.transform.position.y) < 1f && groundInfo)
            {
                if (transform.position.x < player.transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    rigidbody.velocity = Vector2.right * speed * Time.deltaTime;
                    animator.SetBool("run", true);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                    rigidbody.velocity = Vector2.left * speed * Time.deltaTime;
                    animator.SetBool("run", true);
                }
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
                animator.SetBool("run", false);
                if (transform.position.x < player.transform.position.x)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
            }
        }
    }

    public void SummonCondition()
    {
        if (!enemyHealthSystem.getHit)
        {
            if (summonCooldown <= 0)
            {
                Summon();
            }
            else
            {
                summonCooldown -= Time.deltaTime;
            }
        }
    }

    public void Summon()
    {
        animator.SetBool("summon", true);
        animator.SetBool("run", false);
        summoning = true;
    }

    public void SummonEnd()
    {
        animator.SetBool("summon", false);
        Instantiate(summoner, firstSummonPos.position, Quaternion.identity);
        Instantiate(summoner, secondSummonPos.position, Quaternion.identity);
        summonCooldown = startSummonCooldown;
        summoning = false;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPos.position, checkGroundLenght);
    }
}
