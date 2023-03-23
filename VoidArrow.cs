using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidArrow : MonoBehaviour
{
    // applies anti heal to target
    public void Activate(GameObject TargetEnemy)
    {
        if(TargetEnemy.GetComponent<AntiHeal>() != null) GetComponent<AntiHeal>().DisableAndDestroy(); // if theres already this status effect, remove it
        TargetEnemy.AddComponent(System.Type.GetType("AntiHeal")); // add the script 
        TargetEnemy.GetComponent<AntiHeal>().Activate(6);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Void Arrow", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], TargetEnemy.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 255, 0, 255);
    }
}
