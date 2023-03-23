using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiHeal : MonoBehaviour
{
    public int Duration;
    // Unit attached with this script cannot receive healing
    public void Activate(int DurationInTurns)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
        Duration = DurationInTurns;
    }

    void RoundStart()
    {
        Duration--;
        if(Duration == 0 || GetComponent<UnitBattle>().isDead)
        {
            DisableAndDestroy();
        }
    }

    void GameEnd()
    {
        DisableAndDestroy();
    }

    public void DisableAndDestroy()
    {
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
        Destroy(GetComponent<AntiHeal>());
    }
}
