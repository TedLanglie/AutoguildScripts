using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualShot : MonoBehaviour
{
    /* ==================================
        FOR SOME REASON THIS TRAIT CAUSES A
        UNITY CRASHING BUG, PERMENANT LOOP MAYBE?
        SEEMS TO OCCUR WHILE AN ATTACKER WITH THIS TRAIT, KILL THE LAST UNIT
        =================================
    */ 
    public void Activate(BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject firstTarget = battleManager.GetRandomUnit(null, TargetingPlayer);
        GameObject SecondTarget = battleManager.GetRandomUnit(firstTarget, TargetingPlayer);

        if(firstTarget == null || SecondTarget == null) return;

        // from here we have two targets 
        firstTarget.GetComponent<UnitBattle>().Hit(Random.Range(2, 7), false, false, true, false, false, gameObject); // attack enemy with damage
        SecondTarget.GetComponent<UnitBattle>().Hit(Random.Range(2, 7), false, false, true, false, false, gameObject); // attack enemy with damage
        
        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Dual Shot", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], firstTarget.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
        GameObject effect2 = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], SecondTarget.transform.position, Quaternion.identity);
        effect2.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
    }
}
