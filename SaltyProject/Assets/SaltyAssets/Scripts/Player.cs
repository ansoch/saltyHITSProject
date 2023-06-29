using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    sword,
    hammer,
    sycthe
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
    private string _syctheAnim = "sycthe_side";

    private bool _isFiring = false;

    [SerializeField] private Collider2D collider;
    [SerializeField] private PhysicsMaterial2D highFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D lowFrictionMaterial;
    [SerializeField] private GameObject shield;

    private float _facingRight = -1;

    //private IPlayerState _playerState = new IdlePlayerState();

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inventory = new Inventory();
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
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

        bool isNotBusy = !stateInfo.IsName(CurrentWeaponAnim) && !stateInfo.IsName("slide_side") && !_isFiring;
        this.SetState(isNotBusy);

        if (isNotBusy)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);

            Running();

            WeaponChanger();

            OtherActions();

            Flip();
        }

        _isFiring = false;
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        Anim.SetFloat("SpeedX", Math.Abs(rb.velocity.x));
        Anim.SetBool("IsRunning", Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A));
        Anim.SetBool("IsGrounded", IsGrounded);
    }

    private void WeaponChanger()
    {
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
            CurrentWeaponAnim = _syctheAnim;
            Anim.SetInteger("WeaponType", (int)WeaponType.sycthe);
        }
    }

    private void Running()
    {
        if (Input.GetKey(KeyCode.D))
        {
            collider.sharedMaterial = lowFrictionMaterial;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            _facingRight = 1f;
            this.SetFacingRight(_facingRight);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            collider.sharedMaterial = lowFrictionMaterial;
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            _facingRight = -1f;
            this.SetFacingRight(_facingRight);
        }
        else
        {
            collider.sharedMaterial = highFrictionMaterial;
        }
    }

    private void OtherActions()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(transform.up * jumpForse, ForceMode2D.Impulse);
            Anim.SetTrigger("IsJumping");
        }

        if (Input.GetButtonDown("Fire1") && PlayerInfo.Stamina > 0)
        {
            collider.sharedMaterial = lowFrictionMaterial;
            //rb.velocity = new Vector2(speed * _facingRight * 2f, rb.velocity.y);
            Anim.SetTrigger("IsAttacking");
            PlayerInfo.OnAction(15);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && PlayerInfo.Stamina > 0)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), true);
            rb.velocity = new Vector2(speed * _facingRight * 2.5f, rb.velocity.y);
            Anim.SetTrigger("IsSliding");
            collider.sharedMaterial = lowFrictionMaterial;
            PlayerInfo.OnAction(20);
        }

        if (Input.GetMouseButtonDown(1) && PlayerInfo.Stamina > 0)
        {
            shield.active = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            shield.active = false;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isFiring = true;
        }
    }
}

