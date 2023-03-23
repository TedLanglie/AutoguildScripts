using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepingStrike : MonoBehaviour
{
    public void Activate(float damage, GameObject target, BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject targetedEnemy = battleManager.GetRandomUnit(target, TargetingPlayer);
        if(targetedEnemy == null) return;
        
        targetedEnemy.GetComponent<UnitBattle>().Hit(Mathf.Floor(damage), true, true, false, false, false, gameObject); // attack enemy with damage

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Sweeping Strike", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], targetedEnemy.transform.position, Quaternion.identity);
    }
}
