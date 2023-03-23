using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    GameObject currentEffect;
    // Unit attached with this script cannot receive healing
    public void Activate(GameObject FreezeEffect)
    {
        BattleManager.onRoundStart += RoundStart;
        BattleManager.onGameEnd += GameEnd;

        currentEffect = FreezeEffect;
    }

    void RoundStart()
    {
        if(GetComponent<UnitBattle>().isDead) StartCoroutine(DestroySelf());
    }

    void GameEnd()
    {
        StartCoroutine(DestroySelf());
    }

    public IEnumerator DestroySelf()
    {
        BattleManager.onRoundStart -= RoundStart;
        BattleManager.onGameEnd -= GameEnd;
        currentEffect.GetComponent<Animator>().SetTrigger("GoAway");
        yield return new WaitForSeconds(.4f);
        Destroy(currentEffect);
        Destroy(GetComponent<Freeze>());
    }
}
