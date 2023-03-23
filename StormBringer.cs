using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBringer : MonoBehaviour
{
    public void Activate(BattleManager battleManager, int targetedIndex)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;

        if(targetedIndex != 0 && targetedIndex != TargetList.Count-1)
        {
            if(TargetList[targetedIndex-1] != null && !TargetList[targetedIndex-1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[targetedIndex-1].GetComponent<UnitBattle>().Hit(5, false, false, false, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], TargetList[targetedIndex-1].transform.position, Quaternion.identity);
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Storm Bringer", false);
            } 
            if(TargetList[targetedIndex+1] != null && !TargetList[targetedIndex+1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[targetedIndex+1].GetComponent<UnitBattle>().Hit(5, false, false, false, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], TargetList[targetedIndex+1].transform.position, Quaternion.identity);
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Storm Bringer", false);
            } 
        }
        else if(targetedIndex == 0)
        {
            if(TargetList[targetedIndex+1] != null && !TargetList[targetedIndex+1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[targetedIndex+1].GetComponent<UnitBattle>().Hit(5, false, false, false, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], TargetList[targetedIndex+1].transform.position, Quaternion.identity);
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Storm Bringer", false);
            }
        }
        else if(targetedIndex == TargetList.Count-1)
        {
            if(TargetList[targetedIndex-1] != null && !TargetList[targetedIndex-1].GetComponent<UnitBattle>().isDead)
            {
                TargetList[targetedIndex-1].GetComponent<UnitBattle>().Hit(5, false, false, false, false, false, gameObject);
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], TargetList[targetedIndex-1].transform.position, Quaternion.identity);
                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Storm Bringer", false);
            }
        }
    }
}
