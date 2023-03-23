using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyBell : MonoBehaviour
{
    public void Activate(float damageDelt, BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = true;
        else TargetingPlayer = false;

        GameObject targetedAlly = battleManager.GetRandomInjuredUnit(gameObject, TargetingPlayer);

        if(targetedAlly == null) return;

        targetedAlly.GetComponent<UnitBattle>().Healed(Mathf.Floor(damageDelt / 2)); // heal ally 1/2 of damage

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Holy Bell", false);
        Vector3 holyPos = targetedAlly.transform.position + new Vector3(0, .02f, 0);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[3], holyPos, Quaternion.identity);
    }
}
