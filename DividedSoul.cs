using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DividedSoul : MonoBehaviour
{
    public void Activate(GameObject enemy)
    {
        if(GetComponent<UnitBattle>().CurrentHealth < enemy.GetComponent<UnitBattle>().CurrentHealth && GetComponent<UnitBattle>().getInjuryStatus())
        {
            GetComponent<UnitBattle>().Healed(7);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[6], transform.position, Quaternion.identity);
        }
        else
        {
            GetComponent<UnitBattle>().CurrentDamage += 5;
            GameObject soulEffect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[6], transform.position, Quaternion.identity);
            soulEffect.GetComponent<SpriteRenderer>().color = new Color(100, 0, 255, 255);
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Divided Soul", false);
    }
}
