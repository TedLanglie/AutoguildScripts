using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conquerer : MonoBehaviour
{
    public void Activate()
    {
        GetComponent<UnitBattle>().CurrentDamage += 2;

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Conquerer", false);
        Vector3 Pos = transform.position + new Vector3(0, .1f, 0);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[13], transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(255, 0, 20, 255);
    }
}
