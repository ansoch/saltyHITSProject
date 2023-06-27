using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformExit : MonoBehaviour
{
    [SerializeField] private Collider2D boxCollider;
    private bool isSDown = false;
    private bool isOnPlatform = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnPlatform = true;
        }
    }

    private void OnTriggerExit2D()
    {
        isOnPlatform = false;
        boxCollider.isTrigger = false;
    }

    private void OnCollisionExit2D()
    {
        isOnPlatform = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && isOnPlatform) boxCollider.isTrigger = true;
    }
    private void FixedUpdate()
    {

    }
}
