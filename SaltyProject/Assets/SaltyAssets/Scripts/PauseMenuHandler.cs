using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        continueButton.onClick.AddListener(() => {
            PauseMenu.SetActive(false);
        });
        exitButton.onClick.AddListener(() => {
            SceneManager.LoadScene("MainMenu");
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;  
            PauseMenu.SetActive(!PauseMenu.active);
        }
    }
}
