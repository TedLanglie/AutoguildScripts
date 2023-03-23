using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eviscerate : MonoBehaviour
{
    // Start is called before the first frame update
    public void Activate(GameObject targetedEnemy)
    {
        if(targetedEnemy.GetComponent<UnitBattle>().CurrentHealth > targetedEnemy.GetComponent<UnitStats>().maxHealth * .4)
        {
            targetedEnemy.GetComponent<UnitBattle>().Hit(7, false, false, false, false, false, gameObject); // attack enemy with damage
            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Eviscerate", false);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], targetedEnemy.transform.position, Quaternion.identity);
        }
    }
}
