using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : MonoBehaviour
{
    public void Activate(GameObject attacker)
    {
        attacker.GetComponent<UnitBattle>().Hit(5, false, false, false, false, false, gameObject); // attack enemy with damage

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Bear Trap", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[8], attacker.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 255, 120, 255);
    }
}
