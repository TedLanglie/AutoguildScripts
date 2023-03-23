using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBloodEffect : MonoBehaviour
{
    public int Duration;
    // Unit attached with this script cannot receive healing
    public void Activate(int DurationInTurns)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
        Duration = DurationInTurns;
    }

    void RoundStart()
    {
        Duration--;
        if(Duration == 0 || GetComponent<UnitBattle>().isDead)
        {
            DisableAndDestroy();
        }
    }

    void GameEnd()
    {
        DisableAndDestroy();
    }

    public void Explode(BattleManager battleManager, int indexOfThisUnit)
    {
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[1], transform.position, Quaternion.identity);

        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.PlayerTeam;
        else TargetList = battleManager.EnemyTeam;

        if(TargetList.Count == 1) return; // only 1 person so damaging adjacent is pointless

        if(indexOfThisUnit != 0 && indexOfThisUnit != TargetList.Count-1)
        {
            if(TargetList[indexOfThisUnit-1] != null && !TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().Hit(8, false, false, false, false, false, gameObject);
            if(TargetList[indexOfThisUnit+1] != null && !TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().Hit(8, false, false, false, false, false, gameObject);
        }
        else if(indexOfThisUnit == 0)
        {
            if(TargetList[indexOfThisUnit+1] != null && !TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().Hit(8, false, false, false, false, false, gameObject);
        }
        else if(indexOfThisUnit == TargetList.Count-1)
        {
            if(TargetList[indexOfThisUnit-1] != null && !TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().Hit(8, false, false, false, false, false, gameObject);
        }
    }

    public void DisableAndDestroy()
    {
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
        Destroy(GetComponent<ExplodingBloodEffect>());
    }
}
