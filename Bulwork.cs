using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulwork : MonoBehaviour
{
    // Gain +4 +4 on friendly unit death
    public void Activate()
    {
        UnitBattle.onUnitBlock += UnitBlock;
        BattleManager.onGameEnd += GameEnd;
    }

    void UnitBlock(bool isPlayer, GameObject unit)
    {
        if(isPlayer == GetComponent<UnitBattle>().isPlayer)
        {
            // a friendly unit has blocked, heal it +6
            unit.GetComponent<UnitBattle>().Healed(6);
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Bulwork", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[5], unit.transform.position, Quaternion.identity);
    }

    void GameEnd()
    {
        UnitBattle.onUnitBlock -= UnitBlock;
        BattleManager.onGameEnd -= GameEnd;
    }
}
