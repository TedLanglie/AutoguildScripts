using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldTextHandler : MonoBehaviour
{
    void Start()
    {
        GetComponent<TextMeshProUGUI>().text = "Current Gold: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().credits;
    }

    public void ChangeOfGold()
    {
        GetComponent<TextMeshProUGUI>().text = "Current Gold: " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().credits;
    }
}
