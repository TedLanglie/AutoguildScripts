using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evasion : MonoBehaviour
{
    bool triggered50percent = false;
    bool triggered25percent = false;
    bool triggered10percent = false;
    public void Activate()
    {
        if(GetComponent<UnitBattle>().CurrentHealth <= GetComponent<UnitBattle>().CurrentHealth * .5 && triggered50percent == false)
        {
            triggered50percent = true;
            GetComponent<UnitBattle>().CurrentDodgeChance += 5;
        }
        else if(GetComponent<UnitBattle>().CurrentHealth <= GetComponent<UnitBattle>().CurrentHealth * .25 && triggered25percent == false)
        {
            triggered50percent = true;
            GetComponent<UnitBattle>().CurrentDodgeChance += 5;
        }
        else if(GetComponent<UnitBattle>().CurrentHealth <= GetComponent<UnitBattle>().CurrentHealth * .1 && triggered10percent == false)
        {
            triggered50percent = true;
            GetComponent<UnitBattle>().CurrentDodgeChance += 5;
        }
    }
}
