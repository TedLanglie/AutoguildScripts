using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominatingForce : MonoBehaviour
{
    // Start is called before the first frame update
    public bool DomForceBlock(GameObject Attacker)
    {
        if(Attacker.GetComponent<UnitBattle>().CurrentDamage < GetComponent<UnitBattle>().CurrentDamage)
        {
            GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[5], transform.position, Quaternion.identity);
            effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 255);
            return true;
        }
        else return false;
    }
}
