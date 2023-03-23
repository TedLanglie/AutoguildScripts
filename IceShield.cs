using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShield : MonoBehaviour
{
    // applies freeze to a target
    public void Activate(GameObject attacker)
    {
        if(attacker.GetComponent<Freeze>() != null) StartCoroutine(attacker.GetComponent<Freeze>().DestroySelf()); // if theres already this status effect, remove it
        attacker.AddComponent(System.Type.GetType("Freeze")); // add the script 
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[10], attacker.transform.position, Quaternion.identity);
        attacker.GetComponent<Freeze>().Activate(effect);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Ice Binding", false);
    }
}
