using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fiend : MonoBehaviour
{
    float DamageDeltToAddToAttack = 0;
    public void Activate(BattleManager battleManager, int indexOfThisUnit)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.PlayerTeam;
        else TargetList = battleManager.EnemyTeam;

        if(TargetList.Count == 1) return;
        
        DamageDeltToAddToAttack = 0;

        if(indexOfThisUnit != 0 && indexOfThisUnit != TargetList.Count-1)
        {
            DamageDeltToAddToAttack += 6;
            if(TargetList[indexOfThisUnit-1] != null && !TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);
            if(TargetList[indexOfThisUnit+1] != null && !TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], TargetList[indexOfThisUnit+1].transform.position, Quaternion.identity);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], TargetList[indexOfThisUnit-1].transform.position, Quaternion.identity);
        }
        else if(indexOfThisUnit == 0)
        {
            DamageDeltToAddToAttack += 3;
            if(TargetList[indexOfThisUnit+1] != null && !TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit+1].GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], TargetList[indexOfThisUnit+1].transform.position, Quaternion.identity);
        }
        else if(indexOfThisUnit == TargetList.Count-1)
        {
            DamageDeltToAddToAttack += 3;
            if(TargetList[indexOfThisUnit-1] != null && !TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().isDead) TargetList[indexOfThisUnit-1].GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], TargetList[indexOfThisUnit-1].transform.position, Quaternion.identity);
        }

        GetComponent<UnitBattle>().CurrentDamage += DamageDeltToAddToAttack;

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Fiend", false);
        GameObject unrelentEffect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[7], transform.position, Quaternion.identity);
        unrelentEffect.GetComponent<SpriteRenderer>().color = new Color(70, 0, 55, 255);
    }

    public void DeActivate()
    {
        GetComponent<UnitBattle>().CurrentDamage -= DamageDeltToAddToAttack;
    }
}
