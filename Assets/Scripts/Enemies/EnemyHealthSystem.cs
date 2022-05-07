using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public float health;

    Animator animator;
    public float getHitAnimationTime;
    internal float getHitAnimationTimeCounter;
    internal bool getHit;

    internal bool dead;
    bool playedDeathAnimation;

    void Start()
    {
        init();
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
        health -= damage;
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
