using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBath : MonoBehaviour
{
    // gain +2 attack if both unit and targeted unit are INJURED
    public void Activate(GameObject targetedUnit)
    {
        if(GetComponent<UnitBattle>().getInjuryStatus() && targetedUnit.GetComponent<UnitBattle>().getInjuryStatus())
        {
            GetComponent<UnitBattle>().CurrentDamage += 7;

            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Blood Bath", false);

            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], transform.position, Quaternion.identity);
        }
    }
}
