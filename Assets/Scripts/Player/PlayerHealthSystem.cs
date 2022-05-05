using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    Animator animator;

    public float maxHealth;
    internal float health;

    public static bool dead;
    bool playedDeathAnimation;

    internal static bool hit;
    public float delayAfterHit;
    float delayAfterHitTimer;

    public Slider slider;

    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        SetMaxHealthBar();
    }

    void Update()
    {
        Death();

        if (hit)
        {
            if (delayAfterHitTimer <= 0)
            {
                hit = false;
                animator.SetBool("hit", hit);
            }
            else
            {
                delayAfterHitTimer -= Time.deltaTime;
            }
        }
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        hit = true;
        delayAfterHitTimer = delayAfterHit;
        animator.SetBool("hit", hit);
        animator.SetTrigger("getHit");

        if (health <= 0)
        {
            dead = true; 
        }

        SetHealthBar();
    }

    public void GetHeal(float heal)
    {
        health += heal;

        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void SetHealthBar()
    {
        slider.value = health;
    }

    public void SetMaxHealthBar()
    {
        health = maxHealth;
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void Death()
    {
        if (health<=0)
        {
            dead = true;
        }

        if (!playedDeathAnimation && dead && player.GetComponent<PlayerMovement>().isGrounded)
        {
            animator.SetBool("dead", true);
            animator.SetTrigger("death");
            playedDeathAnimation = true;
        }
    }
}