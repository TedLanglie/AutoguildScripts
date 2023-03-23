using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodKnives : MonoBehaviour
{
    public void Activate(GameObject targetEnemy)
    {
        if(targetEnemy.GetComponent<UnitBattle>().CurrentHealth <= targetEnemy.GetComponent<UnitStats>().maxHealth * .5)
        {
            GetComponent<UnitBattle>().CurrentCritChance += 50;
            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Blood Knives", false);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], transform.position, Quaternion.identity);
        }
    }

    public void DeActivate()
    {
        GetComponent<UnitBattle>().CurrentCritChance -= 50;
    }
}
