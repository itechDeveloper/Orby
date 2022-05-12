using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagmaShot : MonoBehaviour
{
    public LayerMask whatIsEnemies;
    public float damage;
    public Transform magmaShotAttackPosition;
    public float magmaShotAttackRangeX;
    public float magmaShotAttackRangeY;

    public void GiveDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(magmaShotAttackPosition.position, new Vector2(magmaShotAttackRangeX, magmaShotAttackRangeY), whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(damage);
        }
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(magmaShotAttackPosition.position, new Vector2(magmaShotAttackRangeX, magmaShotAttackRangeY));
    }

}
