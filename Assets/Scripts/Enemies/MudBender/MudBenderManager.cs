using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudBenderManager : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    GameObject player;

    bool undergrounded;
    bool resetTeleUpCd;
    public float noTeleUpCooldown;
    float teleUpCooldown;
    public float startTeleUpCooldown;

    bool realized;
    public float realizingDistance;

    bool chasing;
    public float speed;
    float defaultSpeed;

    bool attacking;
    public LayerMask whatIsEnemies;

    public float startMudAttackRange;
    public float mudAttackDamage;
    public Transform mudAttackPosition;
    public float mudAttackRangeX;
    public float mudAttackRangeY;
    bool mudAttacking;
    bool mudAttackDone;
    bool gaveDamage;

    public float startSlapAttackRange;
    public float slapAttackDamage;
    public Transform slapAttackPosition;
    public float slapAttackRange;
    bool slapAttackDone;

    public float startTeleportDistance;
    float teleportDistance;
    bool canTeleport;
    public float checkGroundLenght;

    public float teleDownDistanceX;
    public float teleDownDistanceY;
    public Transform groundCheckPos;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        defaultSpeed = speed;
        undergrounded = true;
        canTeleport = true;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * checkGroundLenght, Color.red);

        if (!GetComponent<EnemyHealthSystem>().dead)
        {
            if (!undergrounded)
            {
                TeleDownCondition();
            }
            if (GetComponent<EnemyHealthSystem>().getHit)
            {
                resetTeleUpCd = true;
                undergrounded = true;
                animator.SetBool("teleDown", true);
            }
            else
            {
                RealizePlayer();
                ChasePlayer();
                FaceToPlayer();
                AttackCondition();
                MudAttack();
            }
        }

        if (undergrounded && realized)
        {
            GetComponent<EnemyHealthSystem>().canBeDamaged = false;
            if (teleUpCooldown <= 0 && player.GetComponent<PlayerMovement>().isGrounded)
            {
                TeleportToPlayer();
            }
            else
            {
                teleUpCooldown -= Time.deltaTime;
            }
        }
        else
        {
            GetComponent<EnemyHealthSystem>().canBeDamaged = true;
        }

        if (attacking)
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    public void RealizePlayer()
    {
        if (Vector2.Distance(player.transform.position, transform.position) < realizingDistance)
        {
            realized = true;
        }
    }

    public void FaceToPlayer()
    {
        if (!attacking)
        {
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

    public void EndOfTeleUp()
    {
        animator.SetBool("teleUp", false);
        animator.SetBool("running", true);
        chasing = true;
        undergrounded = false;
        mudAttackDone = false;
        slapAttackDone = false;
    }

    public void EndOfTeleDown()
    {
        chasing = false;
        undergrounded = true;
        canTeleport = true;
        gaveDamage = false;
        teleportDistance = startTeleportDistance;
        if (resetTeleUpCd)
        {
            teleUpCooldown = startTeleUpCooldown;
        }
        else
        {
            teleUpCooldown = 0.5f;
        }
        animator.SetBool("teleDown", false);
        animator.SetBool("teleUp", false);
    }

    public void TeleportToPlayer()
    {
        if (canTeleport)
        {
            teleportDistance -= 1f;
            if (teleportDistance == 0f)
            {
                teleportDistance -= 1f;
            }

            if (teleportDistance < -4f)
            {
                teleportDistance = startTeleportDistance;
            }

            rigidbody.gravityScale = 0f;
            if (player.transform.eulerAngles.y < 90)
            {
                transform.position = new Vector2(player.transform.position.x + teleportDistance, player.transform.position.y - 0.5f);
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.down), checkGroundLenght);
                foreach (var hit in hits)
                {
                    if (hit.collider.name == "Tilemap")
                    {
                        animator.SetBool("teleUp", true);
                        rigidbody.gravityScale = 3f;
                        canTeleport = false;
                    }
                }
            }
            else
            {
                transform.position = new Vector2(player.transform.position.x - teleportDistance, player.transform.position.y - 0.5f);
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.down), checkGroundLenght);
                foreach (var hit in hits)
                {
                    if (hit.collider.name == "Tilemap")
                    {
                        animator.SetBool("teleUp", true);
                        rigidbody.gravityScale = 3f;
                        canTeleport = false;
                    }
                }
            }
        }
    }

    public void ChasePlayer()
    {
        if (chasing && !attacking && !undergrounded)
        {
            if (transform.position.x < player.transform.position.x)
            {
                speed = defaultSpeed;
            }
            else
            {
                speed = -defaultSpeed;
            }

            rigidbody.velocity = new Vector2(speed * Time.deltaTime, rigidbody.velocity.y);
        }
    }

    public void TeleDownCondition()
    {
        if (Mathf.Abs(transform.position.x - player.transform.position.x) > teleDownDistanceX || Mathf.Abs(transform.position.y - player.transform.position.y) > teleDownDistanceY)
        {
            resetTeleUpCd = false;
            undergrounded = true;
            animator.SetBool("teleDown", true);
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundCheckPos.position, Vector2.down, checkGroundLenght);
        if (!groundInfo)
        {
            resetTeleUpCd = false;
            undergrounded = true;
            animator.SetBool("teleDown", true);
        }
    }

    public void AttackCondition()
    {
        if (realized && !attacking && !undergrounded)
        {
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < startMudAttackRange && Mathf.Abs(transform.position.y - player.transform.position.y) < 1f && !mudAttackDone)
            {
                chasing = false;
                attacking = true;
                animator.SetBool("mudAttack", true);
                mudAttackDone = true;
            }else if (Mathf.Abs(transform.position.x - player.transform.position.x) < startSlapAttackRange && Mathf.Abs(transform.position.y - player.transform.position.y) < 1f && !slapAttackDone)
            {
                resetTeleUpCd = true;
                chasing = false;
                attacking = true;
                animator.SetBool("slapAttack", true);
                slapAttackDone = true;
            }
        }
    }

    public void StartMudAttack()
    {
        mudAttacking = true;
    }

    public void EndMudAttack()
    {
        chasing = true;
        attacking = false;
        mudAttacking = false;
        animator.SetBool("mudAttack", false);
        gaveDamage = false;
    }

    public void MudAttack()
    {
        if (mudAttacking && !gaveDamage)
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(mudAttackPosition.position, new Vector2(mudAttackRangeX, mudAttackRangeY), whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].GetComponent<PlayerHealthSystem>() != null)
                {
                    enemiesToDamage[i].GetComponent<PlayerHealthSystem>().GetDamage(mudAttackDamage);
                    gaveDamage = true;
                }
            }
        }
    }

    public void SlapAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(slapAttackPosition.position, slapAttackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<PlayerHealthSystem>() != null)
            {
                enemiesToDamage[i].GetComponent<PlayerHealthSystem>().GetDamage(slapAttackDamage);
            }
        }
    }

    public void EndOfSlapAttack()
    {
        attacking = false;
        animator.SetBool("slapAttack", false);
        animator.SetBool("teleDown", true);
        undergrounded = true;
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(slapAttackPosition.position, slapAttackRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mudAttackPosition.position, new Vector2(mudAttackRangeX, mudAttackRangeY));
    }
}