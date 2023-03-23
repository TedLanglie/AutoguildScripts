using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadRacialTrait : MonoBehaviour
{
    void Awake()
    {
        GetComponent<UnitStats>().baseDamage += 2;
        GetComponent<UnitStats>().blockChance += 15;
    }

    public void Revoke()
    {
        GetComponent<UnitStats>().baseDamage -= 2;
        GetComponent<UnitStats>().blockChance -= 15;
    }
}
