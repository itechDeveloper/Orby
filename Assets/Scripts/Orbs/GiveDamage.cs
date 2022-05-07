using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (PlayerSkillSystem.rageActive)
            {
                damage += 5;
            }
            collision.GetComponent<EnemyHealthSystem>().GetDamage(damage);
            if (collision.GetComponent<EnemyHealthSystem>().health > 0)
            {
                collision.GetComponent<EnemyHealthSystem>().getHit = true;
                collision.GetComponent<Animator>().SetBool("getHit", true);
                collision.GetComponent<Animator>().SetTrigger("getHitTrigger");
                collision.GetComponent<EnemyHealthSystem>().getHitAnimationTimeCounter = collision.GetComponent<EnemyHealthSystem>().getHitAnimationTime;
            }
        }
    }
}