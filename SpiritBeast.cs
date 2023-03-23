using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritBeast : MonoBehaviour
{
    public void Activate(float damageOfDyingEnemy)
    {
        GetComponent<UnitBattle>().CurrentDamage += Mathf.Floor(damageOfDyingEnemy / 2);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Spirit Beast", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[12], transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(117, 255, 0, 255);
    }
}
