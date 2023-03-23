using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRestCanvas : MonoBehaviour
{
    [SerializeField] GameObject _MapCanvas;
    [SerializeField] GameObject _MyGuildCanvas;
    [SerializeField] GameObject _MainCanvas;
    public void BackToMainCanvas()
    {
        _MyGuildCanvas.SetActive(false);
        _MapCanvas.SetActive(false);

        _MainCanvas.SetActive(true);
    }

    public void ActivateGuildCanvas()
    {
        _MainCanvas.SetActive(false);
        _MapCanvas.SetActive(false);

        _MyGuildCanvas.SetActive(true);
    }

    public void ActivateMapCanvas()
    {
        _MainCanvas.SetActive(false);
        _MyGuildCanvas.SetActive(false);

        _MapCanvas.SetActive(true);
    }
}
