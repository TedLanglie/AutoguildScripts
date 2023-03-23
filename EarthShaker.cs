using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShaker : MonoBehaviour
{
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
                unit.GetComponent<UnitBattle>().Hit(3, false, true, true, false, false, gameObject); // attack enemy with damage
                Vector3 Pos = unit.transform.position + new Vector3(0, -.1f, 0);
                GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[11], Pos, Quaternion.identity);
                if(!GetComponent<UnitBattle>().isPlayer)
                {
                    Vector3 localFlipScale = effect.transform.localScale; // flip Object
                    localFlipScale.x *= -1;
                    effect.transform.localScale = localFlipScale;
                }
            }
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Earth Shaker", false);
    }
}
