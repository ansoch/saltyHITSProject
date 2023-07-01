using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    private PickUpObject bomb;
    private Animator bombAnimator;
    public Transform AttackPoint;
    public LayerMask DamagableLayerMask;
    public LayerMask Barrels;
    public float Damage;
    public float AttackRange;
    [SerializeField] private float igniteTimer = 3f;
    [SerializeField] private float explosionTimer = 1.3f;
    [SerializeField] private GameObject bombObject;

    private bool isPickedUp = false;
    private bool isIgnited = false;
    void Start()
    {
        bomb = GetComponent<PickUpObject>();
        bombAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (bomb.IsInRange && Input.GetKeyDown(KeyCode.X) && !isPickedUp)
        {
            isPickedUp = true;

            bombAnimator.SetBool("IsPickedUp", true);

            isIgnited = true;
        }
        //else if (isPickedUp && Input.GetKeyDown(KeyCode.X)) Throw();
    }
    private void FixedUpdate()
    {
        if (isIgnited) igniteTimer -= Time.fixedDeltaTime;

        if (igniteTimer <= 0)
        {
            explosionTimer -= Time.fixedDeltaTime;
            Throw();
        }

        if (explosionTimer <= 0)
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, DamagableLayerMask);
            if (enemies.Length != 0)
            {
                for (int i = 0; i < enemies.Length; ++i)
                {
                    enemies[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
                }
            }
            Collider2D[] barrels = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, Barrels);
            if (barrels.Length != 0)
            {
                for (int i = 0; i < barrels.Length; ++i)
                {
                    barrels[i].GetComponent<DamagebleObject>().TakeDamage(Damage);
                }
            }
            Destroy(bombObject);
        }
    }

    private void Throw()
    {
        bombAnimator.SetTrigger("IsThrown");
    }
}
