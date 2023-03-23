using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificerOfChaos : MonoBehaviour
{
    BattleManager battleManager;
    public void Activate(BattleManager _battleManager)
    {
        battleManager = _battleManager;
        BattleManager.onGameStart += OnGameStart;
    }

    void OnGameStart()
    {
        // TODO: SCRIPT IS BUGGED WITH NEW HEALTH BARS, NEEDS FIX!!!
        // get correct side/list to swap
        List<GameObject> TargetList;
        if(GetComponent<UnitBattle>().isPlayer) TargetList = battleManager.EnemyTeam;
        else TargetList = battleManager.PlayerTeam;

        // if theres only 1 unit on the other team, return
        if(TargetList.Count < 2) return;

        // swap positions of end units
        Vector3 tempPos;
        tempPos = TargetList[0].transform.position; // hold pos of 1 unit
        TargetList[0].transform.position = TargetList[TargetList.Count-1].transform.position; // swap 1 unit with end unit
        TargetList[TargetList.Count-1].transform.position = tempPos; // swap end unit with tempPos (1 pos)

        // Find Healthbars accociated with unit
        GameObject firstUnitHeatlhbar = null;
        GameObject lastUnitHeatlhbar = null;
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("HealthBar");   
        foreach(GameObject healthBar in taggedObjects)
        {
            if(healthBar.GetComponent<HealthBar>().getAssignedUnit() == TargetList[0]) firstUnitHeatlhbar = healthBar.transform.parent.gameObject;
            if(healthBar.GetComponent<HealthBar>().getAssignedUnit() == TargetList[TargetList.Count-1]) lastUnitHeatlhbar = healthBar.transform.parent.gameObject;
        }

        // swap positions of the health bars
        tempPos = firstUnitHeatlhbar.transform.position; // hold pos of 1 unit
        firstUnitHeatlhbar.transform.position = lastUnitHeatlhbar.transform.position; // swap 1 unit with end unit
        lastUnitHeatlhbar.transform.position = tempPos; // swap end unit with tempPos (1 pos)

        // swap the units in the list in battlemanager
        GameObject temp = TargetList[0];
        TargetList[0] = TargetList[TargetList.Count-1];
        TargetList[TargetList.Count-1] = temp;

        // reduce their attack
        TargetList[0].GetComponent<UnitBattle>().CurrentDamage -= 3;
        TargetList[TargetList.Count-1].GetComponent<UnitBattle>().CurrentDamage -= 3;

        BattleManager.onGameStart -= OnGameStart; // unsubscribe from event

        // activate text effect!
        GetComponent<StatusNumbersEffect>().ActivateAmount(-1, "Artificer Of Chaos", false);
        GameObject unrelentEffect = Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[7], transform.position, Quaternion.identity);
        unrelentEffect.GetComponent<SpriteRenderer>().color = new Color(70, 0, 55, 255);
    }
}
