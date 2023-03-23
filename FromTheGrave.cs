using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromTheGrave : MonoBehaviour
{
    // activated if an enemy parries somebody with this ability
    public void Activate(GameObject Enemy)
    {
        Enemy.GetComponent<UnitBattle>().Hit(10, false, false, false, false, false, gameObject); // attack enemy with damage

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "From The Grave", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[8], Enemy.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 255);
    }
}
