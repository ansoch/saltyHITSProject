using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    sword,
    hammer,
    scythe
}

public class Player : MonoBehaviour
{
    public Animator Anim { get; private set; }

    public float speed;
    public float jumpForse;
    public Rigidbody2D rb { get; private set; }
    public Transform groundCheck;
    public LayerMask WhatIsGround;
    public float radiusGroundCheck;
    public bool IsGrounded { get; private set; } = true;
    private float signPreviousFrame;
    private float signCurrentFrame;
    private Vector3 _leftFlip = new Vector3(0, 180, 0);
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;

    public string CurrentWeaponAnim { get; private set; } = "sword_side";

    private string _swordAnim = "sword_side";
    private string _hammerAnim = "hammer_side";
    private string _scytheAnim = "sycthe_side";



    private float _facingRight = -1;

    //private IPlayerState _playerState = new IdlePlayerState();

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //rb.drag = 0f;
        //rb.angularDrag = 0f;
        inventory = new Inventory();
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null && Input.GetKey(KeyCode.E))
        {
            if (inventory.GetItemList().Count < 8)
            {

                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
            }
        }

        /*
        if (collision.CompareTag("Carriable"))
        {
            //heldObject = collision.gameObject;
        }
        */
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    private void Flip()
    {
        if (Input.GetKey(KeyCode.A))
            transform.rotation = Quaternion.Euler(Vector3.zero);
        else if (Input.GetKey(KeyCode.D))
            transform.rotation = Quaternion.Euler(_leftFlip);
    }
    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName(CurrentWeaponAnim) && !stateInfo.IsName("slide_side"))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);

            if (Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
                _facingRight = 1f;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                _facingRight = -1f;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CurrentWeaponAnim = _swordAnim;
                Anim.SetInteger("WeaponType", (int)WeaponType.sword);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CurrentWeaponAnim = _hammerAnim;
                Anim.SetInteger("WeaponType", (int)WeaponType.hammer);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CurrentWeaponAnim = _scytheAnim;
                Anim.SetInteger("WeaponType", (int)WeaponType.scythe);
            }

            if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            {
                rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
                //Anim.Play("jump_side");
                Anim.SetTrigger("IsJumping");
            }
            if (Input.GetButtonDown("Fire1"))
            {
                //Anim.Play("AttackingCatana");
                rb.velocity = new Vector2(speed*_facingRight*0.75f, rb.velocity.y);
                Anim.SetTrigger("IsAttacking");
            }
            if (Input.GetKeyDown(KeyCode.LeftShift)){
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
                rb.velocity = new Vector2(speed * _facingRight * 2f, rb.velocity.y);
                Anim.SetTrigger("IsSliding");
                //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
            }
            Flip();
        }
        //_playerState = _playerState.UpdateState(this);
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        Anim.SetFloat("SpeedX", Math.Abs(rb.velocity.x));
        Anim.SetBool("IsRunning", Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
        Anim.SetBool("IsGrounded", IsGrounded);
        //NormalizeSlope();
    }
    void NormalizeSlope()
    {
        // Attempt vertical normalization
        if (IsGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 0.5f, WhatIsGround);

            if (hit.collider != null && Mathf.Abs(hit.normal.x) > 0.1f)
            {
                Rigidbody2D body = rb;
                // Apply the opposite force against the slope force 
                // You will need to provide your own slopeFriction to stabalize movement
                body.velocity = new Vector2(body.velocity.x - (hit.normal.x * 0.6f), body.velocity.y);

                //Move Player up or down to compensate for the slope below them
                Vector3 pos = transform.position;
                pos.y += -hit.normal.x * Mathf.Abs(body.velocity.x) * Time.deltaTime * (body.velocity.x - hit.normal.x > 0 ? 1 : -1);
                transform.position = pos;
            }
        }
    }
}