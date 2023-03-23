using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundColorRandomizer : MonoBehaviour
{
    
    void Start()
    {
        int RandomRoll = Random.Range(0, 9);
        switch(RandomRoll)
        {
            case 0:
            GetComponent<SpriteRenderer>().color = new Color32(184, 208, 255, 255);
            break;
            case 1:
            GetComponent<SpriteRenderer>().color = new Color32(184, 255, 240, 255);
            break;
            case 2:
            GetComponent<SpriteRenderer>().color = new Color32(212, 255, 184, 255);
            break;
            case 3:
            GetComponent<SpriteRenderer>().color = new Color32(255, 222, 184, 255);
            break;
            case 4:
            GetComponent<SpriteRenderer>().color = new Color32(255, 184, 203, 255);
            break;
            case 5:
            GetComponent<SpriteRenderer>().color = new Color32(242, 184, 255, 255);
            break;
            case 6:
            GetComponent<SpriteRenderer>().color = new Color32(52, 90, 140, 255);
            break;
            case 7:
            GetComponent<SpriteRenderer>().color = new Color32(120, 50, 140, 255);
            break;
            case 8:
            GetComponent<SpriteRenderer>().color = new Color32(133, 133, 116, 255);
            break;
            default:
            break;
        }
    }
}
