using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanRacialTrait : MonoBehaviour
{
    public void Activate()
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
    }
    void RoundStart()
    {
        if(!GetComponent<UnitBattle>().isDead) GetComponent<UnitBattle>().Healed(1);
    }
    void GameEnd()
    {
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
    }
}
