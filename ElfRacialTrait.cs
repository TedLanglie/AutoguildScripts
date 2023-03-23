using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfRacialTrait : MonoBehaviour
{
    void Awake()
    {
        GetComponent<UnitStats>().critChance += 10;
        GetComponent<UnitStats>().critDamage += 5;
    }

    public void Revoke()
    {
        GetComponent<UnitStats>().critChance -= 10;
        GetComponent<UnitStats>().critDamage -= 5;
    }
}
