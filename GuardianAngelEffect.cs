using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianAngelEffect : MonoBehaviour
{
    public int Duration;
    public float initialBlockChance;
    // Unit attached with this script cannot receive healing
    public void Activate(int DurationInTurns)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
        Duration = DurationInTurns;

        initialBlockChance = GetComponent<UnitBattle>().CurrentBlockChance;
        GetComponent<UnitBattle>().CurrentBlockChance = 70;
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
        GetComponent<UnitBattle>().CurrentBlockChance = initialBlockChance;
        Destroy(GetComponent<GuardianAngelEffect>());
    }
}
