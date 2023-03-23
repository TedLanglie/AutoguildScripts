using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Execute : MonoBehaviour
{

    // This is the execute trait which deals 3 damage to the enemy with the lowest HP before attacking

    public void Attack(BattleManager battleManager)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;

        // by default we assume the lowest health enemy is the first index
        GameObject targetedEnemy = TargetList[0];
        float lowestHealth = TargetList[0].GetComponent<UnitBattle>().CurrentHealth;

        // loop to determine which enemy has the lowest HP by comparing the current index in the loop to lowest we've seen
        for (int i = 0; i < TargetList.Count; i++) {
            if (TargetList[i].GetComponent<UnitBattle>().CurrentHealth < lowestHealth) {
                if (TargetList[i].GetComponent<UnitBattle>().isDead == false) {
                    targetedEnemy = TargetList[i];
                    lowestHealth = TargetList[i].GetComponent<UnitBattle>().CurrentHealth;
                }
            }
        }

        // attack lowest HP enemy for 3 HP
        targetedEnemy.GetComponent<UnitBattle>().Hit(6, false, false, false, false, false, gameObject); 

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Execute", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[8], targetedEnemy.transform.position, Quaternion.identity);
    }
}