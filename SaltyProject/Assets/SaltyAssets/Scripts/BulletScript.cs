using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private GameObject thisGameObject;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;

    public int Damage { get; private set; } = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = new Vector2(20 * PlayerInfo.FacingRight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(thisGameObject);
        collision.gameObject.GetComponent<DamagebleObject>().TakeDamage(Damage);
    }
}
