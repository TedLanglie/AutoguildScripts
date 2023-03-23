using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judgement : MonoBehaviour
{
    public void Activate(BattleManager battleManager)
    {
        // get correct side/list to target
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;
        float highestAttack = -1;
        GameObject unitToTarget = null;

        foreach(GameObject unit in TargetList)
        {
            if(unit == null) continue;

            if(unit.GetComponent<UnitBattle>().CurrentDamage > highestAttack && !unit.GetComponent<UnitBattle>().isDead)
            {
                highestAttack = unit.GetComponent<UnitBattle>().CurrentDamage;
                unitToTarget = unit;
            }
            else if(unit.GetComponent<UnitBattle>().CurrentDamage == highestAttack && !unit.GetComponent<UnitBattle>().isDead)
            {
                // 50 / 50 roll on whether to switch or not, so that if two have same highest damage, its random between them
                int roll = Random.Range(1, 3);
                if(roll == 1)
                {
                    highestAttack = unit.GetComponent<UnitBattle>().CurrentDamage;
                    unitToTarget = unit;
                }
            }
        }


        // unit to target should never be null at this point, only if I guess all enemies had negative attack...
        if(unitToTarget != null)
        {
            unitToTarget.GetComponent<UnitBattle>().CurrentDamage -= 4;

            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Judgement", false);
            Vector3 Pos = unitToTarget.transform.position + new Vector3(0, .1f, 0);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[4], Pos, Quaternion.identity);
        }
    }
}
