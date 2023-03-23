using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string GuildName; // Name of the crew player inputs
    public int NumOfUnits;
    public int MaxNumOfUnits;
    public int CombinedLevel;
    public int DayCount = 0;
    public int credits = 0; // the currency in the game (shouldn't start with 0, should start with enough to buy 5 units)
    public List<GameObject> TotalUnitList; // all units this player has
    public List<GameObject> LineupUnitList; // units in the current lineup

    // these methods are for the EMBARK scene at the start of the game
    public void SetPlayerName(string s)
    {
        GuildName = s;
        Debug.Log(GuildName);
    }

    // this is where we should prolly call a diff script to check for profanity, spaces etc
    public bool IsValidInput()
    {
        if(GuildName.Length > 0) return true;

        return false;
    }

    // this method is called to add a unit to the guild
    public void AddUnitToCrew(GameObject unit, int cost)
    {
        // catch, should realistically never trigger
        if(NumOfUnits == MaxNumOfUnits || cost > credits) return;

        unit.AddComponent(System.Type.GetType("BasicDontDestroy"));
        credits -= cost; // paying
        TotalUnitList.Add(unit);
        Debug.Log("New unit added!");

        // now update a crew list if one exists
        if(GameObject.FindGameObjectWithTag("PlayerCrewWindowManager") == null) return; // nothing else to do if this object isnt here
        RefreshCrewWindow();
    }

    public void RefreshCrewWindow()
    {
        if(GameObject.FindGameObjectWithTag("PlayerCrewWindowManager").activeInHierarchy) GameObject.FindGameObjectWithTag("PlayerCrewWindowManager").GetComponent<PlayerCrewWindowManager>().RefreshPlayerUnitsList();
    }

    public int getHighestLevelOfUnitsPlayerHas()
    {
        int highestLevel = 0;
        foreach(GameObject unit in TotalUnitList)
        {
            if(unit.GetComponent<UnitStats>().level > highestLevel) highestLevel = unit.GetComponent<UnitStats>().level;
        }
        return highestLevel;
    }
}
