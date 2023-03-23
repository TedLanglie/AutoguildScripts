using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : MonoBehaviour
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
        if(!GetComponent<UnitBattle>().isDead) 
        {
            GetComponent<UnitBattle>().Hit(2, false, false, false, false, false, gameObject);
            Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[0], transform.position, Quaternion.identity);
        }
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
        Destroy(GetComponent<Bleed>());
    }
}
