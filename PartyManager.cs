using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PartyManager : MonoBehaviour
{
    // THESE ARE SERIALIZED FOR NOW BUT WE MIGHT HAVE TO FIND THEM WITH TAG IN FINAL
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject traitSection;
    [SerializeField] GameObject unitCard;
    [SerializeField] TextMeshProUGUI DayText;
    [SerializeField] GameObject Platforms;
    [SerializeField] GameObject[] PlatformGrads;
    [SerializeField] GameObject LineupIsEmptyText;
    // these are probably fine to serialize.. but it could be the same
    [SerializeField] GameObject BackButton;
    [Header("Unit Card Components")]
    [SerializeField] TextMeshProUGUI UnitNameText;
    [SerializeField] TextMeshProUGUI UnitLevelandClassText;
    [SerializeField] TextMeshProUGUI UnitHealthText;
    [SerializeField] TextMeshProUGUI UnitDamageText;
    [SerializeField] TextMeshProUGUI UnitCritChanceText;
    [SerializeField] TextMeshProUGUI UnitCritDamageText;
    [SerializeField] TextMeshProUGUI UnitDodgeChanceText;
    [SerializeField] TextMeshProUGUI UnitBlockChanceText;
    [SerializeField] TextMeshProUGUI UnitParryChanceText;
    [SerializeField] Image unitShownSprite;
    [SerializeField] Image CardImageItself;
    [SerializeField] TextMeshProUGUI titleClassTraitText;
    [SerializeField] Image traitBoxImage;
    [Header("Pos Components")]
    // positions of units on screen
    [SerializeField] Transform pos0;
    [SerializeField] Transform pos1;
    [SerializeField] Transform pos2;
    [SerializeField] Transform pos3;
    [SerializeField] Transform pos4;
    [Header("Party Manager Unit List")]
    public List<GameObject> UnitsInParty = new List<GameObject>();
    [Header("Sound Components")]
    [SerializeField] private AudioClip UnitClickedSound;
    [SerializeField] private AudioClip UnitSwapSound;
    [SerializeField] private AudioClip EmbarkSound;
    // on start, get our list of units for the party, might remove this in final
    void Awake()
    {    
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LineupUnitList.Count >= 1) UnitsInParty = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LineupUnitList;   // get lineup from player
        
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList.Count == 0) return; // dont do anything if player has no units
        //put into correct spots
        for(int i = 0; i< UnitsInParty.Count; i++)
        {
            switch(i)
            {
                case 0:
                if(UnitsInParty[i] != null) UnitsInParty[i].transform.position = pos0.transform.position;
                break;
                case 1:
                if(UnitsInParty[i] != null) UnitsInParty[i].transform.position = pos1.transform.position;
                break;
                case 2:
                if(UnitsInParty[i] != null) UnitsInParty[i].transform.position = pos2.transform.position;
                break;
                case 3:
                if(UnitsInParty[i] != null) UnitsInParty[i].transform.position = pos3.transform.position;
                break;
                case 4:
                if(UnitsInParty[i] != null) UnitsInParty[i].transform.position = pos4.transform.position;
                break;
            default:
                // code block
                break;
            }
        }

        foreach(GameObject unit in GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList)
        {
            unit.GetComponent<Unit>().ToggleSkillPointObject(true);
        }

        // set day text
        int dayNum = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DayCount++;
        DayText.text = "Day: " + (dayNum+1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Navigation>().PartyLocationSet(dayNum+1);
    }

    // wipes the current party list and then adds the new one of units in the scene
    public void ReloadParty()
    {
        // clear units
        UnitsInParty.Clear();
        // first get all units
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Unit");   
        foreach(GameObject unit in taggedObjects)
        {
            UnitsInParty.Add(unit);
        }
        //put into correct spots
        //put into correct spots
        for(int i = 0; i< UnitsInParty.Count; i++)
        {
            switch(i)
            {
                case 0:
                UnitsInParty[i].transform.position = pos0.transform.position;
                break;
                case 1:
                UnitsInParty[i].transform.position = pos1.transform.position;
                break;
                case 2:
                UnitsInParty[i].transform.position = pos2.transform.position;
                break;
                case 3:
                UnitsInParty[i].transform.position = pos3.transform.position;
                break;
                case 4:
                UnitsInParty[i].transform.position = pos4.transform.position;
                break;
            default:
                // code block
                break;
            }
        }
    }


    // HANDLING UI SHOWINGS ETC
    // ----------------------
    // this will fire when a user clicks on the hidden button on top of a unit, it will pass a correct unit index so that we know what unit to show on the canvas UI (FOR CLICKING ON UNITS IN LINEUP)
    public void UnitClicked(int unitIndex)
    {
        if(UnitsInParty[unitIndex] == null) return; // leave case
        UnitsInParty[unitIndex].GetComponent<Unit>().ToggleSkillPointObject(false);
        GameObject unit = UnitsInParty[unitIndex];

        RefreshUnitCanvas(unit);
        //unitShownSprite.sprite = unit.GetComponent<SpriteRenderer>().sprite;

        traitSection.SetActive(true);
        unitCard.SetActive(true);

        // ADDED CODE FROM UPGRADE UNIT
        // wipe all existing trait button objects (prevents stacking)
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("TraitButton");   
        foreach (GameObject foundObject in taggedObjects) {
            Destroy(foundObject); // destroy trait button
        }
        
        unit.GetComponent<TraitTreeManager>().ShowPreviouslyChosenTraits();
        // this will generate traits for the unit if they have unspent points
        if(unit.GetComponent<Unit>().unspentTraitPoints > 0) unit.GetComponent<TraitTreeManager>().InitializeUnit();
        if(unit.GetComponent<Unit>().unspentTraitPoints > 0) unit.GetComponent<TraitTreeManager>().GenerateRow();
    }
    // this will fire when a unit clicks on a unit in their player unit manager line item thingy
    public void UnitClicked(GameObject unit)
    {
        unit.GetComponent<Unit>().ToggleSkillPointObject(false);

        RefreshUnitCanvas(unit);
        //unitShownSprite.sprite = unit.GetComponent<SpriteRenderer>().sprite;

        traitSection.SetActive(true);
        unitCard.SetActive(true);

        // wipe all existing trait button objects (prevents stacking)
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("TraitButton");   
        foreach (GameObject foundObject in taggedObjects) {
            Destroy(foundObject); // destroy trait button object
        }
        
        unit.GetComponent<TraitTreeManager>().ShowPreviouslyChosenTraits();
        // this will generate traits for the unit if they have unspent points
        if(unit.GetComponent<Unit>().unspentTraitPoints > 0) unit.GetComponent<TraitTreeManager>().InitializeUnit();
        if(unit.GetComponent<Unit>().unspentTraitPoints > 0) unit.GetComponent<TraitTreeManager>().GenerateRow();
    }
    // ^^^^^ ABOVE THERE ARE TWO UNIT CLICKED METHODS, POSSIBLE WE SHOULD MERGE INTO ONE CUZ THEY DO SAME THING REALLY, JUST DIFF PARAMS

    // helper method to refresh stats on player card
    void RefreshUnitCanvas(GameObject unit)
    {
        UnitStats unitStats = unit.GetComponent<Unit>().stats;

        unitCard.GetComponent<UnitCardManager>().SetUIElements(unitStats);

        SoundManager.instance.PlaySound(UnitClickedSound);

        // Change color of different images and text
        MyriadInfo info = new MyriadInfo();
        titleClassTraitText.text = unitStats.titleClass;
        titleClassTraitText.color = info.GetUnitsTitleClassColor(unitStats.primaryClass, unitStats.subClass);
        traitBoxImage.color = info.GetUnitsTitleClassColor(unitStats.primaryClass, unitStats.subClass);
        // ----------------------------------------------------------
    }

    // this function will take two units, and swap the indexs of them in the party list along with their positions on the screen
    public void SwapUnitIndex(GameObject unit1, GameObject unit2)
    {
        SoundManager.instance.PlaySound(UnitSwapSound);
        int unit1Index = UnitsInParty.IndexOf(unit1);
        int unit2Index = UnitsInParty.IndexOf(unit2);
        
        // swap them in list
        GameObject tmp = UnitsInParty[unit1Index];
        UnitsInParty[unit1Index] = UnitsInParty[unit2Index];
        UnitsInParty[unit2Index] = tmp;

        // flip their positions as well
        Vector3 tempPosition = UnitsInParty[unit1Index].transform.position;
        UnitsInParty[unit1Index].transform.position = UnitsInParty[unit2Index].transform.position;
        UnitsInParty[unit2Index].transform.position = tempPosition;
    }

    // this function will put a unit into a different empty slot index
    public void SwapUnitWithEmpty(GameObject unit, GameObject EmptyPos)
    {
        SoundManager.instance.PlaySound(UnitSwapSound);
        int indexOfEmpty = EmptyPos.GetComponent<IndexHolder>().index;
        if(UnitsInParty[indexOfEmpty] != null)
        {
            // this block is for if there is actually a unit inside the empty!
            SwapUnitIndex(unit, UnitsInParty[indexOfEmpty]);
            return;
        }
        int unitIndex = UnitsInParty.IndexOf(unit);

        if(indexOfEmpty == unitIndex) return; // this case would mean a unit is trying to swap to its own empty slot. not good so return!
        
        // put unit in list and remove its previous index
        UnitsInParty[indexOfEmpty] = unit;
        UnitsInParty[unitIndex] = null;

        // correct position as well
        unit.transform.position = EmptyPos.transform.position;
    }

    // this function will take the unit out of the lineup, refresh its line item text
    public void SwapUnitOutOfLineup(GameObject unit)
    {
        SoundManager.instance.PlaySound(UnitSwapSound);
        int unitIndex = UnitsInParty.IndexOf(unit);

        UnitsInParty[unitIndex] = null;

        // update the removed unit to not have "in lineup" since its no longer in lineup. Done by looping through all lineitems, finding the one with the unit, and then changing it with an accessor and changer method
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("UnitLineItem");   
        foreach (GameObject foundObject in taggedObjects) {
            if(foundObject.GetComponent<UnitLineItemShop>().GetAssociatedUnit() == unit) foundObject.GetComponent<UnitLineItemShop>().SetCostText(""); // Set cost text to nothing
        }

        // correct position as well
        unit.transform.position = new Vector3(20, 20, 0); // move unit offscreen
    }

    public void PositionUnitCorrectly(GameObject unit, int buttonIndex)
    {
        switch(buttonIndex)
        {
            case 0:
                unit.transform.position = pos0.transform.position;
                    break;
            case 1:
                unit.transform.position = pos1.transform.position;
                    break;
            case 2:
                unit.transform.position = pos2.transform.position;
                    break;
            case 3:
                unit.transform.position = pos3.transform.position;
                    break;
            case 4:
                unit.transform.position = pos4.transform.position;
                    break;
            default:
                // code block
                break;
        }
    }

    public void MakePlatformGradientsAppear(bool trueIfAppear)
    {
        if(trueIfAppear)
        {
            foreach (GameObject grad in PlatformGrads) {
            grad.SetActive(true);
            }
        } else
        {
            foreach (GameObject grad in PlatformGrads) {
            grad.SetActive(false);
            }
        }
    }

    public void GiveLineupToPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().LineupUnitList = UnitsInParty;
    }

    public void LocationButtonClicked(string inputLocation)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Navigation>().ChangeLocation(inputLocation);
    }

    public void EmbarkButtonClicked()
    {
        bool atLeastOneInLineup = false;
        foreach(GameObject unit in UnitsInParty)
        {
            if(unit != null) atLeastOneInLineup = true;
        }
        if(atLeastOneInLineup == false)
        {
            StartCoroutine(LineupEmptyShow());
            return;
        }

        //only do this if we have atleast 1 party member
        StartCoroutine(EmbarkRoutine());
    }

    private IEnumerator LineupEmptyShow()
    {
        LineupIsEmptyText.SetActive(true);
        yield return new WaitForSeconds(4f);
        LineupIsEmptyText.SetActive(false);
    }

    private IEnumerator EmbarkRoutine()
    {
        GameObject.FindGameObjectWithTag("MusicBox").GetComponent<MusicBoxFader>().TriggerFadeOut();
        SoundManager.instance.PlaySound(EmbarkSound);
        foreach(GameObject unit in GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList)
        {
            unit.GetComponent<Unit>().ToggleSkillPointObject(false);
        }
        unitCard.SetActive(false);
        traitSection.SetActive(false);
        Platforms.GetComponent<Animator>().SetTrigger("GoAway"); 
        GameObject.FindGameObjectWithTag("PlayerCrewWindowManager").GetComponent<PlayerCrewWindowManager>().GoAway(); 
        GameObject.FindGameObjectWithTag("FooterImage").GetComponent<FooterImage>().ActivateDropDown();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("MiddleRight");
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Navigation>().EmbarkLocation();
    }
}
