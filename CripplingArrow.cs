using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CripplingArrow : MonoBehaviour
{
    public void Activate(BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject targetedEnemy = battleManager.GetRandomUnit(null, TargetingPlayer);

        targetedEnemy.GetComponent<UnitBattle>().Hit(4, false, false, false, false, false, gameObject); // attack enemy with damage
        targetedEnemy.GetComponent<UnitBattle>().CurrentDamage -= 3;

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Crippling Arrow", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], targetedEnemy.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], targetedEnemy.transform.position, Quaternion.identity);
    }
}
