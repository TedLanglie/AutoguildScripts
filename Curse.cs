using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curse : MonoBehaviour
{
    public void Activate(GameObject enemy)
    {
        if(enemy.GetComponent<CurseEffect>() == null) return; // activate if it isn't already cursed (doesnt stack)
        enemy.AddComponent(System.Type.GetType("CurseEffect"));

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Curse", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[13], enemy.transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(130, 0, 255, 255);
    }
}
