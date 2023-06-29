using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
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
    public bool IsShielded { get; private set; } = false;

    [SerializeField] private Collider2D collider;
    [SerializeField] private PhysicsMaterial2D highFrictionMaterial;
    [SerializeField] private PhysicsMaterial2D lowFrictionMaterial;
    [SerializeField] private GameObject shield;
    [field: SerializeField] public Transform PickUpTransform { get; private set; }

    private float _facingRight = -1;

    private IPlayerState playerState = new BasicPlayerState();

    public GameObject InteructibleObject { get; private set; } = null;

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

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactible"))
        {
            InteructibleObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactible"))
        {
            InteructibleObject = null;
        }
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

        bool isNotBusy = !stateInfo.IsName(CurrentWeaponAnim) && !stateInfo.IsName("slide_side") && !_isFiring && !IsShielded;
        this.SetState(isNotBusy);

        if (isNotBusy)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer("Enemy"), false);

            playerState = playerState.UpdateState(this);

            Flip();
        }
        else
        {
            if (Input.GetMouseButtonUp(1))
            {
                shield.active = false;
                IsShielded = false;
            }
        }
        _isFiring = false;
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        playerState.FixedUpdateState(this);
    }

    public void WeaponChanger()
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

    public void Running()
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

    public void OtherActions()
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
            IsShielded = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isFiring = true;
        }
    }
}

public interface IPlayerState
{
    IPlayerState UpdateState(Player player);
    void FixedUpdateState(Player player);
}

public class BasicPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        player.WeaponChanger();
        player.Running();
        player.OtherActions();

        if (Input.GetKeyDown(KeyCode.X) && player.InteructibleObject != null)
        {
            player.InteructibleObject.GetComponent<IInteractible>().Collider.enabled = false;
            return player.InteructibleObject.GetComponent<IInteractible>().Interact();
        }

        return this;
    }
    public void FixedUpdateState(Player player)
    {
        player.Anim.SetFloat("SpeedX", Math.Abs(player.rb.velocity.x));
        player.Anim.SetBool("IsRunning", (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && !player.IsShielded);
        player.Anim.SetBool("IsGrounded", player.IsGrounded);
        player.Anim.SetBool("IsCarrying", false);
    }
}

public class CarryingPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        player.Running();
        PickUp(player, player.InteructibleObject);

        if (Input.GetKeyDown(KeyCode.X))
        {
            player.InteructibleObject.GetComponent<IInteractible>().Rigidbody.velocity = new Vector2(10 * PlayerInfo.FacingRight, 1);
            player.InteructibleObject.GetComponent<IInteractible>().Collider.enabled = true;
            return new BasicPlayerState();
        }

        return this;
    }
    public void FixedUpdateState(Player player)
    {
        player.Anim.SetBool("IsRunning", (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)));
        player.Anim.SetBool("IsCarrying", true);
    }
    private void PickUp(Player player, GameObject pickUpObject) => pickUpObject.transform.position = player.PickUpTransform.position;
}
public class TalkingPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            return new BasicPlayerState();
        }

        return this;
    }
    public void FixedUpdateState(Player player)
    {
        
    }
}