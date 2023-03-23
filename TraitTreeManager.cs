using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class TraitTreeManager : MonoBehaviour
{
    [SerializeField] GameObject traitBoxPrefab; // this prefab is the individual trait
    List<int> SelectedTraits = new List<int>(); // keep track of what traits the unit has
    List<int> RolledTraitIDs = new List<int>(); // keep track of what traits are rolled by the RNG so we dont prompt duplicates
    List<string> CurrentRolledTraits = new List<string>(); // This should be a list of 3 strings, to hold the CURRENT ROLL of traits. this is to prevent rerolling
    public List<Vector3> TraitPositions = new List<Vector3>();
    Dictionary<int, string> traitsIndex;
    private GameObject TheUnitWithThisTraitTree;
    private UnitStats StatsOfTheUnit;
    private int lastSlectedTrait; // this will be used to pass the int index of the trait to the individual trait object
    [Header("Canvas and Components")]
    private GameObject Canvas; // canvas for traits
    private Transform traitPlacement1; // canvas for traits
    private Transform traitPlacement2; // canvas for traits
    private Transform traitPlacement3; // canvas for traits
    private GameObject trait1;
    private GameObject trait2;
    private GameObject trait3;


    // ------ PURPOSE OF SCRIPT:
        /*
            EACH UNIT SHOULD HAVE ONE OF THESE ATTACTHED, IT SHOULD KEEP TRACK OF TRAITS A UNIT HAS
            AS WELL AS GENERATES NEW TRAITS TO DISPLAY TO A CANVAS WHEN PROMPTED
            THIS SCRIPT WILL GENERATE THOSE 3 TRAITS, AND THEN GIVE THEM THEIR TRAIT NAME TO ITS OWN SCRIPT
            THAT SCRIPT IS CALLED IndividualTraitSelect WHEN THAT TRAIT INITIALIZES, IT WILL USE THE NAME IT
            WAS GIVEN BY THIS SCRIPT, TO CHANGE ITS UI ACCORDINGLY. THEN WHEN IT IS CLICKED, THAT SCRIPT
            WILL ALSO ADD THE CORRECT TRAITSCRIPT COMPONENT TO THE UNIT
        */
    // --------------------------
    
    /* ==========================
            TRAIT INDEX:
      (EACH TRAIT NEEDS A UNIQUE INT AS ITS INDEX)
      || CLASS RULES
      Warrior TRAIT ID'S MUST START WITH 1
      MAGE TRAIT ID'S MUST START WITH 2
      RANGER TRAIT ID'S MUST START WITH 3
      ASSASSIN TRAIT ID'S MUST START WITH 4
      CONJURER TRAIT ID'S MUST START WITH 5
      PRIEST TRAIT ID'S MUST START WITH 6
      ||
      TRAITS ASCEND STARTING AT ITS HUNDREDS. FOR EXAMPLE, THE FIRST PRIEST TRAIT IS 600, THEN 601 ETC
    */
    void Awake()
    {
        SceneManager.activeSceneChanged += SetCanvasComponents; // means that every time a scene changes, we will set canvas components accordingly

        // dictionary to hold traitscript name, and index
        MyriadInfo info = new MyriadInfo();
        traitsIndex = info.traitsIndex;
    }

    public void SetCanvasComponents(Scene current, Scene next)
    {
        if(GameObject.FindGameObjectWithTag("UnitCanvas"))
        {
            Canvas = GameObject.FindGameObjectWithTag("UnitCanvas");
            traitPlacement1 = GameObject.FindGameObjectWithTag("PosTrait1").transform;
            traitPlacement2 = GameObject.FindGameObjectWithTag("PosTrait2").transform;
            traitPlacement3 = GameObject.FindGameObjectWithTag("PosTrait3").transform;
        }
    }

    // initialize this TraitTreeManager with a given unit
    public void InitializeUnit()
    {
        TheUnitWithThisTraitTree = gameObject; // setting it as the gameobject this script is attached to for now

        StatsOfTheUnit = TheUnitWithThisTraitTree.GetComponent<Unit>().stats; // get the stats of the unit, we will need this info to generate the correct traits
    }

    // generate a row of 3 traits
    public void GenerateRow()
    {
        // calculate where traits should spawn
        Vector3 offSetYTrait = new Vector3(0, (GetComponent<UnitStats>().traits.Count * 87) *-1, 0);

        Vector3 trait1place = new Vector3(traitPlacement1.position.x, traitPlacement1.position.y + offSetYTrait.y, traitPlacement1.position.z);
        Vector3 trait2place = new Vector3(traitPlacement2.position.x, traitPlacement2.position.y + offSetYTrait.y, traitPlacement2.position.z);
        Vector3 trait3place = new Vector3(traitPlacement3.position.x, traitPlacement3.position.y + offSetYTrait.y, traitPlacement3.position.z);

        // spawn 3 traitbox prefabs
        trait1 = Instantiate(traitBoxPrefab, trait1place, Quaternion.identity, Canvas.transform);
        trait2 = Instantiate(traitBoxPrefab, trait2place, Quaternion.identity, Canvas.transform);
        trait3 = Instantiate(traitBoxPrefab, trait3place, Quaternion.identity, Canvas.transform);

        // here we are checking to see if there are currently rolled traits already for this unit, if there are, then we dont have to generate, we can prompt the ones we have instead
        if(CurrentRolledTraits.Count > 0)
        {
            trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = CurrentRolledTraits[0];
            trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = CurrentRolledTraits[1];
            trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = CurrentRolledTraits[2];
        }
        else
        {
            // ELSE : we have to generate traits!
            // change NameOfTraitScriptToApply variable in those prefabs component in individual script, to a random available trait based on their class
            // FIRST 2 TRAITS ARE GENERATED BASED ON PRIMARY CLASS
            switch(StatsOfTheUnit.primaryClass)
            {
                case "Warrior":
                    //Generate two Warrior traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Warrior");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Warrior");
                    break;
                case "Mage":
                    //Generate two mage traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Mage");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Mage");
                    break;
                case "Ranger":
                    //Generate two Ranger traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Ranger");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Ranger");
                    break;
                case "Assassin":
                    //Generate two assassin traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Assassin");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Assassin");
                    break;
                case "Conjurer":
                    //Generate two conjurer traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Conjurer");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Conjurer");
                    break;
                case "Priest":
                    //Generate two Priest traits
                    trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Priest");
                    trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Priest");
                    break;
                default:
                    // code block
                    break;
            }
            // THIS IS FOR GENERATING SINGLE TRAIT FROM SECONDARY CLASS
            switch(StatsOfTheUnit.subClass)
            {
                case "Warrior":
                    //Generate two Warrior traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Warrior");
                    break;
                case "Mage":
                    //Generate two mage traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Mage");
                    break;
                case "Ranger":
                    //Generate two Ranger traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Ranger");
                    break;
                case "Assassin":
                    //Generate two assassin traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Assassin");
                    break;
                case "Conjurer":
                    //Generate two conjurer traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Conjurer");
                    break;
                case "Priest":
                    //Generate two Priest traits
                    trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetRandomAvailableTrait("Priest");
                    break;
                default:
                    // code block
                    break;
            }

            // after generating, store these as the current roll
            CurrentRolledTraits.Add(trait1.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply);
            CurrentRolledTraits.Add(trait2.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply);
            CurrentRolledTraits.Add(trait3.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply);
        }

        // now initialize the element, so it can set its UI elements to the correct things
        trait1.GetComponent<IndividualTraitSelect>().initializeElement(TheUnitWithThisTraitTree);
        trait2.GetComponent<IndividualTraitSelect>().initializeElement(TheUnitWithThisTraitTree);
        trait3.GetComponent<IndividualTraitSelect>().initializeElement(TheUnitWithThisTraitTree);

        // clear the rolls list, so that unselected traits can be rolled again
        RolledTraitIDs.Clear();
    }

    string GetRandomAvailableTrait(string classOfTrait)
    {
        string returnedTraitName = "";
        int foundTraitIndex = 0;
        bool findingValidTrait = true;

        int traitIndexMin = 0;
        int traitIndexMax = 0;

        switch(classOfTrait)
            {
                case "Warrior":
                    traitIndexMin = 100;
                    traitIndexMax = 107;
                    break;
                case "Mage":
                    traitIndexMin = 200;
                    traitIndexMax = 207;
                    break;
                case "Ranger":
                    traitIndexMin = 300;
                    traitIndexMax = 307;
                    break;
                case "Assassin":
                    traitIndexMin = 400;
                    traitIndexMax = 407;
                    break;
                case "Conjurer":
                    traitIndexMin = 500;
                    traitIndexMax = 507;
                    break;
                case "Priest":
                    traitIndexMin = 600;
                    traitIndexMax = 607;
                    break;
                default:
                    // code block
                    break;
            }

        // loop to find valid trait
        while(findingValidTrait)
        {
            // ROLL BETWEEN ALL VALID TRAITS
            int randomRoll = Random.Range(traitIndexMin, traitIndexMax); // THIS IS RANGE OF VALID TRAIT ID'S

            // CHECK IF WE ALREADY HAVE THE TRAIT
            if(!SelectedTraits.Contains(randomRoll) && !RolledTraitIDs.Contains(randomRoll))
            {
                // IF WE DONT ALREADY HAVE IT, EXIT LOOP
                findingValidTrait = false;

                foundTraitIndex = randomRoll;
            }
        }

        // get the name of the trait from the dictionary
        traitsIndex.TryGetValue(foundTraitIndex, out returnedTraitName);

        // add the trait index to our list of selected traits, so that unit wont be prompted the same traits
        RolledTraitIDs.Add(foundTraitIndex);

        return returnedTraitName; // return that value
    }


    // when user clicks on trait, the IndividualTraitScript will this function, passing the name of the trait so it can be added to the selected traits list
    public void TraitSelected(string SelectedTraitName, GameObject SelectedTraitObject)
    {   
        if(SelectedTraitObject == null)
        {
            // THIS MEANS THE TRAIT SELECT IS BEING GENERATED
            int randomRoll = Random.Range(1, 4); // THIS IS RANGE OF VALID TRAIT PLACEMENTS
            float xPosForTrait = 0;
            if(randomRoll == 1) xPosForTrait = 171.6134f;
            if(randomRoll == 2) xPosForTrait = 69.33588f;
            if(randomRoll == 3) xPosForTrait = -33.60583f;
            Vector3 TraitPos = new Vector3(xPosForTrait + 637.6f, 661 + (GetComponent<UnitStats>().traits.Count * 87) *-1, 0); // this is same as offset Y when generating... yikes again refactor
            TraitPositions.Add(TraitPos);
        } else
        {
            // THIS MEANS TRAITS ARE BEING CHOSEN IN NAV BY PLAYER
            TraitPositions.Add(SelectedTraitObject.transform.position);
            TheUnitWithThisTraitTree.GetComponent<Unit>().unspentTraitPoints--; // REMOVE UNSPENT POINT
        } 

        int myKey = traitsIndex.FirstOrDefault(x => x.Value == SelectedTraitName).Key;

        // destroy all trait button gameobjects so they cant select multiple obv
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("TraitButton");   
        foreach(GameObject traitButton in taggedObjects)
        {
            if(!traitButton.GetComponent<IndividualTraitSelect>().selected) Destroy(traitButton); // only destroy buttons that weren't selected
        }

        // add the selectedtraitID to the selected traits list
        SelectedTraits.Add(myKey);
        Debug.Log("added trait index: " + myKey);

        // also clear the current roll list, so that new traits may be rolled
        CurrentRolledTraits.Clear();

        // check for unspent trait points, if so go again, if we are generating traits, just leave now
        if(SelectedTraitObject == null) return;
        if(TheUnitWithThisTraitTree.GetComponent<Unit>().unspentTraitPoints > 0)
        {
            GenerateRow();
        }
    }

    // this method should instantiate each trait in its correct position
    public void ShowPreviouslyChosenTraits()
    {
        // loop through all trait positions
        for(int i = 0; i < TraitPositions.Count; i++)
        {
            GameObject previousTrait = Instantiate(traitBoxPrefab, TraitPositions[i], Quaternion.identity, Canvas.transform); // instantiate previous trait at certain position
            previousTrait.GetComponent<IndividualTraitSelect>().NameOfTraitScriptToApply = GetComponent<UnitStats>().traits[i]; // style that trait object as current trait
            previousTrait.GetComponent<IndividualTraitSelect>().initializeElement(TheUnitWithThisTraitTree); // initialize element
            previousTrait.GetComponent<IndividualTraitSelect>().selected = true; // set selected to true
            previousTrait.GetComponent<Button>().enabled = false; // disable button so that player cannot click it and gain trait again
            StartCoroutine(previousTrait.GetComponent<IndividualTraitSelect>().SetInfoParentTransform());
        }
    }
}
