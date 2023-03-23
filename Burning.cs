using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    // reducing currentblock change by 50% and current dodge chance by 20%, deals 1 damage per turn
    // Start is called before the first frame update
    public int Duration;
    // Unit attached with this script cannot receive healing
    public void Activate(int DurationInTurns)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
        Duration = DurationInTurns;

        GetComponent<UnitBattle>().CurrentBlockChance -= 50;
        GetComponent<UnitBattle>().CurrentDodgeChance -= 20;
    }

    void RoundStart()
    {
        if(Duration > 0 && !GetComponent<UnitBattle>().isDead)
        {
            if(!GetComponent<UnitBattle>().isDead) GetComponent<UnitBattle>().Hit(1, false, false, false, false, false, gameObject);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[16], transform.position, Quaternion.identity);
            Duration--;
        }
    }

    void GameEnd()
    {
        Disable();
        Destroy(GetComponent<Burning>());
    }

    public void Disable()
    {
        GetComponent<UnitBattle>().CurrentBlockChance += 50;
        GetComponent<UnitBattle>().CurrentDodgeChance += 20;
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
    }
}
