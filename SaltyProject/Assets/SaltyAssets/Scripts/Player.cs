using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private TestAttack TA;

    public string CurrentWeaponAnim { get; private set; } = "sword_side";

    private string _swordAnim = "sword_side";
    private string _hammerAnim = "hammer_side";
    private string _syctheAnim = "sycthe_side";
    private bool OneItemTake = false;

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



    [SerializeField] private float poisonTime = 0;
    [SerializeField] private float poisonDamage = 0.01f;

    [SerializeField] private float balance = 100f;

    [SerializeField] private float balanceHealTime = 6;
    [SerializeField] private bool isTimer = false;

    [SerializeField] private float stunTime = 4;
    [SerializeField] private bool isStuned = false;

    [SerializeField] private GameObject panel;

    //private IPlayerState _playerState = new IdlePlayerState();

    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        inventory = new Inventory();
        uiInventory.SetPlayer(this);
        uiInventory.SetInventory(inventory);
        inventory.AddItem(new Item { itemType = Item.ItemType.ForWeapon, amount = 1 });
        inventory.AddItem(new Item { itemType = Item.ItemType.ForWeapon, amount = 1 });
        inventory.AddItem(new Item { itemType = Item.ItemType.ForWeapon, amount = 1 });
        inventory.AddItem(new Item { itemType = Item.ItemType.Weapon, amount = 1 });
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (OneItemTake == true)
        {
            Debug.Log("OneItem=true");
        }
        if (itemWorld != null && Input.GetKey(KeyCode.E) && !OneItemTake)
        {
            if (inventory.GetItemList().Count < 8)
            {
                inventory.AddItem(itemWorld.GetItem());
                itemWorld.DestroySelf();
                OneItemTake = true;
            }
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interactible"))
        {
            InteructibleObject = collision.gameObject;
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("asdasdas");
            SceneManager.LoadScene("Level 2");
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
        Debug.Log(PlayerInfo.Health);
        AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        bool isNotBusy = !stateInfo.IsName(CurrentWeaponAnim) && !stateInfo.IsName("slide_side") && !_isFiring && !IsShielded;
        this.SetState(isNotBusy);
        if (isStuned == true)
        {
            stunTime -= Time.deltaTime;
            balance = 100f;
            if (stunTime <= 0)
            {
                isStuned = false;
                stunTime = 4;
            }
        }
        else
        {
            if (isTimer == true)
            {
                balanceHealTime -= Time.deltaTime;
                if (balanceHealTime <= 0)
                {
                    balance = 100f;
                    isTimer = false;
                }
            }
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
        }
        UseInventory();
        _isFiring = false;
    }
    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(groundCheck.position, radiusGroundCheck, WhatIsGround);
        playerState.FixedUpdateState(this);
    }
    private void UseItem(Item item)
    {
        if (item.itemType == Item.ItemType.HealthPotion)
        {
            PlayerInfo.OnHealing(3);
            inventory.RemoveItem(new Item { itemType = Item.ItemType.HealthPotion, amount = 1 });
        }
        else if (item.itemType == Item.ItemType.Scythe)
        {
            CurrentWeaponAnim = _syctheAnim;
            Anim.SetInteger("WeaponType", (int)WeaponType.sycthe);
            TA.anim.SetInteger("WeaponType", (int)WeaponType.sycthe);
            TA.CurrentWeaponAnim = _syctheAnim;
        }
        else if (item.itemType == Item.ItemType.Weapon)
        {
            CurrentWeaponAnim = _swordAnim;
            Anim.SetInteger("WeaponType", (int)WeaponType.sword);
            TA.anim.SetInteger("WeaponType", (int)WeaponType.sword);
            TA.CurrentWeaponAnim = _swordAnim;
        }
        else if (item.itemType == Item.ItemType.Hummer)
        {
            CurrentWeaponAnim = _hammerAnim;
            Anim.SetInteger("WeaponType", (int)WeaponType.hammer);
            TA.anim.SetInteger("WeaponType", (int)WeaponType.hammer);
            TA.CurrentWeaponAnim = _hammerAnim;
        }
    }
    private void UseInventory()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.GetItemList().Count >= 1)
                UseItem(inventory.GetItemList()[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory.GetItemList().Count >= 2)
                UseItem(inventory.GetItemList()[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory.GetItemList().Count >= 3)
                UseItem(inventory.GetItemList()[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory.GetItemList().Count >= 4)
                UseItem(inventory.GetItemList()[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (inventory.GetItemList().Count >= 5)
                UseItem(inventory.GetItemList()[4]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (inventory.GetItemList().Count >= 6)
                UseItem(inventory.GetItemList()[5]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (inventory.GetItemList().Count >= 7)
                UseItem(inventory.GetItemList()[6]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (inventory.GetItemList().Count >= 8)
                UseItem(inventory.GetItemList()[7]);
        }
    }
    public void WeaponChanger()
    {
        /*
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
        */
    }

    public void Running()
    {
        if (Input.GetKey(KeyCode.D))
        {
            collider.sharedMaterial = lowFrictionMaterial;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            _facingRight = 1f;
            this.SetFacingRight(_facingRight);
            OneItemTake = false;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            collider.sharedMaterial = lowFrictionMaterial;
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            _facingRight = -1f;
            this.SetFacingRight(_facingRight);
            OneItemTake = false;
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


    public void TakeDamage(float hpDamage, float balanceDamage)
    {
        if (Anim.GetCurrentAnimatorStateInfo(0).IsName("slide_side")) return;

        PlayerInfo.OnDamage((int)hpDamage);
        Anim.Play("hit2_side");
        balance -= balanceDamage;
        balanceHealTime = 6;
        isTimer = true;
        if (PlayerInfo.Health <= 0)
        {
            Die();
        }
        if (balance <= 0)
        {
            isTimer = false;
            isStuned = true;
        }
    }

    public void Die()
    {
        PlayerInfo.OnHealing(100);
        Time.timeScale = 0;

        panel.SetActive(true);

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void KnockBack(bool direction)
    {
        if (!direction)
        {
            rb.AddForce((Vector2.up + Vector2.right) * jumpForse, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce((Vector2.up + Vector2.left) * jumpForse, ForceMode2D.Impulse);
        }
    }

    public void GetPoisoned(float poisonAmount)
    {
        poisonTime = poisonAmount;
    }
    public void GetDamageByPoison()
    {
        if (poisonTime > 0)
        {
            poisonTime -= Time.deltaTime;
            //hp -= poisonDamage;
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

        if (player.InteructibleObject != null)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                player.InteructibleObject.GetComponent<IInteractible>().Collider.enabled = false;
                return player.InteructibleObject.GetComponent<IInteractible>().Interact();
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) && player.InteructibleObject.GetComponent<LadderScript>() != null)
            {
                Transform playerTransform = player.GetComponent<Transform>();
                Transform ladderTransform = player.InteructibleObject.GetComponent<Transform>();

                playerTransform.position = new Vector3(ladderTransform.position.x, playerTransform.position.y, playerTransform.position.z);
                player.rb.gravityScale = 0;

                return player.InteructibleObject.GetComponent<IInteractible>().Interact();
            }
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
        if (player.InteructibleObject != null) PickUp(player, player.InteructibleObject);
        else return new BasicPlayerState();

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

public class LadderPlayerState : IPlayerState
{
    public IPlayerState UpdateState(Player player)
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.rb.velocity = new Vector2(0, 5);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            player.rb.velocity = new Vector2(0, -5);
        }
        else
        {
            player.rb.velocity = new Vector2(0, 0);
        }

        if (player.InteructibleObject == null || Input.GetKeyDown(KeyCode.Space))
        {
            player.rb.gravityScale = 3;
            player.Anim.SetBool("IsOnLadder", false);
            player.Anim.SetBool("IsMoving", false);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.rb.AddForce(player.transform.up * player.jumpForse, ForceMode2D.Impulse);
                player.Anim.SetTrigger("IsJumping");
            }

            return new BasicPlayerState();
        }

        return this;
    }
    public void FixedUpdateState(Player player)
    {
        player.Anim.SetBool("IsOnLadder", true);
        player.Anim.SetBool("IsMoving", Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W));
    }
}