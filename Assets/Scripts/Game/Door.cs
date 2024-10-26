using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public static Door Instance;

    [SerializeField] private string _name = "Null";
    [SerializeField] private GameObject _wordE;

    private void Start()
    {
        Instance = this;
    }

    public string DoorName() { return _name; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameUI.Instance.ButtonActive(true, "Door");
            if (DeviceControl.Instance.CurrentDevice() == "PC")
                _wordE.SetActive(true);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GameUI.Instance.ButtonActive(false, "Use");
            if (_wordE.activeSelf)
                _wordE.SetActive(false);
        }
    }
}
