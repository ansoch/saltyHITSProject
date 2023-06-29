using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Animator fireAnim;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && PlayerInfo.IsNotBusy)
        {
            //spriteRenderer.enabled = true;
            fireAnim.SetTrigger("IsFiring");
            Fire();
        }

        if (fireAnim.GetCurrentAnimatorStateInfo(0).IsName("muzzleflash_gun")) spriteRenderer.enabled = true;
        else spriteRenderer.enabled = false;
    }

    private void Fire()
    {
        Vector2 spawnPosition = new Vector2(spawnPoint.position.x, spawnPoint.position.y);
        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
    } 
}
