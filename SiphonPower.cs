using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiphonPower : MonoBehaviour
{
    public float originalDamage;
    public void Activate(GameObject enemy)
    {
        originalDamage = GetComponent<UnitBattle>().CurrentDamage;
        GetComponent<UnitBattle>().CurrentDamage = enemy.GetComponent<UnitBattle>().CurrentDamage;

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Siphon Power", false);

        Vector3 Pos = transform.position + new Vector3(0, .1f, 0);
        GameObject swordEffect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[4], Pos, Quaternion.identity);
        swordEffect.GetComponent<SpriteRenderer>().color = new Color(200, 0, 255, 255);
    }

    public void DeActivate()
    {
        GetComponent<UnitBattle>().CurrentDamage = originalDamage;
    }
}
