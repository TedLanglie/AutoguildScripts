using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShield : MonoBehaviour
{
    public void Activate(GameObject attacker)
    {
        if(!attacker.GetComponent<UnitBattle>().isDead)
        {
            if(attacker.GetComponent<Burning>() != null) attacker.GetComponent<Burning>().Disable();
            attacker.AddComponent(System.Type.GetType("Burning")); // add the script 
            attacker.GetComponent<Burning>().Activate(3);

            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Fire Shield", false);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[15], transform.position, Quaternion.identity);
        }
    }
}
