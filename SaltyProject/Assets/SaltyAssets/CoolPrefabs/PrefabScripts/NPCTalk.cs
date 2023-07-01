using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class NPCTalk : MonoBehaviour, IInteractible
{
    public bool IsInRange { get; private set; }
    [field: SerializeField] public Collider2D Collider { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody { get; private set; }
    [SerializeField] private GameObject pressX;

    [SerializeField] private GameObject dialoguePanel;
    //[SerializeField] private GameObject text;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private string dialogue;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = true;
            pressX.active = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsInRange = false;
            pressX.active = false;
        }
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(IsInRange && Input.GetKeyDown(KeyCode.X))
        {
            dialoguePanel.SetActive(!dialoguePanel.active);

            text.text = dialogue;
        }
    }
    public IPlayerState Interact() => new TalkingPlayerState();
}
