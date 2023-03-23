using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAngel : MonoBehaviour
{
    public void Activate(BattleManager battleManager)
    {
        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = true;
        else TargetingPlayer = false;

        GameObject targetedAlly = battleManager.GetRandomUnit(gameObject, TargetingPlayer);
        if(targetedAlly == null) return;

        if(targetedAlly.GetComponent<GuardianAngelEffect>() != null) targetedAlly.GetComponent<GuardianAngelEffect>().DisableAndDestroy(); // if theres already this status effect, remove it
        targetedAlly.AddComponent(System.Type.GetType("GuardianAngelEffect")); // add the script 
        targetedAlly.GetComponent<GuardianAngelEffect>().Activate(4);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Guardian Angel", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[5], targetedAlly.transform.position, Quaternion.identity);
    }
}
