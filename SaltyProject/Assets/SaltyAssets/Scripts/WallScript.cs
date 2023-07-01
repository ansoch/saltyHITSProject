using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneManager.LoadScene("Level2");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
