using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    [Header("Enemy parametres")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private Transform groundDetection;
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
    [SerializeField] private float stateSpeed = 2;
    [SerializeField] private float rayDistance;

    [Header("Attack parametres")]
    [SerializeField] private float attackRange;
    [SerializeField] private float weakHpDamage;
    [SerializeField] private float weakBalanceDamage;
    [SerializeField] private float aoeHpDamage;
    [SerializeField] private float aoeBalanceDamage;
    [SerializeField] private float strongHpDamage;
    [SerializeField] private float strongBalanceDamage;
    [SerializeField] private float aoeCoeff = 4;
    [SerializeField] private GameObject bullet;

    [Header("Cooldowns parametres")]
    [SerializeField] private float cooldown = 0;
    [SerializeField] private float callDelay = 3;
    [SerializeField] private float attackDelay = 1;
    [SerializeField] private float aoeAttackDelay = 2;
    [SerializeField] private float hardAttackDelay = 3;
    [SerializeField] private float stateDelay = 4;
    [SerializeField] private float attackTime = 1;
    [SerializeField] private float attackCooldown = 1;


    [Header("Cooldowns parametres")]
    [SerializeField] private int attackType = 1;
    [SerializeField] private bool isDoing = false;
    [SerializeField] private bool isWaiting = false;

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
        anim.SetInteger("AttackType", 1);
    }

    void Update()
    {
        LifeCycle();
    }

    public void LifeCycle()
    {
        anim.SetBool("isWalk", false);
        if (PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            if(!isDoing)
            {
                isDoing = true;
                switch (attackType)
                {
                    case 1:
                        cooldown = callDelay;
                        break;
                    case 2:
                        cooldown = attackDelay;
                        break;
                    case 3:
                        cooldown = aoeAttackDelay;
                        break;
                    case 4:
                        cooldown = hardAttackDelay;
                        break;
                }
            }
            else
            {
                DoAState();
            }
        }
    }

    private void DoAState()
    {
        switch (attackType)
        {
            case 1:
                if(!isWaiting)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    if (cooldown >= 0)
                    {
                        anim.SetInteger("AttackType", 1);
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Shot(bullet, attackPoint);
                        if (attackCooldown >= 0)
                        {
                            anim.SetBool("isAttack", true);
                            attackCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            isWaiting = true;
                            cooldown = stateDelay;
                            attackCooldown = attackTime;
                        }
                    }
                }
                else
                {
                    anim.SetBool("isWalk", true);
                    anim.SetBool("isAttack", false);
                    if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= attackRange)
                    {
                        Pursuit(groundDetection, playerTransform, rayDistance, rb, stateSpeed, scaleX);
                    }
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 2;
                    }
                }
                break;

            case 2:
                if (!isWaiting)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    if (cooldown >= 0)
                    {
                        anim.SetInteger("AttackType", 2);
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange, weakHpDamage, weakBalanceDamage);
                        if (attackCooldown >= 0)
                        {
                            anim.SetBool("isAttack", true);
                            attackCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            isWaiting = true;
                            cooldown = stateDelay;
                            attackCooldown = attackTime;
                        }
                    }
                }
                else
                {
                    anim.SetBool("isWalk", true);
                    anim.SetBool("isAttack", false);
                    if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= attackRange)
                    {
                        Pursuit(groundDetection, playerTransform, rayDistance, rb, stateSpeed, scaleX);
                    }
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 3;
                    }
                }
                break;

            case 3:
                if (!isWaiting)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    if (cooldown >= 0)
                    {
                        anim.SetInteger("AttackType", 3);
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange * aoeCoeff, aoeHpDamage, aoeBalanceDamage);
                        if (attackCooldown >= 0)
                        {
                            anim.SetBool("isAttack", true);
                            attackCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            isWaiting = true;
                            cooldown = stateDelay;
                            attackCooldown = attackTime;
                        }
                    }
                }
                else
                {
                    anim.SetBool("isWalk", true);
                    anim.SetBool("isAttack", false);
                    if (Mathf.Abs(playerTransform.position.x - transform.position.x) >= attackRange)
                    {
                        Pursuit(groundDetection, playerTransform, rayDistance, rb, stateSpeed, scaleX);
                    }
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 4;
                    }
                }
                break;

            case 4:
                if (!isWaiting)
                {
                    anim.SetBool("isWalk", false);
                    anim.SetBool("isAttack", false);
                    if (cooldown >= 0)
                    {
                        anim.SetInteger("AttackType", 4);
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        Punch(attackPoint, playerLayer, attackRange, strongHpDamage, strongBalanceDamage);
                        if (attackCooldown >= 0)
                        {
                            anim.SetBool("isAttack", true);
                            attackCooldown -= Time.deltaTime;
                        }
                        else
                        {
                            isWaiting = true;
                            cooldown = stateDelay;
                            attackCooldown = attackTime;
                        }
                    }
                }
                else
                {
                    anim.SetBool("isWalk", true);
                    anim.SetBool("isAttack", false);
                    if (cooldown >= 0)
                    {
                        cooldown -= Time.deltaTime;
                    }
                    else
                    {
                        isWaiting = false;
                        isDoing = false;
                        attackType = 1;
                    }
                }
                break;
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
