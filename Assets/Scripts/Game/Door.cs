using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance;

    [SerializeField] private string _name = "Null";

    private void Start() { Instance = this; }

    public string DoorName() { return _name; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            GameUI.Instance.ButtonActive(true, "Door");
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            GameUI.Instance.ButtonActive(false, "Use");
    }
}
