using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barrel : DamagebleObject
{
    public Transform AttackPoint;
    public LayerMask DamagableLayerMask;
    public float Damage;
    public float AttackRange;
    [SerializeField] private Animator anim;
    protected override void Die()
    {
        //anim = GetComponent<Animator>();
        anim.SetTrigger("Explosion");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, DamagableLayerMask);
        if (enemies.Length != 0)
        {
            for (int i = 0; i < enemies.Length; ++i)
            {
                enemies[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
            }
        }
    }
    private void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("after")) Destroy(gameObject); ;
    }
}