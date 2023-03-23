using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnitCreator : MonoBehaviour
{
    [SerializeField] GameObject _ClassButton;
    [SerializeField] TextMeshProUGUI _TitleTextAtTop;
    [SerializeField] private Transform[] _EvenPos;
    [SerializeField] private Transform[] _OddPos;
    [SerializeField] GameObject _NameInputObject;
    [SerializeField] GameObject _NameConfirmObject;
    [SerializeField] Transform _TestUnitSpawnLocation;
    [SerializeField] GameObject _ProtectorPrefab;
    [SerializeField] GameObject _MagePrefab;
    [SerializeField] GameObject _RangerPrefab;
    [SerializeField] GameObject _AssassinPrefab;
    [SerializeField] GameObject _ConjurerPrefab;
    [SerializeField] GameObject _PriestPrefab;

    private int AmountOfOptions;
    private List<int> randomClassIndexs;
    private int ChosenPrimaryClassIndex = -1; // these two indexs are negative 1 by default as a way to null check if they have been chosen
    private int ChosenSecondaryClassIndex = -1;
    private string unitName = "";

    // THIS START METHOD IS FOR TESTING IN THE SCENE
    void Start()
    {
        CreatePlayerUnit(6);
    }
    public void CreatePlayerUnit(int options)
    {
        randomClassIndexs = new List<int>();
        AmountOfOptions = options; // for now the options for secondary and primary are the same
        // set diff title text
        if(ChosenPrimaryClassIndex == -1) _TitleTextAtTop.text = "Choose Primary Class";
        if(ChosenPrimaryClassIndex != -1 && ChosenSecondaryClassIndex == -1) _TitleTextAtTop.text = "Choose Secondary Class";

        // choose primary trait
        // 0 - PROTECTOR
        // 1 - MAGE
        // 2 - RANGER
        // 3 - ASSASSIN
        // 4 - CONJURER
        // 5 - PRIEST
        while(randomClassIndexs.Count < options)
        {
            // roll 0-5 (6 total)
            int currRoll = Random.Range(0, 6);
            if(!randomClassIndexs.Contains(currRoll)) randomClassIndexs.Add(currRoll);
        }
        GenerateButtons(options);
        // choose secondary trait

        // choose name

        // create scriptable object with that data
    }

    void GenerateButtons(int options)
    {
        // if number of options are even, else odd
        if(options % 2 == 0)
        {
            for(int i = 0; i < options; i++)
            {
                GameObject currentButton = Instantiate(_ClassButton, _EvenPos[i].position, Quaternion.identity);
                currentButton.transform.parent = transform;

                currentButton.GetComponent<PlayerUnitButton>().Initialize(randomClassIndexs[i]);
            }
        }
        else
        {
            for(int i = 0; i < options; i++)
            {
                GameObject currentButton = Instantiate(_ClassButton, _OddPos[i].position, Quaternion.identity);
                currentButton.transform.parent = transform;

                currentButton.GetComponent<PlayerUnitButton>().Initialize(randomClassIndexs[i]);
            }
        }
    }

    public void ClassChosen(int chosenIndexClass)
    {
        // setting the class index of currently creating character based on buttons passing index
        if(ChosenPrimaryClassIndex == -1) ChosenPrimaryClassIndex = chosenIndexClass;
        else if(ChosenSecondaryClassIndex == -1) ChosenSecondaryClassIndex = chosenIndexClass;

        //Destroy all current buttons
        GameObject[] currentlyActiveClassButtons = GameObject.FindGameObjectsWithTag("ClassCreatorButton");
        foreach(GameObject button in currentlyActiveClassButtons)
        {
            Destroy(button);
        }

        // check to see if we should prompt again or be done and fully create the character
        if(ChosenPrimaryClassIndex != -1 && ChosenSecondaryClassIndex == -1) CreatePlayerUnit(AmountOfOptions);
        if(ChosenPrimaryClassIndex != -1 && ChosenSecondaryClassIndex != -1) AskForName();
    }

    // now that we have the two classes, we need to ask for a name
    void AskForName()
    {
        _TitleTextAtTop.text = "Name Unit";
        // set active an input field and button
        _NameInputObject.SetActive(true);
        _NameConfirmObject.SetActive(true);
    }

    // this function is called by the input field, updates name to be valid
    public void SetUnitName(string s)
    {
        unitName = s;
        Debug.Log(unitName);
    }

    // this function is called when the button is pressed
    public void NameRecieved()
    {
        if(unitName.Length > 0)
        {
            // deactivate input field and button
            _NameInputObject.SetActive(false);
            _NameConfirmObject.SetActive(false);
            // now finally, create character
            CreateCharacter();
        }
        else
        {
            Debug.Log("not valid name!");
        }
    }

    void CreateCharacter()
    {
        // create character prefab and add it to Player.GetComponent<Player>().addunitToGuild(unit)
        GameObject createdUnit = null; // this is the object we will pass to add to the player
        // first lets create the UnitStats scriptable object to set its stats
        UnitStats thisUnitsStats = createdUnit.GetComponent<UnitStats>();
        thisUnitsStats.name = unitName;
        
        // === BEGINNING OF MASSIVE STAT SETTING SWITCH BLOCKS!! ===
        // now set some stats based on primary class chosen
        switch(ChosenPrimaryClassIndex)
        {
            case 0:
                thisUnitsStats.primaryClass = "Protector";
                // health and damage
                thisUnitsStats.baseDamage = 4;
                thisUnitsStats.maxHealth = 10;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 5;
                thisUnitsStats.critDamage = 5;
                thisUnitsStats.dodgeChance = 3;
                thisUnitsStats.blockChance = 15;
                thisUnitsStats.parryChance = 7;

                createdUnit = Instantiate(_ProtectorPrefab, _TestUnitSpawnLocation.position, Quaternion.identity); // for now the prefab/sprite selected is only based on their primary class. this is likely due to change where and how we instantiate the prefab

                break;
            case 1:
                thisUnitsStats.primaryClass = "Mage";
                // health and damage
                thisUnitsStats.baseDamage = 3;
                thisUnitsStats.maxHealth = 8;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 2;
                thisUnitsStats.critDamage = 3;
                thisUnitsStats.dodgeChance = 2;
                thisUnitsStats.blockChance = 0;
                thisUnitsStats.parryChance = 1;

                createdUnit = Instantiate(_MagePrefab, _TestUnitSpawnLocation.position, Quaternion.identity);
                break;
            case 2:
                thisUnitsStats.primaryClass = "Ranger";
                // health and damage
                thisUnitsStats.baseDamage = 6;
                thisUnitsStats.maxHealth = 8;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 7;
                thisUnitsStats.critDamage = 6;
                thisUnitsStats.dodgeChance = 5;
                thisUnitsStats.blockChance = 3;
                thisUnitsStats.parryChance = 5;

                createdUnit = Instantiate(_RangerPrefab, _TestUnitSpawnLocation.position, Quaternion.identity);
                break;
            case 3:
                thisUnitsStats.primaryClass = "Assassin";
                // health and damage
                thisUnitsStats.baseDamage = 9;
                thisUnitsStats.maxHealth = 5;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 8;
                thisUnitsStats.critDamage = 7;
                thisUnitsStats.dodgeChance = 7;
                thisUnitsStats.blockChance = 1;
                thisUnitsStats.parryChance = 7;

                createdUnit = Instantiate(_AssassinPrefab, _TestUnitSpawnLocation.position, Quaternion.identity);
                break;
            case 4:
                thisUnitsStats.primaryClass = "Conjurer";
                // health and damage
                thisUnitsStats.baseDamage = 3;
                thisUnitsStats.maxHealth = 8;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 6;
                thisUnitsStats.critDamage = 4;
                thisUnitsStats.dodgeChance = 3;
                thisUnitsStats.blockChance = 0;
                thisUnitsStats.parryChance = 1;

                createdUnit = Instantiate(_ConjurerPrefab, _TestUnitSpawnLocation.position, Quaternion.identity);
                break;
            case 5:
                thisUnitsStats.primaryClass = "Priest";
                // health and damage
                thisUnitsStats.baseDamage = 2;
                thisUnitsStats.maxHealth = 9;
                // crit, block, parry, dodge chance
                thisUnitsStats.critChance = 1;
                thisUnitsStats.critDamage = 2;
                thisUnitsStats.dodgeChance = 2;
                thisUnitsStats.blockChance = 0;
                thisUnitsStats.parryChance = 3;

                createdUnit = Instantiate(_PriestPrefab, _TestUnitSpawnLocation.position, Quaternion.identity);
                break;
            default:
                // code block
                break;
        }

        // NOW FOR SUBCLASS

        switch(ChosenSecondaryClassIndex)
        {
            case 0:
                thisUnitsStats.subClass = "Protector";
                // health and damage
                thisUnitsStats.baseDamage += 2;
                thisUnitsStats.maxHealth += 4;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Tank";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Guardian";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Wild Blade";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Rogue";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Dreadnought";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Templar";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case 1:
                thisUnitsStats.subClass = "Mage";
                // health and damage
                thisUnitsStats.baseDamage += 2;
                thisUnitsStats.maxHealth += 2;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Sentinel";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Battle Mage";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Shaman";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Trickster";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Warlock";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Oracle";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case 2:
                thisUnitsStats.subClass = "Ranger";
                // health and damage
                thisUnitsStats.baseDamage += 3;
                thisUnitsStats.maxHealth += 2;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Warden";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Archwizard";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Hunter";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Scout";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Wanderer";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Druid";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case 3:
                thisUnitsStats.subClass = "Assassin";
                // health and damage
                thisUnitsStats.baseDamage += 5;
                thisUnitsStats.maxHealth += 0;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Blade Master";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Arcane Thief";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Duelist";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Ninja";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Cultist";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Holy Blade";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case 4:
                thisUnitsStats.subClass = "Conjurer";
                // health and damage
                thisUnitsStats.baseDamage += 1;
                thisUnitsStats.maxHealth += 3;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Night Shield";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Shadow Caster";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Beast Master";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Shadow Blade";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Summoner";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Shadow Lord";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case 5:
                thisUnitsStats.subClass = "Priest";
                // health and damage
                thisUnitsStats.baseDamage += 0;
                thisUnitsStats.maxHealth += 4;
                // now check what primary class this was to determine the full class name
                switch(ChosenPrimaryClassIndex)
                {
                    case 0:
                        thisUnitsStats.titleClass = "Paladin";
                        break;
                    case 1:
                        thisUnitsStats.titleClass = "Soul Weaver";
                        break;
                    case 2:
                        thisUnitsStats.titleClass = "Soulbow";
                        break;
                    case 3:
                        thisUnitsStats.titleClass = "Acolyte";
                        break;
                    case 4:
                        thisUnitsStats.titleClass = "Necromancer";
                        break;
                    case 5:
                        thisUnitsStats.titleClass = "Divine";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            default:
                // code block
                break;
        }
        // === END OF MASSIVE STAT SETTING SWITCH BLOCKS!! ===

        _TitleTextAtTop.text = thisUnitsStats.titleClass; // temporary to show the full class at top text

        createdUnit.GetComponent<Unit>().SetUnitStats(thisUnitsStats); // this gives the units stats to the unit script.

        GameObject PlayerObject = GameObject.FindGameObjectWithTag("Player");
        PlayerObject.GetComponent<Player>().AddUnitToCrew(createdUnit, 0);
    }
}
