using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyriadInfo
{

    public Dictionary<int, string> traitsIndex = new Dictionary<int, string>()
        {
            // PROT TRAITS
            { 100, "Warcry" },
            { 101, "UnrelentingWarrior" },
            { 102, "Tank" },
            { 103, "BloodBath" },
            { 104, "DominatingForce" },
            { 105, "Conquerer" },
            { 106, "Cleave" },
            // MAGE TRAITS
            { 200, "StormBringer" },
            { 201, "IceBinding" },
            { 202, "EarthShaker" },
            { 203, "ArcaneShield" },
            { 204, "BloodMagic" },
            { 205, "Deflection" },
            { 206, "IceShield" },
            // RANGER TRAITS
            { 300, "VoidArrow" },
            { 301, "SpiritBeast" },
            { 302, "Retreat" },
            { 303, "DualShot" },
            { 304, "CripplingArrow" },
            { 305, "BearTrap" },
            { 306, "Headshot" },
            // ASSASSIN TRAITS
            { 400, "MasterSwordsman" },
            { 401, "FromTheGrave" },
            { 402, "Execute" },
            { 403, "Eviscerate" },
            { 404, "BloodKnives" },
            { 405, "SweepingStrike" },
            { 406, "Evasion" },
            // CONJURER TRAITS
            { 500, "SiphonPower" },
            { 501, "Shapeshift" },
            { 502, "Ghoul" },
            { 503, "Fiend" },
            { 504, "ExplodingBlood" },
            { 505, "Curse" },
            { 506, "ArtificerOfChaos" },
            // PRIEST TRAITS
            { 600, "Judgement" },
            { 601, "HolyNova" },
            { 602, "HolyBell" },
            { 603, "GuardianAngel" },
            { 604, "EtherealSpirit" },
            { 605, "DividedSoul" },
            { 606, "Bulwork" },
        };

    public Color GetPrimaryClassColor(string givenClass)
    {
        Color returningColor = new Color32(128, 255, 128, 255);

        switch(givenClass)
            {
                case "Warrior":
                    returningColor = new Color32(255, 39, 18, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Mage":
                    returningColor = new Color32(18, 156, 255, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Ranger":
                    returningColor = new Color32(87, 195, 57, 255);
                    // set icon when we have icons
                    break;
                case "Assassin":
                    returningColor = new Color32(71, 36, 215, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Conjurer":
                    returningColor = new Color32(223, 55, 255, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Priest":
                    returningColor = new Color32(255, 188, 54, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                default:
                    // code block
                    break;
            }

        return returningColor;
    }

    public Color GetUnitsTitleClassColor(string primaryClass, string subClass)
    {
        Color returningColor = new Color32(128, 255, 128, 255);

        // === MASSIVE NESTED SWITCH BLOCK TO DETERMINE CLASS ===
        switch(primaryClass)
        {
            case "Warrior":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Tank";
                        returningColor = new Color32(255, 80, 0, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Sentinel";
                        returningColor = new Color32(174, 183, 255, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Warden";
                        returningColor = new Color32(117, 255, 18, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Blade Master";
                        returningColor = new Color32(217, 71, 117, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Night Shield";
                        returningColor = new Color32(110, 38, 193, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Paladin";
                        returningColor = new Color32(255, 225, 0, 255);
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Mage":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Guardian";
                        returningColor = new Color32(239, 124, 255, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Battle Mage";
                        returningColor = new Color32(0, 255, 227, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Archwizard";
                        returningColor = new Color32(72, 248, 160, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Arcane Thief";
                        returningColor = new Color32(255, 192, 255, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Shadow Caster";
                        returningColor = new Color32(144, 91, 221, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Soul Weaver";
                        returningColor = new Color32(248, 246, 179, 255);
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Ranger":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Wild Blade";
                        returningColor = new Color32(80, 180, 58, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Shaman";
                        returningColor = new Color32(77, 144, 212, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Hunter";
                        returningColor = new Color32(43, 231, 75, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Duelist";
                        returningColor = new Color32(90, 145, 100, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Beast Master";
                        returningColor = new Color32(119, 90, 135, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Soulbow";
                        returningColor = new Color32(237, 255, 114, 255);
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Assassin":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Rogue";
                        returningColor = new Color32(142, 0, 29, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Trickster";
                        returningColor = new Color32(0, 155, 155, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Scout";
                        returningColor = new Color32(77, 144, 212, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Ninja";
                        returningColor = new Color32(70, 70, 200, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Shadow Blade";
                        returningColor = new Color32(107, 0, 156, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Acolyte";
                        returningColor = new Color32(156, 144, 0, 255);
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Conjurer":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Dreadnought";
                        returningColor = new Color32(210, 5, 118, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Warlock";
                        returningColor = new Color32(242, 72, 242, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Wanderer";
                        returningColor = new Color32(148, 100, 164, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Cultist";
                        returningColor = new Color32(107, 40, 255, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Summoner";
                        returningColor = new Color32(197, 0, 255, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Necromancer";
                        returningColor = new Color32(212, 123, 200, 255);
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Priest":
                switch(subClass)
                {
                    case "Warrior":
                        //returnedTitleClass = "Templar";
                        returningColor = new Color32(255, 195, 93, 255);
                        break;
                    case "Mage":
                        //returnedTitleClass = "Oracle";
                        returningColor = new Color32(165, 255, 241, 255);
                        break;
                    case "Ranger":
                        //returnedTitleClass = "Druid";
                        returningColor = new Color32(83, 255, 98, 255);
                        break;
                    case "Assassin":
                        //returnedTitleClass = "Holy Blade";
                        returningColor = new Color32(255, 236, 84, 255);
                        break;
                    case "Conjurer":
                        //returnedTitleClass = "Shadow Lord";
                        returningColor = new Color32(255, 156, 188, 255);
                        break;
                    case "Priest":
                        //returnedTitleClass = "Divine";
                        returningColor = new Color32(255, 241, 81, 255);
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

        return returningColor;
    }

    public Color GetUnitRaceColor(GameObject unit)
    {
        // change info + color
        Color colorToChangeTo = new Color32(128, 255, 128, 255);
        switch(unit.GetComponent<UnitStats>().race)
            {
                case "Human":
                    colorToChangeTo = new Color32(255, 235, 158, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Orc":
                    colorToChangeTo = new Color32(248, 119, 65, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Elf":
                    colorToChangeTo = new Color32(90, 235, 255, 255);
                    // set icon when we have icons
                    break;
                case "Undead":
                    colorToChangeTo = new Color32(59, 176, 147, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                case "Demon":
                    colorToChangeTo = new Color32(255, 73, 71, 255); // R, G, B, ALPHA
                    // set icon when we have icons
                    break;
                default:
                    // code block
                    break;
            }
        
        return colorToChangeTo;
    }

    public string GetTitleClass(string primaryClass, string subClass)
    {
        string returnedTitleClass = "";

        // === MASSIVE NESTED SWITCH BLOCK TO DETERMINE CLASS ===
        switch(primaryClass)
        {
            case "Warrior":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Tank";
                        break;
                    case "Mage":
                        returnedTitleClass = "Sentinel";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Warden";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Blade Master";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Night Shield";
                        break;
                    case "Priest":
                        returnedTitleClass = "Paladin";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Mage":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Guardian";
                        break;
                    case "Mage":
                        returnedTitleClass = "Battle Mage";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Archwizard";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Arcane Thief";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Void Caster";
                        break;
                    case "Priest":
                        returnedTitleClass = "Soul Weaver";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Ranger":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Wild Blade";
                        break;
                    case "Mage":
                        returnedTitleClass = "Shaman";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Hunter";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Duelist";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Voidbow";
                        break;
                    case "Priest":
                        returnedTitleClass = "Soulbow";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Assassin":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Rogue";
                        break;
                    case "Mage":
                        returnedTitleClass = "Trickster";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Scout";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Ninja";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Shadow Blade";
                        break;
                    case "Priest":
                        returnedTitleClass = "Acolyte";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Conjurer":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Dreadnought";
                        break;
                    case "Mage":
                        returnedTitleClass = "Void Mage";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Wanderer";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Cultist";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Warlock";
                        break;
                    case "Priest":
                        returnedTitleClass = "Necromancer";
                        break;
                    default:
                        // code block
                        break;
                }
                break;
            case "Priest":
                switch(subClass)
                {
                    case "Warrior":
                        returnedTitleClass = "Templar";
                        break;
                    case "Mage":
                        returnedTitleClass = "Oracle";
                        break;
                    case "Ranger":
                        returnedTitleClass = "Druid";
                        break;
                    case "Assassin":
                        returnedTitleClass = "Holy Blade";
                        break;
                    case "Conjurer":
                        returnedTitleClass = "Shadow Lord";
                        break;
                    case "Priest":
                        returnedTitleClass = "Divine";
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
        // === END OF MASSIVE SWITCH BLOCK!! ===

        return returnedTitleClass;
    }

    public string GetRandomClassTrait(List<string> cullingTraits, string PrimaryClass, string SubClass)
    {
        // choose to either take from primary or subclass at RANDOM
        float classRoll = Random.Range(0, 2);
        string chosenClass = "";
        if(classRoll == 0f) chosenClass = PrimaryClass;
        else chosenClass = SubClass;

        int traitIndexMin = 0;
        int traitIndexMax = 0;

        // this is bad, this switch statement exists in TraitTreeManager script when getting traits as well, needs to only be one of these probably!
        switch(chosenClass)
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

        bool findingValidTrait = true;
        string returnedTraitName = "";
        // loop to find valid trait
        while(findingValidTrait)
        {
            // ROLL BETWEEN ALL VALID TRAITS
            int randomRoll = Random.Range(traitIndexMin, traitIndexMax); // THIS IS RANGE OF VALID TRAIT ID'S

            // get the name of the trait from the dictionary
            traitsIndex.TryGetValue(randomRoll, out returnedTraitName);

            if(!cullingTraits.Contains(returnedTraitName)) findingValidTrait = false;
        }

        return returnedTraitName; // return that value
    }

    public Color GetClassColorFromTraitName(string traitName)
    {
        Color returningColor = new Color32(255, 255, 255, 255);
        switch(traitName)
        {

            // ----------------------------------- Warrior TRAITS
            case "Warcry":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Unrelenting Warrior":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Tank":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Blood Bath":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Dominating Force":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Conquerer":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            case "Cleave":
                returningColor = GetPrimaryClassColor("Warrior");
                break;
            // ----------------------------------- Mage TRAITS
            case "Storm Bringer":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Ice Binding":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Earth Shaker":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Arcane Shield":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Blood Magic":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Deflection":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            case "Ice Shield":
                returningColor = GetPrimaryClassColor("Mage");
                break;
            // ----------------------------------- Ranger TRAITS
            case "Void Arrow":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Spirit Beast":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Retreat":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Dual Shot":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Crippling Arrow":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Bear Trap":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            case "Headshot":
                returningColor = GetPrimaryClassColor("Ranger");
                break;
            // ----------------------------------- Assassin TRAITS
            case "Master Swordsman":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "From The Grave":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "Execute":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "Eviscerate":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "Blood Knives":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "Sweeping Strike":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            case "Evasion":
                returningColor = GetPrimaryClassColor("Assassin");
                break;
            // ----------------------------------- Conjurer TRAITS
            case "Siphon Power":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Shapeshift":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Ghoul":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Fiend":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Exploding Blood":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Curse":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            case "Artificer Of Chaos":
                returningColor = GetPrimaryClassColor("Conjurer");
                break;
            // ----------------------------------- Priest TRAITS
            case "Judgement":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Holy Nova":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Holy Bell":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Guardian Angel":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Ethereal Spirit":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Divided Soul":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            case "Bulwork":
                returningColor = GetPrimaryClassColor("Priest");
                break;
            default:
                // code block
                break;
        }

        return returningColor;
    }

}
