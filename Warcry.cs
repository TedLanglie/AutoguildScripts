using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warcry : MonoBehaviour
{
    // give adjacent units +2 attack
    public void Activate(BattleManager battleManager, int thisUnitsIndex)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.PlayerTeam;
        else TargetList = battleManager.EnemyTeam;

        if(TargetList.Count == 1) return;

        if(thisUnitsIndex != 0 && thisUnitsIndex != TargetList.Count-1)
        {
            bool weAddedAttacktoSomebody = false;
            if(TargetList[thisUnitsIndex-1] != null && !TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().CurrentDamage +=3;
                weAddedAttacktoSomebody = true;
            } 
            if(TargetList[thisUnitsIndex+1] != null && !TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().CurrentDamage +=3;
                weAddedAttacktoSomebody = true;
            }
            if(weAddedAttacktoSomebody)
            {
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Warcry", false);
                GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], transform.position, Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            }
        }
        else if(thisUnitsIndex == 0)
        {
            if(TargetList[thisUnitsIndex+1] != null && !TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().CurrentDamage +=3;
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Warcry", false);
                GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], transform.position, Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            }
        }
        else if(thisUnitsIndex == TargetList.Count-1)
        {
            if(TargetList[thisUnitsIndex-1] != null && !TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().CurrentDamage +=3;
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Warcry", false);
                GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], transform.position, Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            }
        }
    }
}
