using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Unit : MonoBehaviour
{
    public UnitStats stats;
    [SerializeField] GameObject SkillPointText;
    GameObject unitInfoHUD;
    GameObject unitInfoHUDTitleClassText;
    GameObject unitInfoHUDNameText;
    GameObject unitInfoHUDLevelText;
    public bool isGM = false;
    [Header("Leveling and XP")]
    public int unspentTraitPoints;
    int LevelCap = 10;
    public int currentXP;
    private int XPforNextLevel;
    void Awake()
    {
        stats = gameObject.GetComponent<UnitStats>();

        // XP variables, maybe shouldn't be done in awake
        currentXP = 0;
        XPforNextLevel = 200;
    }
    
    void Start()
    {
        // this loops through all children of object to find the HUD object
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "UnitInfoHUD") unitInfoHUD = child.gameObject; // BASE GAMEOBJECT, THIS IS WHAT WE TURN OFF AND ON
        }
        // this loops through all children of unitinfo to find units info
        foreach (Transform child in unitInfoHUD.transform)
        {
            if (child.tag == "UnitInfoHUDTitleClassText") unitInfoHUDTitleClassText = child.gameObject; // this is for showing title class
            if (child.tag == "UnitInfoHUDNameText") unitInfoHUDNameText = child.gameObject; // this is for showing name
            if (child.tag == "UnitInfoHUDLevelText") unitInfoHUDLevelText = child.gameObject; // this is for showing level
        }

        // setting values of the text (this will need to be refreshed when a units name is changed, or when their level increases)
        unitInfoHUDTitleClassText.GetComponent<TextMeshPro>().text = stats.titleClass;
        unitInfoHUDNameText.GetComponent<TextMeshPro>().text = stats.name;
        unitInfoHUDLevelText.GetComponent<TextMeshPro>().text = "Level " + stats.level;
    }

    // ----- XP SECTION -----
    public bool GainXP(int GainedXP)
    {
        int currentLevelPlus1 = stats.level + 1;
        bool leveledUp = false;
        currentXP += GainedXP;
        if(currentXP >= XPforNextLevel && stats.level < LevelCap)
        {
            leveledUp = true;
            LevelUp();
            XPforNextLevel = currentXP + 200; // for now you level up every 500 XP, but there should be a probably more indepth leveling numbers
        } 
        return leveledUp;
    }

    void LevelUp()
    {
        stats.level++;
        if(stats.level % 2 == 0) unspentTraitPoints++; // only gain traits on even levels now
        unitInfoHUDLevelText.GetComponent<TextMeshPro>().text = "Level " + stats.level; // set new level text for info HUD

        switch(stats.subClass)
        {
            case "Warrior":
                stats.maxHealth += 5; // adding stats
                stats.baseDamage += 2;
                break;
            case "Mage":
                stats.maxHealth += 3; // adding stats
                stats.baseDamage += 3;
                break;
            case "Ranger":
                stats.maxHealth += 5; // adding stats
                stats.baseDamage += 2;
                break;
            case "Assassin":
                stats.maxHealth += 3; // adding stats
                stats.baseDamage += 3;
                break;
            case "Conjurer":
                stats.maxHealth += 5; // adding stats
                stats.baseDamage += 2;
                break;
            case "Priest":
                stats.maxHealth += 6; // adding stats
                stats.baseDamage += 1;
                break;
            default:
                // code block
                break;
        }
    }
    // ----------------------

    // this is for hovering to see stats ---------
    void OnMouseOver()
    {
        //If your mouse hovers over the GameObject with the script attached, show Unit Info HUD
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) return; // to prevent doing this outside of nav scene
        unitInfoHUD.SetActive(true);
    }

    void OnMouseExit()
    {
        //The mouse is no longer hovering over the GameObject so remove the Unit Info HUD
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) return; // to prevent doing this outside of nav scene
        unitInfoHUD.SetActive(false);
    }

    // set stats (deprecated?)
    public void SetUnitStats(UnitStats inputStats)
    {
        stats = inputStats;
    }

    public void SetUnitName(string s)
    {
        stats.name = s;
        unitInfoHUDNameText.GetComponent<TextMeshPro>().text = stats.name;
        Debug.Log(stats.name);
    }

    public void ToggleSkillPointObject(bool on)
    {
        if(on && unspentTraitPoints > 0) SkillPointText.SetActive(true);
        else SkillPointText.SetActive(false);
    }
}
