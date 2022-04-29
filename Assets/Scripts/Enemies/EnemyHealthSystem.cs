using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public float health;

    Animator animator;
    public float getHitAnimationTime;
    private float getHitAnimationTimeCounter;
    bool getHit;

    void Start()
    {
        init();
    }

    void Update()
    {
        Animate();
    }

    void init()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Orb")
        {
            health -= 10;
            getHit = true;
            animator.SetBool("getHit", getHit);
            getHitAnimationTimeCounter = getHitAnimationTime;
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
}
