using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headshot : MonoBehaviour
{
    // permanently gain +5 crit damage. You're critical hits also make enemies bleed for 3 turns;
    void Awake()
    {
        GetComponent<UnitStats>().critDamage += 7;
    }
    public void Activate(GameObject enemy)
    {
        if(enemy.GetComponent<Bleed>() != null) enemy.GetComponent<Bleed>().DisableAndDestroy(); // if theres already this status effect, remove it
        enemy.AddComponent(System.Type.GetType("Bleed")); // add the script 
        enemy.GetComponent<Bleed>().Activate(3);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Headshot", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], enemy.transform.position, Quaternion.identity);
    }
}
