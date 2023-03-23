using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBlood : MonoBehaviour
{
    public void Activate(GameObject enemy)
    {
        if(!enemy.GetComponent<UnitBattle>().isDead)
        if(enemy.GetComponent<Bleed>() != null) enemy.GetComponent<Bleed>().DisableAndDestroy(); // if theres already this status effect, remove it
        enemy.AddComponent(System.Type.GetType("Bleed")); // add the script 
        enemy.GetComponent<Bleed>().Activate(3);

        if(enemy.GetComponent<ExplodingBloodEffect>() != null) enemy.GetComponent<ExplodingBloodEffect>().DisableAndDestroy(); // if theres already this status effect, remove it
        enemy.AddComponent(System.Type.GetType("ExplodingBloodEffect")); // add the script 
        enemy.GetComponent<ExplodingBloodEffect>().Activate(3);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Exploding Blood", false);
    }
}
