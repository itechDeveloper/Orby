using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillSystem : MonoBehaviour
{
    Animator animator;

    // Mana Bar
    public float maxMana;
    private float currentMana;
    public Slider slider;

    // Skills General
    public bool water;
    public bool fire;
    public static bool usingSkill;

    public float quickAttackDamage;
    public float waveSlameAttackDamage;
    public float spearAttackDamage;
    public float furyFistAttackDamage;

    public LayerMask whatIsEnemies;

    // Water Skills
    public float healthRegen;
    private float timeBtwHealthRegen;
    public float startTimeBtwHealthRegen;
    public float healManaRequirement;

    public Transform quickAttackPosition;
    private float timeBtwQuickAttack;
    public float startTimeBtwQuickAttack;
    public float quickAttackRange;
    public float quickAttackManaRequirement;

    public Transform waveSlameAttackPosition;
    private float timeBtwWaveSlameAttack;
    public float startTimeBtwwaveSlameAttack;
    public float waveSlameAttackRange;
    public float waveSlameManaRequirement;

    public Transform spearAttackPosition;
    private float timeBtwSpearAttack;
    public float startTimeBtwSpearAttack;
    public float spearAttackRangeX;
    public float spearAttackRangeY;
    public float spearAttackManaRequirement;

    // Fire Skills
    public static bool rageActive;
    private float timeBtwRage;
    public float startTimeBtwRage;
    private float rageTime;
    public float startRageTime;
    public float rageManaRequirement;

    internal static bool fireDashing;
    private float timeBtwFireDash;
    public Transform fireDashPosition;
    public float fireDashDamage;
    public float fireDashDamageRadius;
    public float startTimeBtwFireDash;
    public float fireDashSpeed;
    public float fireDashManaRequirement;

    public Transform furyFistAttackPosition;
    private float timeBtwFuryFistAttack;
    public float startTimeBtwFuryFistAttack;
    public float furyFistAttackRangeX;
    public float furyFistAttackRangeY;
    public float furyFistManaRequirement;

    public Transform magmaShotAttackPosition;
    private float timeBtwMagmaShotAttack;
    public float startTimeBtwMagmaShotAttack;
    public GameObject magmaShot;
    public float magmaShotManaRequirement;

    void Start()
    {
        animator = GetComponent<Animator>();
        SetMaxManaBar();
    }

    void Update()
    {
        if (GetComponent<PlayerMovement>().isGrounded && !PlayerHealthSystem.hit)
        {
            if (Input.GetKey(KeyCode.Alpha1) && !usingSkill)
            {
                if (water)
                {
                    if (timeBtwHealthRegen <= 0 && currentMana >= healManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("heal");
                        timeBtwHealthRegen = startTimeBtwHealthRegen;
                        GetComponent<PlayerHealthSystem>().GetHeal(healthRegen);
                        SetMana(healManaRequirement);
                    }
                }
                else if (fire)
                {
                    if (timeBtwRage <= 0 && currentMana >= rageManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("rage");
                        rageTime = startRageTime;
                        rageActive = true;
                        SetMana(rageManaRequirement);
                    }
                }
            }

            if (Input.GetKey(KeyCode.Alpha2) && !usingSkill)
            {
                if (water)
                {
                    if (timeBtwQuickAttack <= 0 && currentMana >= quickAttackManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("quickAttack");
                        timeBtwQuickAttack = startTimeBtwQuickAttack;
                        SetMana(quickAttackManaRequirement);
                    }                    
                }else if (fire)
                {
                    if (timeBtwFireDash <= 0 && currentMana >= fireDashManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("fireDash");
                        fireDashing = true;
                        SetMana(fireDashManaRequirement);
                    }
                }
            }

            if (Input.GetKey(KeyCode.Alpha3) && !usingSkill)
            {
                if (water)
                {
                    if (timeBtwWaveSlameAttack <= 0 && currentMana >= waveSlameManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("waveSlame");
                        timeBtwWaveSlameAttack = startTimeBtwwaveSlameAttack;
                        SetMana(waveSlameManaRequirement);
                    }
                }
                else if (fire)
                {
                    if (timeBtwFuryFistAttack <= 0 && currentMana >= furyFistManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("furyFist");
                        timeBtwFuryFistAttack = startTimeBtwFuryFistAttack;
                        SetMana(furyFistManaRequirement);
                    }
                    
                }
            }

            if (Input.GetKey(KeyCode.Alpha4) && !usingSkill)
            {
                if (water)
                {
                    if (timeBtwSpearAttack <= 0 && currentMana >= spearAttackManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("spearAttack");
                        timeBtwSpearAttack = startTimeBtwSpearAttack;
                        SetMana(spearAttackManaRequirement);
                    }
                }
                else if (fire)
                {
                    if (timeBtwMagmaShotAttack <= 0 && currentMana >= magmaShotManaRequirement)
                    {
                        usingSkill = true;
                        animator.SetBool("usingSkill", usingSkill);
                        animator.SetTrigger("magmaShot");
                        timeBtwMagmaShotAttack = startTimeBtwMagmaShotAttack;
                        SetMana(magmaShotManaRequirement);
                    }
                }
            }
        }

        FireDash();
        CoolDowns();
    }

    public void SetMana(float manaRequirement)
    {
        currentMana -= manaRequirement;
        slider.value = currentMana;
    }

    public void SetMaxManaBar()
    {
        currentMana = maxMana;
        slider.maxValue = maxMana;
        slider.value = currentMana;
    }

    public void CoolDowns()
    {
        if (timeBtwHealthRegen > 0)
        {
            timeBtwHealthRegen -= Time.deltaTime;
        }

        if (timeBtwQuickAttack > 0)
        {
            timeBtwQuickAttack -= Time.deltaTime;
        }

        if (timeBtwWaveSlameAttack > 0)
        {
            timeBtwWaveSlameAttack -= Time.deltaTime;
        }

        if (timeBtwSpearAttack > 0)
        {
            timeBtwSpearAttack -= Time.deltaTime;
        }

        if (rageTime > 0)
        {
            rageTime -= Time.deltaTime;
        }
        else if (rageActive)
        {
            rageActive = false;
            timeBtwRage = startTimeBtwRage;
        }

        if (timeBtwFireDash > 0)
        {
            timeBtwFireDash -= Time.deltaTime;
        }

        if (timeBtwFuryFistAttack > 0)
        {
            timeBtwFuryFistAttack -= Time.deltaTime;
        }

        if (timeBtwMagmaShotAttack > 0)
        {
            timeBtwMagmaShotAttack -= Time.deltaTime;
        }
    }

    public void QuickAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(quickAttackPosition.position, quickAttackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
            {
                enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(quickAttackDamage);
            }
        }
    }

    public void WaveSlameAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(waveSlameAttackPosition.position, waveSlameAttackRange, whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
            {
                enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(waveSlameAttackDamage);
            }
        }
    }

    public void SpearAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(spearAttackPosition.position, new Vector2(spearAttackRangeX, spearAttackRangeY), whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
            {
                enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(spearAttackDamage);
            }
        }
    }

    public void FireDash()
    {
        if (fireDashing)
        {
            if (transform.eulerAngles == new Vector3(0, 180, 0))
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.left * fireDashSpeed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.right * fireDashSpeed;
            }

            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(fireDashPosition.position, fireDashDamageRadius, whatIsEnemies);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
                {
                    if (rageActive)
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(fireDashDamage * 7 / 5);
                    }
                    else
                    {
                        enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(fireDashDamage);
                    }
                }
            }
        }
    }

    public void FireDashEnd()
    {
        fireDashing = false;
    }

    public void FuryFistAttack()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapBoxAll(furyFistAttackPosition.position, new Vector2(furyFistAttackRangeX, furyFistAttackRangeY), whatIsEnemies);
        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            if (enemiesToDamage[i].GetComponent<EnemyHealthSystem>() != null)
            {
                if (rageActive)
                {
                    enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(furyFistAttackDamage * 7/5);
                }
                else
                {
                    enemiesToDamage[i].GetComponent<EnemyHealthSystem>().GetDamage(furyFistAttackDamage);
                }
            }
        }
    }

    public void MagmaShotAttack()
    {
        Instantiate(magmaShot, magmaShotAttackPosition.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(quickAttackPosition.position, quickAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(waveSlameAttackPosition.position, waveSlameAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(spearAttackPosition.position, new Vector2(spearAttackRangeX, spearAttackRangeY));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(fireDashPosition.position, fireDashDamageRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(furyFistAttackPosition.position, new Vector2(furyFistAttackRangeX, furyFistAttackRangeY));
    }

    public void EndOfUsingSkill()
    {
        usingSkill = false;
        animator.SetBool("usingSkill", usingSkill);
    }
}