using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retreat : MonoBehaviour
{
    public int Duration;
    // Unit attached with this script cannot receive healing
    public void Activate(int DurationInTurns)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
        Duration = DurationInTurns;

        GetComponent<UnitBattle>().CurrentParryChance = 100;
        GetComponent<UnitBattle>().CurrentDodgeChance += 10;
        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Retreat", false);
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[14], transform.position, Quaternion.identity);
    }

    void RoundStart()
    {
        Duration--;
        GetComponent<UnitBattle>().CurrentParryChance = 100;
        if(Duration == 0 || GetComponent<UnitBattle>().isDead)
        {
            BattleManager.onRoundStart -= RoundStart;
            BattleManager.onGameEnd -= GameEnd;
            GetComponent<UnitBattle>().CurrentDodgeChance -= 10;
            GetComponent<UnitBattle>().CurrentParryChance = GetComponent<UnitStats>().critChance;
        }
    }

    void GameEnd()
    {
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
        GetComponent<UnitBattle>().CurrentDodgeChance -= 10;
        GetComponent<UnitBattle>().CurrentParryChance = GetComponent<UnitStats>().critChance;
    }
}