using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Enemy
{
    [Header("Enemy parametres")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
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

    [Header("Turn parametres")]
    [SerializeField] private float turnCooldown = 0;
    [SerializeField] private float turnDelay;

    [Header("Attack parametres")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private float burstDelay = 3;
    [SerializeField] private float shotDelay = 0.5f;
    [SerializeField] private float numOfBurst = 3;
    [SerializeField] private Vector3 cooldowns;

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
        cooldowns = new Vector3(0, 0, numOfBurst);
        playerObject = GameObject.FindWithTag("Player");
        playerTransform = playerObject.GetComponent<Transform>();
    }

    void Update()
    {
        LifeCycle();
    }

    public void LifeCycle()
    {
        if (PlayerInSight(playerLayer, boxCollider, range, colliderDistance, high))
        {
            anim.SetBool("Attack", true);
            cooldowns = Burst(bullet, attackPoint, playerTransform, cooldowns[0], burstDelay, cooldowns[1], shotDelay, cooldowns[2], numOfBurst, scaleX);
        }
        else
        {
            if(turnCooldown <= 0)
            {
                anim.SetBool("Attack", false);
                TurnAround();
                turnCooldown = turnDelay;
            }
            else
            {
                anim.SetBool("Attack", false);
                turnCooldown -= Time.deltaTime;
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
