using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyNova : MonoBehaviour
{
    public void Activate(BattleManager battleManager, int thisUnitsIndex)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.PlayerTeam;
        else TargetList = battleManager.EnemyTeam;

        if(TargetList.Count == 1) return; // only 1 unit on this teams side, soo just leave

        if(thisUnitsIndex != 0 && thisUnitsIndex != TargetList.Count-1)
        {
            bool wasSomebodyHealedHere = false;
            if(TargetList[thisUnitsIndex-1] != null && !TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().isDead)
            {
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], TargetList[thisUnitsIndex-1].transform.position, Quaternion.identity);
                TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().Healed(5);
                wasSomebodyHealedHere = true;
                // activate text effect!
            }
            if(TargetList[thisUnitsIndex+1] != null && !TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().isDead)
            {
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], TargetList[thisUnitsIndex+1].transform.position, Quaternion.identity);
                TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().Healed(5);
                wasSomebodyHealedHere = true;
            }
            if(wasSomebodyHealedHere) GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Holy Nova", false);
        }
        else if(thisUnitsIndex == 0)
        {
            if(TargetList[thisUnitsIndex+1] != null && !TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().isDead)
            {
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], TargetList[thisUnitsIndex+1].transform.position, Quaternion.identity);
                TargetList[thisUnitsIndex+1].GetComponent<UnitBattle>().Healed(5);

                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Holy Nova", false);
            }
        }
        else if(thisUnitsIndex == TargetList.Count-1)
        {
            if(TargetList[thisUnitsIndex-1] != null && !TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().isDead)
            {
                Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], TargetList[thisUnitsIndex-1].transform.position, Quaternion.identity);
                TargetList[thisUnitsIndex-1].GetComponent<UnitBattle>().Healed(5);

                // activate text effect!
                GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Holy Nova", false);
            }
        }
    }

}
