using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    // Start is called before the first frame update
    private PickUpObject bomb;
    private Animator bombAnimator;

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

            bombAnimator.SetBool("IsPickedUp",true);

            isIgnited = true;
        }
            //else if (isPickedUp && Input.GetKeyDown(KeyCode.X)) Throw();
    }
    private void FixedUpdate()
    {
        if(isIgnited) igniteTimer -= Time.fixedDeltaTime;

        if (igniteTimer <= 0)
        {
            explosionTimer -= Time.fixedDeltaTime;
            Throw();
        }

        if (explosionTimer <= 0) Destroy(bombObject);
    }

    private void Throw()
    {
        bombAnimator.SetTrigger("IsThrown");
    }
}
