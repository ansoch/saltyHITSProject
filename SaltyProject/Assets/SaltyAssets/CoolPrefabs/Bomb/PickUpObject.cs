using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour, IInteractible
{
    public bool IsInRange { get; private set; }
    [field: SerializeField] public Collider2D Collider { get; private set;}
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [SerializeField] private GameObject pressX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsInRange && Input.GetKeyDown(KeyCode.X)) PressXSwitch();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pressX.active = true;
            IsInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pressX.active = false;
            IsInRange = false;
        }
    }

    public IPlayerState Interact() => new CarryingPlayerState();

    private void PressXSwitch() => pressX.active = !pressX.active;
}
