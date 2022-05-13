using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    GameObject player;

    public float speed;
    public float lifeTime;
    public float chasingDistance;
    public float attackDistance;
    bool chase;

    bool attacking;
    bool attacked;
    public float damage;
    public Transform attackPos;
    public float attackRange;
    public LayerMask whatIsPlayer;

    public GameObject destroyingProjectile;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (lifeTime > 0)
        {
            lifeTime -= Time.deltaTime;
            if (!attacking)
            {
                Move();
            }
            AttackCondition();
        }
        else
        {
            animator.SetBool("onDestroy", true);
            animator.SetTrigger("destroy");
        }
    }

    void Move()
    {
        if (Mathf.Abs(transform.position.y - player.transform.position.y) < 1f && Mathf.Abs(transform.position.x - player.transform.position.x) < chasingDistance)
        {
            chase = true;
        }
        else if (player.GetComponent<PlayerMovement>().isGrounded)
        {
            chase = false;
        }

        if (chase)
        {
            if (transform.position.x < player.transform.position.x)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y) * Time.deltaTime;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y) * Time.deltaTime;
            }
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    void AttackCondition()
    {
        if (Mathf.Abs(transform.position.y - player.transform.position.y) < 1f && Mathf.Abs(transform.position.x - player.transform.position.x) < attackDistance && !attacked)
        {
            attacking = true;
            animator.SetTrigger("attack");
            attacked = true;
        }
    }

    public void Attack()
    {
        Collider2D playerToDamage = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);
        if (playerToDamage != null)
        {
            playerToDamage.GetComponent<PlayerHealthSystem>().GetDamage(damage);
        }
    }

    public void DestroyThis()
    {
        Instantiate(destroyingProjectile, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
