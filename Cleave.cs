using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleave : MonoBehaviour
{

    // This is the Cleave trait which deals the players damage to units adjacent to the target when attacking
    
    public void Attack(BattleManager battleManager, int targetIndex, float attackDamage, bool isPlayer)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;

        if(TargetList.Count == 1) return;
        // If the first enemy is being attacked then only Cleave the next index
        if (targetIndex == 0) {
            if(!TargetList[1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[1].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[1].transform.position, Quaternion.identity);
            }
        } 
        // If the last enemy is being attacked then only Cleave the previous index
        else if (targetIndex == (TargetList.Count - 1)) {
            if(!TargetList[TargetList.Count - 2].GetComponent<UnitBattle>().isDead) 
            {
                TargetList[TargetList.Count - 2].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[TargetList.Count - 2].transform.position, Quaternion.identity);
            }
        } 
        // Cleave a random adjacent enemy
        else {
            int enemyIndexToTarget = Random.Range(1, 3); // rolls either 1 or two
            if(enemyIndexToTarget == 1)
            {
                // see if that target is dead, if so hit, if dead, try other
                if(!TargetList[targetIndex - 1].GetComponent<UnitBattle>().isDead)
                {
                    TargetList[targetIndex - 1].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                    Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[targetIndex - 1].transform.position, Quaternion.identity);
                }
                else
                {
                    if(!TargetList[targetIndex + 1].GetComponent<UnitBattle>().isDead)
                    {
                        TargetList[targetIndex + 1].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[targetIndex + 1].transform.position, Quaternion.identity);
                    }
                }
            }
            else
            {
                // see if that target is dead, if so hit, if dead, try other
                if(!TargetList[targetIndex + 1].GetComponent<UnitBattle>().isDead)
                {
                    TargetList[targetIndex + 1].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                    Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[targetIndex + 1].transform.position, Quaternion.identity);
                } 
                else
                {
                    if(!TargetList[targetIndex - 1].GetComponent<UnitBattle>().isDead)
                    {
                        TargetList[targetIndex - 1].GetComponent<UnitBattle>().Hit(attackDamage, false, true, true, false, false, gameObject);
                        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetList[targetIndex - 1].transform.position, Quaternion.identity);
                    }
                }
            }
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Cleave", false);
    }
}