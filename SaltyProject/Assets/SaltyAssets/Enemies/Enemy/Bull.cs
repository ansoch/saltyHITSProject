using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : Enemy
{
    [Header("Enemy parametres")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform groundDetection;
    [SerializeField] private Transform wallDetection;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float scaleX;

    [Header("Enemy parametres")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform playerTransform;

    [Header("Vision parametres")]
    [SerializeField] private float range;
    [SerializeField] private float high;
    [SerializeField] private float colliderDistance;

    [Header("Moving parametres")]
    [SerializeField] private float patrolSpeed = 2;
    [SerializeField] private float huntSpeed = 4;
    [SerializeField] private float dashSpeed = 10;
    [SerializeField] private float rayDistance;

    [Header("Attack parametres")]
    [SerializeField] private float attackRange;
    [SerializeField] private float dashRange;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float attackCooldown = 0;
    [SerializeField] private float hpDamage;
    [SerializeField] private float dashDamage;
    [SerializeField] private float balanceDamage;
    [SerializeField] private bool isDash = true;

    [Header("Animation")]
    [SerializeField] private Animator anim;

    public void Start()
    {
        GetReferences();
    }
    private void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
        playerObject = GameObject.FindWithTag("Player");
        playerTransform = playerObject.GetComponent<Transform>();
    }

    void Update()
    {
        LifeCycle();
    }

    void LifeCycle()
    {
        if (!PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            anim.SetBool("Attack", false);
            Patrol(groundDetection, wallDetection, rayDistance, rb, patrolSpeed, scaleX);
        }
        else
        {
            if ((Mathf.Abs(playerTransform.position.x - transform.position.x) == dashRange) || (isDash))
            {
                anim.SetBool("Attack", false);
                attackCooldown = 0;
                isDash = DashAttack(attackPoint, playerTransform, rb, playerLayer, dashSpeed, attackRange, dashDamage, scaleX);
            }
            else
            {
                if (!isDash)
                {
                    if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= attackRange)
                    {
                        anim.SetBool("Attack", false);
                        Pursuit(groundDetection, playerTransform, rayDistance, rb, huntSpeed, scaleX);
                        attackCooldown = 0;
                    }
                    else
                    {
                        anim.SetBool("Attack", true);
                        attackCooldown = Attack(attackPoint, playerLayer, playerTransform, attackCooldown, attackDelay, attackRange, hpDamage, balanceDamage, scaleX);
                    }
                }
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y + high, boxCollider.bounds.size.z));
    }
}
