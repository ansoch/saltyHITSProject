using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipisGimmick : DamagebleObject
{
    private GameObject player;
    private Rigidbody2D playerRB;

    [SerializeField] private float xForce = 5;
    [SerializeField] private float yForce = 5;

    // Start is called before the first frame update
    void Start()
    {
        hp = 1;
        player = GameObject.FindWithTag("Player");
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Die()
    {
        //base.Die();
        hp = 1;
        playerRB.AddForce(new Vector2(xForce * PlayerInfo.FacingRight, yForce), ForceMode2D.Impulse);
    }
}
