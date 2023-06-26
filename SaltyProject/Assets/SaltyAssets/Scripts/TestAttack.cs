using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    public Transform AttackPoint;
    public LayerMask DamagableLayerMask;
    public float Damage;
    public float AttackRange;
    public float TimeBtwAttack;
    private float timer;

    [SerializeField] private Animator playerAnimator;

    public string CurrentWeaponAnim { get; private set; } = "weapon_sword_side";

    private string _swordAnim = "weapon_sword_side";
    private string _hammerAnim = "weapon_hammer_side";
    private string _scytheAnim = "weapon_sycthe_side";

    private AnimatorStateInfo stateInfo;
    private AnimatorStateInfo playerStateInfo;
    Animator anim;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
    public void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            anim.SetInteger("WeaponType", (int)WeaponType.sword);
            CurrentWeaponAnim = _swordAnim;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            anim.SetInteger("WeaponType", (int)WeaponType.hammer);
            CurrentWeaponAnim = _hammerAnim;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            anim.SetInteger("WeaponType", (int)WeaponType.scythe);
            CurrentWeaponAnim = _scytheAnim;
        }


        if (!stateInfo.IsName(CurrentWeaponAnim) && !playerStateInfo.IsName("slide_side"))
        {
            if(Input.GetButtonDown("Fire1"))
            {
                anim.SetTrigger("IsAttacking");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange,DamagableLayerMask);
                if (enemies.Length != 0)
                {
                    for (int i = 0; i < enemies.Length; ++i)
                    {
                        enemies[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
                    }
                }
            }
        }
    }
    private void GetReferences()
    {

    }
    private void Update()
    {
        stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        playerStateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);
        Attack();
    }
    private void FixedUpdate()
    {
        
    }
}
