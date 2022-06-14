using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    public float areaDamage;

    public float startDelayToDamage;
    float delayToDamage;

    public float areaDamageRadius;
    public LayerMask whatIsEnemies;

    internal bool canGiveDamage;

    void Update()
    {
        if (delayToDamage < 0)
        {
            GiveDamage();
        }
        else
        {
            delayToDamage -= Time.deltaTime;
        }
    }

    public void GiveDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(transform.position, areaDamageRadius, whatIsEnemies);
     
        if (canGiveDamage)
        {
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
                {
                    if (PlayerSkillSystem.rageActive)
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(areaDamage * 7 / 5);
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(areaDamage);
                    }
                }
            }

            delayToDamage = startDelayToDamage;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, areaDamageRadius);
    }
}
