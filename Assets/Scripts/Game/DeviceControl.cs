using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceControl : MonoBehaviour
{
    public static DeviceControl Instance;

    [SerializeField] private GameObject _plot;
    [SerializeField] private GameObject _deviceSelection;

    private string _device = "Null";

    private void Awake()
    {
        Instance = this;
    }

    public void ButtonsDeviceSelection(string text)
    {
        _device = text;
        StartPlot();
    }

    private void StartPlot()
    {
        _deviceSelection.SetActive(false);
        _plot.SetActive(true);
    }

    public string CurrentDevice()
    {
        return _device;
    }
}
