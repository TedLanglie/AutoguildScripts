using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSwordsman : MonoBehaviour
{
    // activated if an enemy parries somebody with this ability
    public void Activate(GameObject Enemy)
    {
        Enemy.GetComponent<UnitBattle>().Hit(GetComponent<UnitBattle>().CurrentDamage, true, true, false, false, false, gameObject); // attack enemy with damage
        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Master Swordsman", false);
    }
}
