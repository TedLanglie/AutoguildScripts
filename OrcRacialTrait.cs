using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcRacialTrait : MonoBehaviour
{
    void Awake()
    {
        GetComponent<UnitStats>().maxHealth += 5;
    }

    public void Revoke()
    {
        GetComponent<UnitStats>().maxHealth -= 5;
    }
}
