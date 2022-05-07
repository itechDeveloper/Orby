using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillSystem : MonoBehaviour
{
    Animator animator;

    public bool water;
    public bool fire;
    public static bool usingSkill;

    public float healthRegen;
    public static bool rageActive;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (GetComponent<PlayerMovement>().isGrounded)
        {
            if (Input.GetKey(KeyCode.Alpha1) && !usingSkill)
            {
                if (water)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("heal");
                    GetComponent<PlayerHealthSystem>().GetHeal(healthRegen);
                }else if (fire)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("rage");
                    rageActive = true;
                }
            }
            if (Input.GetKey(KeyCode.Alpha2) && !usingSkill)
            {
                if (water)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("quickAttack");
                }else if (fire)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("fireDash");
                }
            }
            if (Input.GetKey(KeyCode.Alpha3) && !usingSkill)
            {
                if (water)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("waveSlame");
                }
                else if (fire)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("furyFist");
                }
            }
            if (Input.GetKey(KeyCode.Alpha4) && !usingSkill)
            {
                if (water)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("spearAttack");
                }
                else if (fire)
                {
                    usingSkill = true;
                    animator.SetBool("usingSkill", usingSkill);
                    animator.SetTrigger("magmaShot");
                }
            }
        }
    }

    

    public void EndOfUsingSkill()
    {
        usingSkill = false;
        animator.SetBool("usingSkill", usingSkill);
    }
}
