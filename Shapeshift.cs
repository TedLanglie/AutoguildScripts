using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Shapeshift : MonoBehaviour
{
    /*
        ===================================
        THIS TRAIT IS EXTREMELY BROKEN, CAUSES IMMEDIATE CRASH!!!
        ===================================
    */
    string nameOfTraitToAdd;
    BattleManager battleManager;
    public void Activate(BattleManager _battleManager)
    {
        BattleManager.onGameEnd += GameEnd;
        BattleManager.onGameStart += GameStart;
        battleManager = _battleManager;
    }

    void GameStart()
    {
       // get correct side/list to steal traits from
        List<string> stolenTraits;
        if(GetComponent<UnitBattle>().isPlayer) stolenTraits = battleManager.EnemyTeamAllTraitsList;
        else stolenTraits = battleManager.PlayerTeamAllTraitsList;

        // go through list RANDOMLY and find a trait that player does not have
        int index = UnityEngine.Random.Range(0, stolenTraits.Count-1);
        int initialRoll = index;
        bool foundValidTrait = false;
        // iterate until target found
        while(foundValidTrait == false)
        {
            index++;
            // if we are back at the original target
            if(index == initialRoll) return; // THERE IS NOTHING TO DO WITH TRAIT IF THIS IS TRUE
            // if index out
            if(index == stolenTraits.Count) index = 0;
            // if we dont already have said trait, add it and break loop
            if(!gameObject.GetComponent<UnitStats>().traits.Contains(stolenTraits[index]))
            {
                nameOfTraitToAdd = stolenTraits[index];
                gameObject.AddComponent(Type.GetType(nameOfTraitToAdd)); // add the script 
                foundValidTrait = true;
            }
        }

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Shapeshift", false);
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, nameOfTraitToAdd, false);
        GameObject effect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[13], transform.position, Quaternion.identity);
        effect.GetComponent<SpriteRenderer>().color = new Color(130, 0, 255, 255);
    }

    void GameEnd()
    {
        // remove trait when match is over
        BattleManager.onGameEnd -= GameEnd;
        BattleManager.onGameStart -= GameStart;
        Destroy(GetComponent(System.Type.GetType(nameOfTraitToAdd)));
    }
}

