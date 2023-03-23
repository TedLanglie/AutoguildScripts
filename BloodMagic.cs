using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodMagic : MonoBehaviour
{
    public void Activate(GameObject targetedEnemy)
    {
        if(!targetedEnemy.GetComponent<UnitBattle>().isDead)
        {
            targetedEnemy.GetComponent<UnitBattle>().Hit(Mathf.Floor(targetedEnemy.GetComponent<UnitBattle>().CurrentDamage/2), false, false, false, false, false, gameObject); // attack enemy with damage
            
            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Blood Magic", false);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[8], targetedEnemy.transform.position, Quaternion.identity);
        }
    }
}
