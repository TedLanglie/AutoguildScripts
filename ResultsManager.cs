using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour
{
    [Header("NEEDS TO BE SET SCENE BY SCENE")]
    [SerializeField] bool HasContinueButton;
    [SerializeField] AudioClip LevelUp;
    [SerializeField] AudioClip ReturnBlip;
    /* 
        =======================

        THIS SCRIPT IS ATTACHED TO BATTLEMANAGER OBJECT.
        THE PURPOSE OF THIS SCRIPT IS TO PROCESS THE RESULTS OF THE BATTLE.

        =======================
    */
    [Header("Vars")]
    public int LevelAtWhichDeadUnitsAreDestroyed = 5;
    bool didLose = false;
    [Header("UI Components")]
    [SerializeField] GameObject ResultsScreenObject;
    [SerializeField] GameObject ContinueButton;
    [SerializeField] TextMeshProUGUI ResultText;
    [SerializeField] TextMeshProUGUI GoldText;
    [SerializeField] TextMeshProUGUI XPslot1;
    [SerializeField] TextMeshProUGUI XPslot2;
    [SerializeField] TextMeshProUGUI XPslot3;
    [SerializeField] TextMeshProUGUI XPslot4;
    [SerializeField] TextMeshProUGUI XPslot5;
    [SerializeField] TextMeshProUGUI DeathSlot1;
    [SerializeField] TextMeshProUGUI DeathSlot2;
    [SerializeField] TextMeshProUGUI DeathSlot3;
    [SerializeField] TextMeshProUGUI DeathSlot4;
    [SerializeField] TextMeshProUGUI DeathSlot5;

    public void ReturnToNav()
    {
        // this block is to set all units to notDead status, so that they are not considered dead when going to next battle
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Unit");   
        foreach(GameObject unit in taggedObjects)
        {
            unit.GetComponent<UnitBattle>().isDead = false;
        }
        // -----------

        StartCoroutine(ReturnToNavRoutine());
    }

    private IEnumerator ReturnToNavRoutine()
    {
        SoundManager.instance.PlaySound(ReturnBlip);
        GameObject.FindGameObjectWithTag("MusicBox").GetComponent<MusicBoxFader>().TriggerFadeOut();
        ResultsScreenObject.GetComponent<Animator>().SetTrigger("GoAway");
        GameObject.FindGameObjectWithTag("FooterImage").GetComponent<FooterImage>().ActivateDropDown();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("BattleMiddleRight");
        yield return new WaitForSeconds(1);
        if(didLose) SceneManager.LoadScene("MainMenu");
        else SceneManager.LoadScene("Nav");
    }

    public void ContinueToNext()
    {
        // this block is to set all units to notDead status, so that they are not considered dead when going to next battle
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Unit");   
        foreach(GameObject unit in taggedObjects)
        {
            unit.GetComponent<UnitBattle>().isDead = false;
        }
        // -----------

        string sceneToChangeTo = "";
        string thisScenesName = SceneManager.GetActiveScene().name;
        string nums = "";
        int indexWeCameAcrossFirstNumber = 9999;
        for (int i = 0; i <= thisScenesName.Length - 1; i++)
        {
            if(thisScenesName[i] == '0' || thisScenesName[i] == '1' || thisScenesName[i] == '2' || thisScenesName[i] == '3' || thisScenesName[i] == '4' || thisScenesName[i] == '5' || thisScenesName[i] == '6' || thisScenesName[i] == '7' || thisScenesName[i] == '8' || thisScenesName[i] == '9')
            {
                if(i < indexWeCameAcrossFirstNumber) indexWeCameAcrossFirstNumber = i;
                // we've come across a number, so add it to the nums string
                nums += thisScenesName[i];
            }
        }
        // now, we get it as number
        int numVal = Int32.Parse(nums);
        numVal++;
        // remove nums from string
        for(int i = 0; i < indexWeCameAcrossFirstNumber; i++)
        {
            sceneToChangeTo += thisScenesName[i];
        }

        sceneToChangeTo += numVal;
        
        SceneManager.LoadScene(sceneToChangeTo); // load next scene in dungeon
    }
    
    // method is called by battlemanager when match is over
    // result param: W = Win, L = Loss, T = Tie
    public void MatchOver(char result)
    {
        ResultsScreenObject.SetActive(true);

        switch(result)
        {
            case 'W':
                ResultText.text = "You Won!";
                if(HasContinueButton) ContinueButton.SetActive(true); // can only continue in dungeons if you won
                    break;
            case 'L':
                ResultText.text = "You Lost!";
                didLose = true;
                XPslot1.text = "You made it to day " + GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DayCount + "!";
                DestroyDeadUnitsAndPlayer();
                    break;
            case 'T':
                ResultText.text = "You Tied!";
                    break;
            case 'F':
                ResultText.text = "You Fled!";
                    break;
            default:
                // code block
                break;
        }

        if(result != 'L') GiveXPandGold(); // if didn't flee, get XP and Gold
    }

    // if you lost, and you have units dead, calculate which units should be destroyed
    // a unit should only be destroyed if over level 5, and died in a loss
    void DestroyDeadUnitsAndPlayer()
    {
        foreach(GameObject unit in GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList)
        {
            Destroy(unit); // if unit is dead, DESTROY gameobject
        }
        Destroy(GameObject.FindGameObjectWithTag("Player")); // if unit is dead, DESTROY gameobject
    }

    // now that units are destroyed if lost, finally give all remaining units XP based on the
    // level difference between the units level, and the average of the other teams level
    void GiveXPandGold()
    {
        // XP PORTION ===========
        int AveragePlayerLevel = 0; // will hold the average enemy level
        int PlayerLevelTotal = 0; // will hold the total enemy level
        int AverageEnemyLevel = 0; // will hold the average enemy level
        int EnemyLevelTotal = 0; // will hold the total enemy level
        // this method rn only destroys player units since destroying AI units makes no sense
        List<GameObject> EnemyTeam = GetComponent<BattleManager>().EnemyTeam;
        for(int i = 0; i < EnemyTeam.Count; i++)
        {
            EnemyLevelTotal += EnemyTeam[i].GetComponent<UnitStats>().level;
        }
        AverageEnemyLevel = EnemyLevelTotal / EnemyTeam.Count-1; // calculate average level

        // loop through player units and apply XP accordingly
        List<GameObject> PlayerTeam = GetComponent<BattleManager>().PlayerTeam;
        int numOfUnitsGivenXP = 0;
        bool didAnyUnitLevelUp = false;
        for(int i = 0; i < PlayerTeam.Count; i++)
        {
            if(PlayerTeam[i] == null) continue;
            // This case removes units that died in a loss from gaining XP
            if(PlayerTeam[i].GetComponent<UnitBattle>().isDead && didLose && PlayerTeam[i].GetComponent<UnitStats>().level > LevelAtWhichDeadUnitsAreDestroyed) continue;

            // calc exp to gain
            int XPtoGain = UnityEngine.Random.Range(100, 151);
            // submit gained xp + get if they leveled up
            bool DidPlayerLevelUp = PlayerTeam[i].GetComponent<Unit>().GainXP(XPtoGain);
            if(DidPlayerLevelUp) didAnyUnitLevelUp = true;
            // update TextUI depending on slot of i XPslot1
            switch(numOfUnitsGivenXP)
            {
                case 0:
                    XPslot1.text = "" + PlayerTeam[i].GetComponent<UnitStats>().name + " + " +  XPtoGain + "xp";
                    if(DidPlayerLevelUp)
                    {
                        Vector3 holyPos = PlayerTeam[i].transform.position + new Vector3(0, .02f, 0);
                        if(!PlayerTeam[i].GetComponent<UnitBattle>().isDead) Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], holyPos, Quaternion.identity);
                        XPslot1.text += " LEVEL UP!";
                    }
                    break;
                case 1:
                    XPslot2.text = "" + PlayerTeam[i].GetComponent<UnitStats>().name + " + " +  XPtoGain + "xp";
                    if(DidPlayerLevelUp)
                    {
                        Vector3 holyPos = PlayerTeam[i].transform.position + new Vector3(0, .02f, 0);
                        if(!PlayerTeam[i].GetComponent<UnitBattle>().isDead) Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], holyPos, Quaternion.identity);
                        XPslot2.text += " LEVEL UP!";
                    }
                        break;
                case 2:
                    XPslot3.text = "" + PlayerTeam[i].GetComponent<UnitStats>().name + " + " +  XPtoGain + "xp";
                    if(DidPlayerLevelUp)
                    {
                        Vector3 holyPos = PlayerTeam[i].transform.position + new Vector3(0, .02f, 0);
                        if(!PlayerTeam[i].GetComponent<UnitBattle>().isDead) Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], holyPos, Quaternion.identity);
                        XPslot3.text += " LEVEL UP!";
                    }
                        break;
                case 3:
                    XPslot4.text = "" + PlayerTeam[i].GetComponent<UnitStats>().name + " + " +  XPtoGain + "xp";
                    if(DidPlayerLevelUp)
                    {
                        Vector3 holyPos = PlayerTeam[i].transform.position + new Vector3(0, .02f, 0);
                        if(!PlayerTeam[i].GetComponent<UnitBattle>().isDead) Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], holyPos, Quaternion.identity);
                        XPslot4.text += " LEVEL UP!";
                    }
                        break;
                case 4:
                    XPslot5.text = "" + PlayerTeam[i].GetComponent<UnitStats>().name + " + " +  XPtoGain + "xp";
                    if(DidPlayerLevelUp)
                    {
                        Vector3 holyPos = PlayerTeam[i].transform.position + new Vector3(0, .02f, 0);
                        if(!PlayerTeam[i].GetComponent<UnitBattle>().isDead) Instantiate(GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleEffectsHolder>().Effects[2], holyPos, Quaternion.identity);
                        XPslot5.text += " LEVEL UP!";
                    }
                        break;
                default:
                    // code block
                    break;
            }
            numOfUnitsGivenXP++;
        }

        if(didAnyUnitLevelUp) SoundManager.instance.PlaySound(LevelUp);

        // GOLD PORTION ===========
        // get avg player level
        int numOfNonNullUnits = 0;
        for(int i = 0; i < PlayerTeam.Count; i++)
        {
            if(PlayerTeam[i] == null) continue;
            PlayerLevelTotal += PlayerTeam[i].GetComponent<UnitStats>().level;
            numOfNonNullUnits++;
        }
        AveragePlayerLevel = PlayerLevelTotal / numOfNonNullUnits; // calculate average level

        // - CALCULATE GOLD TO GIVE
        int GoldToGive = UnityEngine.Random.Range(7, 10);

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().credits += GoldToGive;

        // UPDATE UI PORTION TO HAVE CORRECT NUMBERS ==========
        GoldText.text = "Gold gained - " + GoldToGive + "g";
        // need block for updating individual XP gains (OR MAYBE HAS TO BE DONE IN ORIGINAL GAIN XP LOOP? AND THEN HERE WE JUST DELETE XP TEXT LINES NOT USED)
        

    }
}
