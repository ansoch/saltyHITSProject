using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private Button SPButton;
    [SerializeField] private Button HostButton;
    [SerializeField] private Button ConnectButton;
    [SerializeField] private Button ExitButton;

    private void Awake()
    {
        SPButton.onClick.AddListener(() => {
            SceneManager.LoadScene("Level");
        });
        ExitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
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
