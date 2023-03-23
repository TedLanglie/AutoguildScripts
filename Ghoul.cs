using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghoul : MonoBehaviour
{
    // Gain +4 +4 on friendly unit death
    public void Activate()
    {
        UnitBattle.onUnitDeath += UnitDeath;
        BattleManager.onGameEnd += GameEnd;
    }

    void UnitDeath(bool isPlayer, GameObject unit)
    {
        if(unit != gameObject && isPlayer == GetComponent<UnitBattle>().isPlayer)
        {
            // a friendly unit has died, that is not the player with ghoul attached
            GetComponent<UnitBattle>().CurrentHealth += 8;
            GetComponent<UnitBattle>().CurrentDamage += 8;
            // activate text effect!
            GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Ghoul", false);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[1], transform.position, Quaternion.identity);
        }
    }

    void GameEnd()
    {
        UnitBattle.onUnitDeath -= UnitDeath;
        BattleManager.onGameEnd -= GameEnd;
    }
}
