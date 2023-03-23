using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrelentingWarrior : MonoBehaviour
{
    bool used = false;
    // need to revoke the damage from unit, and then set isdead to false in unit battle
    void Awake()
    {
        BattleManager.onGameEnd += GameEnd;
    }
    public void Activate(float damage)
    {
        if(used) return;
        GetComponent<UnitBattle>().CurrentHealth += damage;
        used = true;

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Unrelenting Warrior", false);
        GameObject unrelentEffect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], transform.position, Quaternion.identity);
        unrelentEffect.GetComponent<SpriteRenderer>().color = new Color(70, 0, 0, 255);
    }

    void GameEnd()
    {
        used = false;
    }
}
