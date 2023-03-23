using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerateUnit : MonoBehaviour
{
    [Header("Unit Prefab Components")]
    [SerializeField] GameObject UnitPrefab; // prefab of a basic unit, we can change the stats in this script
    [Header("String Arrays")]
    private string[] HumanNamesArray = {"Olaf", "Jarvin", "Torgrim", "Ana", "Felice", "Gordon", "Sander", "Yui", "Freya", "Fenix", "Yosef", 
    "Mikasa", "Armin", "Eren", "Levi", "Angel", "Marticus", "Milo", "Noah", "Olmar", "Thors", "Nefer", "Liam", "Goljar", "Goldron", "Peter", "Zander",
    "Mark", "Andrew", "Vano", "Neptune", "Gillie", "Rena", "Theodosia", "Lerae", "Kate", "Ylva", "Orion", "John", "Leana", "Alexandria", "Adrian", "Deman", "Oliver"};
    private string[] OrcNamesArray = {"Drek'thar", "Guldon", "Raltan", "Sulmar", "Yulta", "Vrathen", "Kulmar", "Simar", "Keth'dral", "Ardiel", "Dradon", "Ortock", "Ramahn",
    "Rulba", "Ruul", "Garma", "Kral", "Kethma", "Rargar", "Relena", "Grothbar", "Oath'tar", "Freth", "Drakmar", "Sundar", "Kiortha", "Dregmar", "Dethca", "Drogthar", "Koraxar",
    "Drok'mar", "Rutilda", "Drogfar", "Ragnar", "Vargash", "Barga", "Ruthgar", "Kalistar", "Ronchala", "Goldrath", "Rakthar", "Zindare", "Kirantha", "Gonchala"};
    private string[] ElfNamesArray = {"Lunari", "Suni", "Rakesh", "Simandre", "Yulva", "Namari", "Aetish", "Leonari", "Zione", "Ariendal", "Sylvia", "Semira", "Lethani",
    "Zigesh", "Runari", "Fumishi", "Lanishi", "Rena", "Falandri", "Samori", "Lenama", "Quethas", "Yuna", "Nier", "Sunari", "Dolna", "Senama", "Decinia", "Toldera", "Dremath",
    "Lamari", "Zentana", "Ilene", "Marsha", "Valentis", "Luraima", "Yoroi", "Subo", "Zetsubo", "Alianda", "Nameria", "Underia", "Loum", "Sonamari"};
    private string[] UndeadNamesArray = {"Arthus", "Drog", "Bog", "Zizeque", "Lukram", "Sagma", "Torlith", "Atlent", "Vuran", "Sylvan", "Krakon", "Drethloc", "Dromang",
    "Baterna", "Ruthda", "Shamdar", "Malekith", "Zith", "Falandra", "Alum", "Podar", "Garmon", "Vordan", "Lordron", "Lothric", "Suldam", "Dromtha", "Gostric", "Kalthu", "Jurax",
    "Galmata", "Ruul", "Viorde", "Darsha", "Gathulu", "Vordie", "Chogata", "Dethari", "Skuldama", "Krethlar", "Loven", "Skartha", "Latvan", "Lathru"};
    private string[] DemonNamesArray = {"Dorgrei", "Zetsu", "Imandi", "Kilza", "Benthy", "Zildi", "Kurapika", "Zelt", "Molron", "Isidro", "Tiel", "Dreklathar", "Fuim",
    "Tilre", "Uotum", "Sunla", "Meldry", "Arony", "Zenlia", "Rektha", "Zimathia", "Aldemia", "Zopondy", "Frelyi", "Zary", "Alvuni", "Avali", "Nightar", "Korreli", "Hellvat",
    "Akuma", "Delvish", "Grubs", "Vein", "Zerny", "Apondy", "Rether", "Nether", "Luri", "Zulra", "Artery", "Vry", "Lurunai", "Zumkuma"};

    private string[] ClassNamesArray = {"Warrior", "Mage", "Ranger", "Assassin", "Conjurer", "Priest"};
    private string[] RaceNamesArray = {"Human", "Orc", "Elf", "Undead", "Demon"};
    
    // if level = -1, it becomes a random level!
    public GameObject GenerateRandomUnit(int level, bool giveUnitTraits, string race, string primaryClass, string subClass)
    {
        // first, generate a basic unit prefab (its position should be changed to be offscreen)
        GameObject returningUnit = Instantiate(UnitPrefab, transform.position, Quaternion.identity);
        // im pretty sure, this isn't COPYING the stats, just dont want so many GetComponent calls
        UnitStats stats = returningUnit.GetComponent<UnitStats>();
        
        stats.level = level;
        if(primaryClass == null) stats.primaryClass = ClassNamesArray[UnityEngine.Random.Range(0, ClassNamesArray.Length)];
        else stats.primaryClass = primaryClass;
        if(subClass == null) stats.subClass = ClassNamesArray[UnityEngine.Random.Range(0, ClassNamesArray.Length)];
        else stats.subClass = subClass;
        if(race == null) stats.race = RaceNamesArray[UnityEngine.Random.Range(0, RaceNamesArray.Length)];
        else stats.race = race;
        MyriadInfo info = new MyriadInfo();
        stats.titleClass = info.GetTitleClass(stats.primaryClass, stats.subClass);
        stats.name = GenerateName(stats.race);

        SetUnitStatsBasedOnLevel(stats);
        SetUnitSprites(stats.race, stats.primaryClass, returningUnit);

        GiveUnitRacialTrait(returningUnit); // apply racial trait to unit
        // give class traits, or trait points based on bool
        if(giveUnitTraits)
        {
            GiveUnitClassTraits(returningUnit, level, stats.primaryClass, stats.subClass);
        }
        else
        {
            returningUnit.GetComponent<Unit>().unspentTraitPoints = level-1; // FOR NOW THIS IS JUST LEVEL, BUT I DONT THINK YOU SHOULD GET A TRAIT POINT EVERY LEVEL, SO THIS SHOULD BE CHANGED LATER
        }
        return returningUnit; // at the end, return the unit
    }

    // give unit its racial trait based on its race
    void GiveUnitRacialTrait(GameObject unit)
    {
        string traitToApply = unit.GetComponent<UnitStats>().race + "RacialTrait";
        unit.AddComponent(System.Type.GetType(traitToApply)); // add the script 
    }

    // generates a random string name
    string GenerateName(string race)
    {
        string nameToReturn = "";
        switch(race)
        {
            case "Human":
                nameToReturn = HumanNamesArray[UnityEngine.Random.Range(0, HumanNamesArray.Length)];
                break;
            case "Orc":
                nameToReturn = OrcNamesArray[UnityEngine.Random.Range(0, OrcNamesArray.Length)];
                break;
            case "Elf":
                nameToReturn = ElfNamesArray[UnityEngine.Random.Range(0, ElfNamesArray.Length)];
                break;
            case "Undead":
                nameToReturn = UndeadNamesArray[UnityEngine.Random.Range(0, UndeadNamesArray.Length)];
                break;
            case "Demon":
                nameToReturn = DemonNamesArray[UnityEngine.Random.Range(0, DemonNamesArray.Length)];
                break;
        }
        return nameToReturn;
    }

    public void SetUnitStatsBasedOnLevel(UnitStats stats)
    {
        // finally, set the SPRITE (currently based on Primary Class), and the units NUMERICAL DATA (health, damage etc)
        switch(stats.primaryClass)
        {
            case "Warrior":
                // unit data
                stats.maxHealth = 18;
                stats.baseDamage = 5;
                stats.critChance = 5;
                stats.critDamage = 5;
                stats.dodgeChance = 2;
                stats.blockChance = 10;
                stats.parryChance = 3;
                break;
            case "Mage":
                // unit data
                stats.maxHealth = 15;
                stats.baseDamage = 6;
                stats.critChance = 5;
                stats.critDamage = 5;
                stats.dodgeChance = 4;
                stats.blockChance = 5;
                stats.parryChance = 7;
                break;
            case "Ranger":
                // unit data
                stats.maxHealth = 18;
                stats.baseDamage = 5;
                stats.critChance = 8;
                stats.critDamage = 7;
                stats.dodgeChance = 5;
                stats.blockChance = 5;
                stats.parryChance = 4;
                break;
            case "Assassin":
                // unit data
                stats.maxHealth = 13;
                stats.baseDamage = 7;
                stats.critChance = 9;
                stats.critDamage = 6;
                stats.dodgeChance = 5;
                stats.blockChance = 4;
                stats.parryChance = 9;
                break;
            case "Conjurer":
                // unit data
                stats.maxHealth = 15;
                stats.baseDamage = 6;
                stats.critChance = 5;
                stats.critDamage = 7;
                stats.dodgeChance = 4;
                stats.blockChance = 7;
                stats.parryChance = 7;
                break;
            case "Priest":
                // unit data
                stats.maxHealth = 20;
                stats.baseDamage = 4;
                stats.critChance = 5;
                stats.critDamage = 5;
                stats.dodgeChance = 7;
                stats.blockChance = 10;
                stats.parryChance = 6;
                break;
            default:
                // code block
                break;
        }

        switch(stats.subClass)
        {
            case "Warrior":
                stats.maxHealth += (5 * (stats.level - 1)); // adding stats
                stats.baseDamage += (2 * (stats.level - 1));
                break;
            case "Mage":
                stats.maxHealth += (3 * (stats.level - 1)); // adding stats
                stats.baseDamage += (3 * (stats.level - 1));
                break;
            case "Ranger":
                stats.maxHealth += (5 * (stats.level - 1)); // adding stats
                stats.baseDamage += (2 * (stats.level - 1));
                break;
            case "Assassin":
                stats.maxHealth += (3 * (stats.level - 1)); // adding stats
                stats.baseDamage += (3 * (stats.level - 1));
                break;
            case "Conjurer":
                stats.maxHealth += (5 * (stats.level - 1)); // adding stats
                stats.baseDamage += (2 * (stats.level - 1));
                break;
            case "Priest":
                stats.maxHealth += (6 * (stats.level - 1)); // adding stats
                stats.baseDamage += (1 * (stats.level - 1));
                break;
            default:
                // code block
                break;
        }
    }

    public void SetUnitSprites(string race, string PrimClass, GameObject unit)
    {
        UnitSpriteManager GMUnitSpriteManager = unit.GetComponentInChildren<UnitSpriteManager>(); // this is what we talk to to set sprites on created unit
        
        List<List<Sprite>> CurrentRaceBodies = new List<List<Sprite>>();
        List<List<Sprite>> CurrentEyes = new List<List<Sprite>>();
        List<Color> CurrentEyeColors = new List<Color>();
        List<Sprite> CurrentHair = new List<Sprite>();
        List<Color> CurrentHairColors = new List<Color>();
        List<Sprite> CurrentFacialHair = new List<Sprite>();

        switch(race)
        {
                case "Human":
                    CurrentRaceBodies = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanBodyList;
                    CurrentEyes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanEyesList;
                    CurrentHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanHairList;
                    CurrentFacialHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanFacialHairList;
                    CurrentHairColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanHairColorList;
                    CurrentEyeColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().HumanEyeColorList;
                    break;
                case "Orc":
                    CurrentRaceBodies = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcBodyList;
                    CurrentEyes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcEyesList;
                    CurrentHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcHairList;
                    CurrentFacialHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcFacialHairList;
                    CurrentHairColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcHairColorList;
                    CurrentEyeColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OrcEyeColorList;
                    break;
                case "Elf":
                    CurrentRaceBodies = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfBodyList;
                    CurrentEyes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfEyesList;
                    CurrentHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfHairList;
                    CurrentFacialHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfFacialHairList;
                    CurrentHairColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfHairColorList;
                    CurrentEyeColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ElfEyeColorList;
                    break;
                case "Undead":
                    CurrentRaceBodies = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadBodyList;
                    CurrentEyes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadEyesList;
                    CurrentHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadHairList;
                    CurrentFacialHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadFacialHairList;
                    CurrentHairColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadHairColorList;
                    CurrentEyeColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().UndeadEyeColorList;
                    break;
                case "Demon":
                    CurrentRaceBodies = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonBodyList;
                    CurrentEyes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonEyesList;
                    CurrentHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonHairList;
                    CurrentFacialHair = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonFacialHairList;
                    CurrentHairColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonHairColorList;
                    CurrentEyeColors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().DemonEyeColorList;
                    break;
                default:
                    // code block
                    break;
        }
        
        // now our list of body types is correct, choose a random one and store it in a index, for when the player changes to a new one within the same race
        int IndexOfBodyType = UnityEngine.Random.Range(0, CurrentRaceBodies.Count);
        GMUnitSpriteManager.SetBodyType(CurrentRaceBodies[IndexOfBodyType]); // sets the body type
        // ^^^ RACE IS NOW SET AND DONE!
        int IndexOfEyeType = UnityEngine.Random.Range(0, CurrentEyes.Count);
        GMUnitSpriteManager.SetEyes(CurrentEyes[IndexOfEyeType]); // sets the eye type
        // HANDLE EYES COLOR HERE!
        // ^^^ EYES IS NOW SET AND DONE
        int IndexOfHairType = UnityEngine.Random.Range(0, CurrentHair.Count);
        GMUnitSpriteManager.SetHair(CurrentHair[IndexOfHairType]); // sets the Hair type
        int IndexOfFacialHairType = UnityEngine.Random.Range(0, CurrentFacialHair.Count);
        GMUnitSpriteManager.SetFacialHair(CurrentFacialHair[IndexOfFacialHairType]); // sets the facial hair type
        // now, set the COLORS of the eyes, and hair/fhair to random colors
        int IndexOfHairColor = UnityEngine.Random.Range(0, CurrentHairColors.Count);
        GMUnitSpriteManager.SetHairColor(CurrentHairColors[IndexOfHairColor]); // sets the hair color
        int IndexOfEyeColor = UnityEngine.Random.Range(0, CurrentEyeColors.Count);
        GMUnitSpriteManager.SetEyeColor(CurrentEyeColors[IndexOfEyeColor]); // sets the eye color
        // all done!

            List<List<Sprite>> CurrentArmors = new List<List<Sprite>>();
            List<List<Sprite>> CurrentClothes = new List<List<Sprite>>();
            List<List<Sprite>> CurrentPants = new List<List<Sprite>>();
            List<Sprite> CurrentHelmets = new List<Sprite>();
            List<Sprite> CurrentMainWeapons = new List<Sprite>();
            List<Sprite> CurrentOffHandWeapons = new List<Sprite>();

        switch(PrimClass)
        {
                case "Warrior":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().WarriorArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().WarriorClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().WarriorHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().WarriorWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffWarriorWeaponsList;
                    break;
                case "Ranger":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().RangerArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().RangerClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().RangerHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().RangerWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffRangerWeaponsList;
                    break;
                case "Assassin":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().AssassinArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().AssassinClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().AssassinHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().AssassinWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffAssassinWeaponsList;
                    break;
                case "Mage":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().MageArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().MageClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().MageHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().MageWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffMageWeaponsList;
                    break;
                case "Priest":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PriestArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PriestClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PriestHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PriestWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffPriestWeaponsList;
                    break;
                case "Conjurer":
                    CurrentArmors = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ConjurerArmorList;
                    CurrentClothes = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ConjurerClothesList;
                    CurrentHelmets = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ConjurerHelmetList;
                    CurrentMainWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().ConjurerWeaponsList;
                    CurrentOffHandWeapons = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().OffConjurerWeaponsList;
                    break;
                default:
                    // code block
                    break;
        }
        int IndexOfArmor = UnityEngine.Random.Range(0, CurrentArmors.Count);
        GMUnitSpriteManager.SetArmor(CurrentArmors[IndexOfArmor]); // sets the armor
        // done with armor
        int IndexOfClothes = UnityEngine.Random.Range(0, CurrentClothes.Count);
        GMUnitSpriteManager.SetClothes(CurrentClothes[IndexOfClothes]); // sets the Clothes
        // done with clothes
        CurrentPants = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PantsList;
        int IndexOfPants = UnityEngine.Random.Range(0, CurrentPants.Count);
        GMUnitSpriteManager.SetPants(CurrentPants[IndexOfPants]); // sets the pants
        // done with pants
        int IndexOfHelmet = UnityEngine.Random.Range(0, CurrentHelmets.Count);
        GMUnitSpriteManager.SetHelmet(CurrentHelmets[IndexOfHelmet]); // sets the helmets (will need a way to make some helms not remove hair)
        // done wit helmet
        int IndexOfMainWeapon = UnityEngine.Random.Range(0, CurrentMainWeapons.Count);
        GMUnitSpriteManager.SetWeapon(CurrentMainWeapons[IndexOfMainWeapon], true); // sets the armor
        int IndexOfOffWeapon = UnityEngine.Random.Range(0, CurrentOffHandWeapons.Count);
        GMUnitSpriteManager.SetWeapon(CurrentOffHandWeapons[IndexOfOffWeapon], false); // sets the armor
    }

    void GiveUnitClassTraits(GameObject unit, int unitsLevel, string unitsPrimClass, string unitsSubClass)
    {
        // grab dictionary of traits
        MyriadInfo info = new MyriadInfo();
        // set up list of traits
        List<string> ChosenTraits = new List<string>();
        string RolledTrait = "";

        if(unitsLevel % 2 != 0) unitsLevel--; // if the level is odd, were jus gonna make it even by remmooving one, then we can add a trait for each even level
        while(ChosenTraits.Count < unitsLevel/2)
        {
            
            RolledTrait = info.GetRandomClassTrait(ChosenTraits, unitsPrimClass, unitsSubClass);
            ChosenTraits.Add(RolledTrait);

            unit.AddComponent(Type.GetType(RolledTrait)); // ADD SCRIPT

            // add it to list in unit stats
            unit.GetComponent<UnitStats>().traits.Add(RolledTrait);
            // add it to selectedlist<int> in traittreemanagerscript
            unit.GetComponent<TraitTreeManager>().TraitSelected(RolledTrait, null);
        }
    }
}
