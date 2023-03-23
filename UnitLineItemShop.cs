using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems; // needed for IPointer methods, which is how to get mouse over on UI elements

public class UnitLineItemShop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public int cost;
    public bool isShopItem;
    public bool isInLineup;
    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI LevelAndClassText;
    [SerializeField] TextMeshProUGUI CostText;
    [SerializeField] Image PrimaryClassIcon;
    [SerializeField] Image SubClassIcon;
    [SerializeField] Image RaceIcon;
    [Header("Sprite Components")]
    [SerializeField] Sprite warriorClassIcon;
    [SerializeField] Sprite mageClassIcon;
    [SerializeField] Sprite rangerClassIcon;
    [SerializeField] Sprite assassinClassIcon;
    [SerializeField] Sprite conjurerClassIcon;
    [SerializeField] Sprite priestClassIcon;
    [SerializeField] Sprite HumanRaceIcon;
    [SerializeField] Sprite OrcRaceIcon;
    [SerializeField] Sprite ElfRaceIcon;
    [SerializeField] Sprite UndeadRaceIcon;
    [SerializeField] Sprite DemonRaceIcon;
    [SerializeField] GameObject BuyButton;
    [Header("Sound Components")]
    [SerializeField] private AudioClip BoughtSoldSound;
    [SerializeField] private AudioClip PutInLineupSound;
    [SerializeField] private AudioClip MouseOverShop;
    [Header("Unit Data")]
    private GameObject unit;
    private UnitStats stats;
    [Header("External Scene Component")]
    GameObject UnitCardObject;
    [Header("Drag Variables")]
    bool isBeingDragged = false;
    GameObject collidingUnitButton;
    Vector3 initialPos;
    [Header("Tutorial Components")]
    GameObject TutorialText2;
    GameObject TutorialText3;

    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DayCount == 1)
            {
                TutorialText2 = GameObject.FindGameObjectWithTag("TutorialText2");
                TutorialText3 = GameObject.FindGameObjectWithTag("TutorialText3");
            }
    }
    // Function to set which unit the lineItem is displaying
    public void SetAssociatedUnit(GameObject InputUnit, bool isBuyable)
    {
        initialPos = transform.localPosition;
        // for now, setting the cost as magic number. will need an actual algorithm to determine how much a unit should cost
        switch(InputUnit.GetComponent<UnitStats>().level)
        {
            case 1:
            cost = 7;
            break;
            case 2:
            cost = 10;
            break;
            case 3:
            cost = 12;
            break;
            case 4:
            cost = 14;
            break;
            case 5:
            cost = 25;
            break;
            case 6:
            cost = 28;
            break;
            case 7:
            cost = 30;
            break;
            case 8:
            cost = 42;
            break;
            case 9:
            cost = 45;
            break;
            case 10:
            cost = 50;
            break;
        }
        // set PlayerCardObject (not in nav tho!)
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) UnitCardObject = GameObject.FindGameObjectWithTag("UnitCard");
        if(GameObject.FindGameObjectWithTag("PartyManager") == null && UnitCardObject == null)
        {
            // unit card ob can be null here so lets grab it
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("UnitLineItem"); 
            foreach (GameObject foundObject in taggedObjects) {
                if(foundObject.GetComponent<UnitLineItemShop>().UnitCardObject != null) UnitCardObject = foundObject.GetComponent<UnitLineItemShop>().UnitCardObject; // destroy line item if it isn't a shop item
            }
        }
        // get the unit
        unit = InputUnit;
        // get the stats of the given unit
        stats = unit.GetComponent<UnitStats>();

        // set buyable status
        if(isBuyable) isShopItem = true;
        
        // now that we have the unit, we can set the UI with a function here
        SetUIElements();
    }

    private void SetSpriteIcon()
    {
        // no idea how to make this work, for the composite units
        
    }

    private void SetUIElements()
    {
        // go through each serialized field that is related to UI elements and
        // set them based on the now set unit

        // using MyriadInfo to set color
        MyriadInfo info = new MyriadInfo();
        GetComponent<Image>().color = info.GetPrimaryClassColor(stats.primaryClass);
        
        // set icons of lineitem
        switch(unit.GetComponent<UnitStats>().primaryClass)
        {
            case "Warrior":
                    PrimaryClassIcon.sprite = warriorClassIcon;
                    break;
                case "Mage":
                    PrimaryClassIcon.sprite = mageClassIcon;
                    break;
                case "Ranger":
                    PrimaryClassIcon.sprite = rangerClassIcon;
                    break;
                case "Assassin":
                    PrimaryClassIcon.sprite = assassinClassIcon;
                    break;
                case "Conjurer":
                    PrimaryClassIcon.sprite = conjurerClassIcon;
                    break;
                case "Priest":
                    PrimaryClassIcon.sprite = priestClassIcon;
                    break;
                default:
                    // code block
                    break;
        }
        // NOW FOR SUBCLASS AND RACE
        switch(unit.GetComponent<UnitStats>().subClass)
        {
            case "Warrior":
                    SubClassIcon.sprite = warriorClassIcon;
                    break;
                case "Mage":
                    SubClassIcon.sprite = mageClassIcon;
                    break;
                case "Ranger":
                    SubClassIcon.sprite = rangerClassIcon;
                    break;
                case "Assassin":
                    SubClassIcon.sprite = assassinClassIcon;
                    break;
                case "Conjurer":
                    SubClassIcon.sprite = conjurerClassIcon;
                    break;
                case "Priest":
                    SubClassIcon.sprite = priestClassIcon;
                    break;
                default:
                    // code block
                    break;
        }
        switch(unit.GetComponent<UnitStats>().race)
        {
            case "Human":
                    RaceIcon.sprite = HumanRaceIcon;
                    break;
                case "Orc":
                    RaceIcon.sprite = OrcRaceIcon;
                    break;
                case "Elf":
                    RaceIcon.sprite = ElfRaceIcon;
                    break;
                case "Undead":
                    RaceIcon.sprite = UndeadRaceIcon;
                    break;
                case "Demon":
                    RaceIcon.sprite = DemonRaceIcon;
                    break;
                default:
                    // code block
                    break;
        }
        // ----------------------------------------------------------

        // text setting
        NameText.text = stats.name;
        LevelAndClassText.text = "Level " + stats.level + " " + stats.titleClass;
        CostText.text = "";
        if(GameObject.FindGameObjectWithTag("ShopManager") != null)
        {
            BuyButton.SetActive(true);
            if(isShopItem) CostText.text = "cost: " + cost + "g";
            else
            {
                CostText.text = "Value: " + (cost/2) + "g";
                BuyButton.GetComponentInChildren<TextMeshProUGUI>().text = "SELL";
                if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList.Count == 1) BuyButton.SetActive(false);
            } 
        }
        if(GameObject.FindGameObjectWithTag("PartyManager") != null)
        {
            if(GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitsInParty.Contains(unit))
            {
                CostText.text = "IN LINEUP"; // check if this unit is in the lineup, if so set cost text to IN LINEUP
                isInLineup = true;
            }
            else
            {
                // THIS MEANS A UNIT IS NOT IN LINEUP. IF THATS THE CASE, I WANNA MOVE THE UNIT ITSELF OFFSCREEN
                unit.transform.position = new Vector3(20, 20, 0); // move unit offscreen
            }
        }

        // for now I wont be setting the class icons but that should be done here
        // ---
        // ---
    }

    public void UnitBought()
    {
        // code runs when player clicks on BUY button on this unit
        Player PlayerComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if(isShopItem)
        {
            // PLAYER HAS BOUGHT FROM STORE
            // if player can afford unit... else...
            if(PlayerComponent.credits >= cost)
            {
                SoundManager.instance.PlaySound(BoughtSoldSound);
                PlayerComponent.AddUnitToCrew(unit, cost);  //add unit to crew
                GameObject.FindGameObjectWithTag("GoldText").GetComponent<GoldTextHandler>().ChangeOfGold();
                StartCoroutine(triggerGoAwayAndDestroy());
            }
            else
            {
                Debug.Log("NOOO NOOO YOU DONT HAVE THE MONEYY!!!");
            }
        }
        else
        {
            // PLAYER IS SELLING THEIR UNIT
            if(PlayerComponent.TotalUnitList.Count == 1) return; // unit only has 1 unit, no sell

            SoundManager.instance.PlaySound(BoughtSoldSound);
            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().credits += (cost/2);
            GameObject.FindGameObjectWithTag("GoldText").GetComponent<GoldTextHandler>().ChangeOfGold();

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList.Remove(unit);
            PlayerComponent.RefreshCrewWindow();  //refresh crew window

            StartCoroutine(triggerGoAwayAndDestroy());
        }
    }

    private IEnumerator triggerGoAwayAndDestroy()
    {
        BuyButton.SetActive(false);
        if(!isShopItem) Destroy(unit); // destroy unit
        yield return new WaitForSeconds(.01f);
        Destroy(gameObject); // destroy this
    }

    public void UnitClicked()
    {
        // code runs when player clicks on unit
        if(GameObject.FindGameObjectWithTag("PartyManager") != null) GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitClicked(unit);
    }

    // HOVER OVER TO SEE PLAYER CARD CODE ==
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isBeingDragged)
        {
            // dont want this in nav anymore, so if were in nav dont do it
            if(GameObject.FindGameObjectWithTag("PartyManager") == null)
            {
                UnitCardObject.SetActive(true);
                UnitCardObject.GetComponent<UnitCardManager>().SetUIElements(stats);
                SoundManager.instance.PlaySound(MouseOverShop);
            }
        }
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        // if(GameObject.FindGameObjectWithTag("PartyManager") == null) UnitCardObject.SetActive(false); NOO GOOD NO GOOD
    }

    // click and drag OnPointerUp
    public void OnPointerDown(PointerEventData eventData)
    {
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) UnitCardObject.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<Animator>().SetTrigger("undragging");
        if(GameObject.FindGameObjectWithTag("PartyManager") != null)
        {
            GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().MakePlatformGradientsAppear(false); // line to make grads appear/disappear
            unit.GetComponent<Unit>().ToggleSkillPointObject(false);
        }
        if(isBeingDragged)
        {
            // logic to swap line item unit with unit in lineup
            if(collidingUnitButton != null)
            {
                // if the unit is not already in the party
                if(!GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitsInParty.Contains(unit))
                {
                    GameObject unitToSwitchWith;
                    try
                    {
                        // swap the non party unt (unit in this line item) with the one in party manager list using index from button
                        unitToSwitchWith = GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitsInParty[collidingUnitButton.GetComponent<IndexHolder>().index];
                        // put line item unit to position of switch unit
                        unit.transform.position = unitToSwitchWith.transform.position;
                        // move unitToSwitchWith off screen since its no longer in the party
                        unitToSwitchWith.transform.position = new Vector3(20f, 20f, 0); // vector 3 is arbitrary
                    }
                    catch
                    {
                        unitToSwitchWith = collidingUnitButton;
                        // put line item unit to correct pos using partymanager function
                        GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().PositionUnitCorrectly(unit, collidingUnitButton.GetComponent<IndexHolder>().index);
                    }
                    
                    // finally, set the unit from this line item into the party manager list
                    GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitsInParty[collidingUnitButton.GetComponent<IndexHolder>().index] = unit;
                    // update in lineup text for this lineitem which is now in lineup
                    isInLineup = true;
                    CostText.text = "IN LINEUP";
                    // update the removed unit to not have "in lineup" since its no longer in lineup. Done by looping through all lineitems, finding the one with the unit, and then changing it with an accessor and changer method
                    GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("UnitLineItem");   
                    foreach (GameObject foundObject in taggedObjects) {
                        if(foundObject.GetComponent<UnitLineItemShop>().GetAssociatedUnit() == unitToSwitchWith) foundObject.GetComponent<UnitLineItemShop>().SetCostText(""); // Set cost text to nothing
                    }
                    SoundManager.instance.PlaySound(PutInLineupSound);
                    if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DayCount == 1)
                    {
                        TutorialText2.SetActive(false);
                        TutorialText3.SetActive(true);
                    }
                }
            }

            transform.localPosition = initialPos; // reset pos no matter what (of line item)
        }
        else
        {
            if(GameObject.FindGameObjectWithTag("PartyManager") != null && isBeingDragged == false) GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().UnitClicked(unit);
        }
        isBeingDragged = false;
    }

     public void OnDrag(PointerEventData data)
    {
        // case to prevent dragging shop and in-lineup units
        if(CostText.text == "")
        {
            if(!isBeingDragged)
            {
                GetComponent<Animator>().SetTrigger("dragging");
                GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().MakePlatformGradientsAppear(true); // line to make grads appear/disappear
            }
            isBeingDragged = true;
            transform.position = Input.mousePosition;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "UnitButton")
        {
            Debug.Log("WE ARE IN?");
            collidingUnitButton = other.gameObject;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "UnitButton")
        {
            Debug.Log("WE OUT");
            collidingUnitButton = null;
        }
    }

    // ==== ACCESS UNIT VARIABLE (these are for making IN LINEUP text correctly change)
    public GameObject GetAssociatedUnit()
    {
        return unit;
    }
    public void SetCostText(string input)
    {
        if(input == "") isInLineup = false;
        CostText.text = input;
    }

}
