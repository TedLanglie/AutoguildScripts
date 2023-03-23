using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonRacialTrait : MonoBehaviour
{
    public void Activate(BattleManager battleManager)
    {
        // damage self
        GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);

        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.PlayerTeam;
        else TargetList = battleManager.EnemyTeam;

        // calculate damage to add based on injury status of team
        int DamageToAddToAttack = 0;
        foreach(GameObject unit in TargetList)
        {
            if(unit != null && unit.GetComponent<UnitBattle>().getInjuryStatus()) DamageToAddToAttack++;
        }

        // add attack based on calculation
        GetComponent<UnitBattle>().CurrentDamage += DamageToAddToAttack;
    }
}
