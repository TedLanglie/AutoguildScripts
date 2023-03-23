using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    public bool shieldIsUp = true;
    public int Duration;
    // Unit attached with this script cannot receive healing
    public void Activate()
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;
    }

    public void DisableShield(float damage, BattleManager battleManager)
    {
        if(shieldIsUp == false) return;
        // disable shield
        shieldIsUp = false;
        Duration = 6;

        bool TargetingPlayer;
        if(GetComponent<UnitBattle>().isPlayer) TargetingPlayer = false;
        else TargetingPlayer = true;

        GameObject targetedEnemy = battleManager.GetRandomUnit(null, TargetingPlayer);

        targetedEnemy.GetComponent<UnitBattle>().Hit(damage, false, true, true, false, false, gameObject); // attack enemy with damage
        Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[9], targetedEnemy.transform.position, Quaternion.identity);

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Deflection", false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[7], transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255, 255);
    }

    void RoundStart()
    {
        if(shieldIsUp == false) Duration--;
        
        if(Duration == 0 || GetComponent<UnitBattle>().isDead) shieldIsUp = true;
    }

    void GameEnd()
    {
        shieldIsUp = true;
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
    }
}
