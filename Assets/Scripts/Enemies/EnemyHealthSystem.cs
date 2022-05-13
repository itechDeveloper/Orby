using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public float health;

    Animator animator;
    public GameObject damageProjectile;
    public float getHitAnimationTime;
    internal float getHitAnimationTimeCounter;
    internal bool getHit;

    internal bool dead;
    bool playedDeathAnimation;

    internal bool canBeDamaged;

    void Start()
    {
        init();
        canBeDamaged = true;
    }

    void Update()
    {
        if (!dead)
        {
            Animate();
        }

        Death();
    }

    void init()
    {
        animator = GetComponent<Animator>();
    }

    public void GetDamage(float damage)
    {
        if (!getHit && !dead && canBeDamaged)
        {
            health -= damage;
            Instantiate(damageProjectile, transform.position, Quaternion.identity);
            if (health > 0)
            {
                getHit = true;
                animator.SetBool("getHit", true);
                animator.SetTrigger("getHitTrigger");
                getHitAnimationTimeCounter = getHitAnimationTime;
            }
        }
    }

    void Animate()
    {
        if (getHitAnimationTimeCounter > 0)
        {
            getHitAnimationTimeCounter -= Time.deltaTime;
        }
        else if (getHit)
        {
            getHit = false;
            animator.SetBool("getHit", getHit);
        }
    }

    void Death()
    {
        if (health <= 0)
        {
            dead = true;
            if (!playedDeathAnimation)
            {
                playedDeathAnimation = true;
                animator.SetTrigger("death");
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
