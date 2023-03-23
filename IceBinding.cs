using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBinding : MonoBehaviour
{
    // applies freeze to a target
    public void Activate(BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject targetedEnemy = battleManager.GetRandomUnit(null, TargetingPlayer);

        if(targetedEnemy.GetComponent<Freeze>() != null) StartCoroutine(targetedEnemy.GetComponent<Freeze>().DestroySelf()); // if theres already this status effect, remove it
        targetedEnemy.AddComponent(System.Type.GetType("Freeze")); // add the script 
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[10], targetedEnemy.transform.position, Quaternion.identity);
        targetedEnemy.GetComponent<Freeze>().Activate(effect);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Ice Binding", false);
    }
}
