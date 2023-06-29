using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour, IInteractible
{
    private bool isInRange = false;
    [field: SerializeField] public Collider2D Collider { get; private set;}
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [SerializeField] private GameObject pressX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            pressX.active = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            pressX.active = false;
    }

    public IPlayerState Interact() => new CarryingPlayerState();
}
