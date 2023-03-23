using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceOfFlames : MonoBehaviour
{
    // apply burning to target, and a random enemy
    public void Activate(BattleManager battleManager, GameObject attackedEnemy)
    {
        if(!attackedEnemy.GetComponent<UnitBattle>().isDead)
        {
            // apply burning to attacked enemy
            if(attackedEnemy.GetComponent<Burning>() != null) attackedEnemy.GetComponent<Burning>().Disable(); // if theres already this status effect, remove it
            attackedEnemy.AddComponent(System.Type.GetType("Burning")); // add the script
            attackedEnemy.GetComponent<Burning>().Activate(3);
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Dance Of Flames", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[15], transform.position, Quaternion.identity);

        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject targetedEnemy = battleManager.GetRandomUnit(attackedEnemy, TargetingPlayer);
        if(targetedEnemy == null) return;

        if(targetedEnemy.GetComponent<Burning>() != null) targetedEnemy.GetComponent<Burning>().Disable(); // if theres already this status effect, remove it
        targetedEnemy.AddComponent(System.Type.GetType("Burning")); // add the script 
        targetedEnemy.GetComponent<Burning>().Activate(3);
    }
}
