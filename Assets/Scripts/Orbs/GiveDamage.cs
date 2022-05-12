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
            if (GetComponent<MoveToCurser>().canGiveDamage)
            {
                if (PlayerSkillSystem.rageActive)
                {
                    collision.GetComponent<EnemyHealthSystem>().GetDamage(damage * 7/5);
                }
                else
                {
                    collision.GetComponent<EnemyHealthSystem>().GetDamage(damage);
                }
            }
        }
    }
}