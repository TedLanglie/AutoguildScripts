using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneShield : MonoBehaviour
{
    // deal 1 damage to all enemies after blocking
    public void Activate(BattleManager battleManager)
    {
        // get correct side/list to damage
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;

        foreach(GameObject unit in TargetList)
        {
            if(unit != null && !unit.GetComponent<UnitBattle>().isDead)
            {
                unit.GetComponent<UnitBattle>().Hit(5, false, true, true, false, false, gameObject); // attack enemy with damage
            }
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Arcane Shield", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[5], transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 5, 255, 255);
    }
}
