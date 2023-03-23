using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GMcreatorManager : MonoBehaviour
{
    [Header("Unit Object Components")]
    public GameObject GMunit;
    private string RacialTraitScriptApplied;
    [Header("Sound Components")]
    [SerializeField] private AudioClip tune;
    [Header("UI Components")]
    [SerializeField] GameObject ClassInfoPanel;
    [SerializeField] TextMeshProUGUI ClassTitleText;
    [SerializeField] TextMeshProUGUI ClassDescriptionText;
    [SerializeField] TextMeshProUGUI ClassAsSubclassStatGainText;
    [SerializeField] Image ClassIcon;
    [SerializeField] GameObject RaceInfoPanel;
    [SerializeField] TextMeshProUGUI RaceTitleText;
    [SerializeField] TextMeshProUGUI RaceDescriptionText;
    [SerializeField] TextMeshProUGUI RaceTraitTitleText;
    [SerializeField] TextMeshProUGUI RaceTraitDescriptionText;
    [SerializeField] Image RaceIcon;
    [SerializeField] Button HumanButton;
    [SerializeField] Button OrcButton;
    [SerializeField] Button ElfButton;
    [SerializeField] Button UndeadButton;
    [SerializeField] Button DemonButton;
    [SerializeField] Button WarriorPrimButton;
    [SerializeField] Button MagePrimButton;
    [SerializeField] Button RangerPrimButton;
    [SerializeField] Button AssassinPrimButton;
    [SerializeField] Button ConjurerPrimButton;
    [SerializeField] Button PriestPrimButton;
    [SerializeField] Button WarriorSecButton;
    [SerializeField] Button MageSecButton;
    [SerializeField] Button RangerSecButton;
    [SerializeField] Button AssassinSecButton;
    [SerializeField] Button ConjurerSecButton;
    [SerializeField] Button PriestSecButton;
    [SerializeField] Image NamePanel;
    [SerializeField] TextMeshProUGUI TitleClassText;
    [SerializeField] TextMeshProUGUI PrimPlusSubText;
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
    [Header("Unit Race and Class Selection")]
    public string SelectedRace = "Human";
    public string SelectedPrimaryClass = "Warrior";
    public string SelectedSubClass = "Warrior";
    public string SelectedTitleClass = "Tank";
    public bool isNameValid = false;
    [Header("SPUM Components")]
    UnitSpriteManager GMUnitSpriteManager;
    private int IndexOfBodyType = 0;
    private int IndexOfEyeType = 0;
    private int IndexOfHairType = 0;
    private int IndexOfFacialHairType = 0;
    private int IndexOfHairColor = 0;
    private int IndexOfEyeColor = 0;
    private List<List<Sprite>> CurrentRaceBodies = new List<List<Sprite>>();
    private List<List<Sprite>> CurrentEyes = new List<List<Sprite>>();
    private List<Color> CurrentEyeColors = new List<Color>();
    private List<Sprite> CurrentHair = new List<Sprite>();
    private List<Color> CurrentHairColors = new List<Color>();
    private List<Sprite> CurrentFacialHair = new List<Sprite>();
    private int IndexOfArmor = 0;
    private int IndexOfClothes = 0;
    private int IndexOfPants = 0;
    private int IndexOfHelmet = 0;
    private int IndexOfMainWeapon = 0;
    private int IndexOfOffWeapon = 0;
    private List<List<Sprite>> CurrentArmors = new List<List<Sprite>>();
    private List<List<Sprite>> CurrentClothes = new List<List<Sprite>>();
    private List<List<Sprite>> CurrentPants = new List<List<Sprite>>();
    private List<Sprite> CurrentHelmets = new List<Sprite>();
    private List<Sprite> CurrentMainWeapons = new List<Sprite>();
    private List<Sprite> CurrentOffHandWeapons = new List<Sprite>();

    void Start()
    {
        GMUnitSpriteManager = GameObject.FindGameObjectWithTag("UnitRoot").GetComponent<UnitSpriteManager>(); // this is what we talk to to set sprites on created unit
        ChangeRaceSprites("Human"); // race started out as human
        ChangeClassSprites("Warrior");

        string traitToApply = "HumanRacialTrait";
        RacialTraitScriptApplied = traitToApply;
        GMunit.AddComponent(System.Type.GetType(traitToApply)); // add the script 

        GetComponent<GenerateUnit>().SetUnitStatsBasedOnLevel(GMunit.GetComponent<UnitStats>());
        GMunit.GetComponent<Unit>().isGM = true;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AddUnitToCrew(GMunit, 0);
    }

    // called when a player hits a race button to change their race
    public void PrimaryClassButtonSelected(string selectedClass)
    {
        if(selectedClass == SelectedPrimaryClass) return; // already selected so do nothing
        SoundManager.instance.PlaySound(tune);
        SelectedPrimaryClass = selectedClass;

        GMunit.GetComponent<UnitStats>().primaryClass = selectedClass;
        GetComponent<GenerateUnit>().SetUnitStatsBasedOnLevel(GMunit.GetComponent<UnitStats>());
        MyriadInfo info = new MyriadInfo();
        GMunit.GetComponent<UnitStats>().titleClass = info.GetTitleClass(SelectedPrimaryClass, SelectedSubClass);

        // armor weapons etc based on primary class
        ChangeClassSprites(selectedClass);
        // finally, refresh the panel
        RefreshClassInfoPanel(selectedClass, true);
    }
    // called when a player hits a race button to change their race
    public void SecondaryClassButtonSelected(string selectedClass)
    {
        if(selectedClass == SelectedSubClass) return; // already selected so do nothing
        SoundManager.instance.PlaySound(tune);
        SelectedSubClass = selectedClass;

        GMunit.GetComponent<UnitStats>().subClass = selectedClass;
        GetComponent<GenerateUnit>().SetUnitStatsBasedOnLevel(GMunit.GetComponent<UnitStats>());
        MyriadInfo info = new MyriadInfo();
        GMunit.GetComponent<UnitStats>().titleClass = info.GetTitleClass(SelectedPrimaryClass, SelectedSubClass);

        // finally, refresh the panel
        RefreshClassInfoPanel(selectedClass, false);
    }

    private void RefreshClassInfoPanel(string selectedClass, bool isPrimary)
    {  
        // play animation
        ClassInfoPanel.GetComponent<Animator>().SetTrigger("Slide");

        //prim
        ColorBlock WarriorPrimColors = WarriorPrimButton.colors; // human
        ColorBlock MagePrimColors = MagePrimButton.colors; // orc
        ColorBlock RangerPrimColors = RangerPrimButton.colors; // elf
        ColorBlock AssassinPrimColors = AssassinPrimButton.colors; // undead
        ColorBlock ConjurerPrimColors = ConjurerPrimButton.colors; // demon
        ColorBlock PriestPrimColors = PriestPrimButton.colors; // demon
        //sec
        ColorBlock WarriorSecColors = WarriorSecButton.colors; // human
        ColorBlock MageSecColors = MageSecButton.colors; // orc
        ColorBlock RangerSecColors = RangerSecButton.colors; // elf
        ColorBlock AssassinSecColors = AssassinSecButton.colors; // undead
        ColorBlock ConjurerSecColors = ConjurerSecButton.colors; // demon
        ColorBlock PriestSecColors = PriestSecButton.colors; // demon

        // change button colors
        if(isPrimary)
        {
            // reset every prim class button colors
            WarriorPrimColors.normalColor = new Color32(255, 39, 18, 255);
            WarriorPrimButton.colors = WarriorPrimColors;
            MagePrimColors.normalColor = new Color32(18, 156, 255, 255);
            MagePrimButton.colors = MagePrimColors;
            RangerPrimColors.normalColor = new Color32(87, 195, 57, 255);
            RangerPrimButton.colors = RangerPrimColors;
            AssassinPrimColors.normalColor = new Color32(71, 36, 215, 255);
            AssassinPrimButton.colors = AssassinPrimColors;
            ConjurerPrimColors.normalColor = new Color32(223, 55, 255, 255);
            ConjurerPrimButton.colors = ConjurerPrimColors;
            PriestPrimColors.normalColor = new Color32(255, 188, 54, 255);
            PriestPrimButton.colors = PriestPrimColors;
        }
        else
        {
            // reset every sec class button colors
            WarriorSecColors.normalColor = new Color32(255, 39, 18, 255);
            WarriorSecButton.colors = WarriorSecColors;
            MageSecColors.normalColor = new Color32(18, 156, 255, 255);
            MageSecButton.colors = MageSecColors;
            RangerSecColors.normalColor = new Color32(87, 195, 57, 255);
            RangerSecButton.colors = RangerSecColors;
            AssassinSecColors.normalColor = new Color32(71, 36, 215, 255);
            AssassinSecButton.colors = AssassinSecColors;
            ConjurerSecColors.normalColor = new Color32(223, 55, 255, 255);
            ConjurerSecButton.colors = ConjurerSecColors;
            PriestSecColors.normalColor = new Color32(255, 188, 54, 255);
            PriestSecButton.colors = PriestSecColors;
        }

        // change info + color
        Color colorToChangeTo = new Color32(128, 255, 128, 255);
        switch(selectedClass)
            {
                case "Warrior":
                    ClassIcon.sprite = warriorClassIcon;
                    ClassTitleText.text = "Warrior";
                    ClassDescriptionText.text = "Shouldering heaps of iron with <color=#FF2712>brute strength<color=#FFFF>. They excel at being a <color=#FF2712>dominating force<color=#FFFF>, relishing in war.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +2d/+5hp EVERY LEVEL";
                    colorToChangeTo = new Color32(255, 39, 18, 255); // R, G, B, ALPHA
                    if(isPrimary) {
                        WarriorPrimColors.normalColor = new Color32(255, 255, 255, 255);
                        WarriorPrimButton.colors = WarriorPrimColors;
                    } else {
                        WarriorSecColors.normalColor = new Color32(255, 255, 255, 255);
                        WarriorSecButton.colors = WarriorSecColors;
                    }
                    // set icon when we have icons
                    break;
                case "Mage":
                    ClassIcon.sprite = mageClassIcon;
                    ClassTitleText.text = "Mage";
                    ClassDescriptionText.text = "Gifted with <color=#129CFF>intellect and magical ability<color=#FFFF>, they use a wide variety of magic to gain the upper hand.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +3d/+3hp EVERY LEVEL";
                    colorToChangeTo = new Color32(18, 156, 255, 255); // R, G, B, ALPHA
                    if(isPrimary) {
                        MagePrimColors.normalColor = new Color32(255, 255, 255, 255);
                        MagePrimButton.colors = MagePrimColors;
                    } else {
                        MageSecColors.normalColor = new Color32(255, 255, 255, 255);
                        MageSecButton.colors = MageSecColors;
                    }
                    // set icon when we have icons
                    break;
                case "Ranger":
                    ClassIcon.sprite = rangerClassIcon;
                    ClassTitleText.text = "Ranger";
                    ClassDescriptionText.text = "A primal and <color=#57C339>precise hunter<color=#FFFF>. They have a fierce connection with nature and a <color=#57C339>heart of the wild<color=#FFFF>.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +2d/+5hp EVERY LEVEL";
                    colorToChangeTo = new Color32(87, 195, 57, 255);
                    if(isPrimary) {
                        RangerPrimColors.normalColor = new Color32(255, 255, 255, 255);
                        RangerPrimButton.colors = RangerPrimColors;
                    } else {
                        RangerSecColors.normalColor = new Color32(255, 255, 255, 255);
                        RangerSecButton.colors = RangerSecColors;
                    }
                    // set icon when we have icons
                    break;
                case "Assassin":
                    ClassIcon.sprite = assassinClassIcon;
                    ClassTitleText.text = "Assassin";
                    ClassDescriptionText.text = "Master of shadows, <color=#4724D7>efficient thieves<color=#FFFF> specialized in taking down enemies as quickly as possible.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +3d/+3hp EVERY LEVEL";
                    colorToChangeTo = new Color32(71, 36, 215, 255); // R, G, B, ALPHA
                    if(isPrimary) {
                        AssassinPrimColors.normalColor = new Color32(255, 255, 255, 255);
                        AssassinPrimButton.colors = AssassinPrimColors;
                    } else {
                        AssassinSecColors.normalColor = new Color32(255, 255, 255, 255);
                        AssassinSecButton.colors = AssassinSecColors;
                    }
                    // set icon when we have icons
                    break;
                case "Conjurer":
                    ClassIcon.sprite = conjurerClassIcon;
                    ClassTitleText.text = "Conjurer";
                    ClassDescriptionText.text = "Harborers of <color=#DF37FF>demonic power<color=#FFFF>, they embrace the <color=#DF37FF>brutality and chaos<color=#FFFF> of the warzone called <color=#DF37FF>hell<color=#FFFF>.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +2d/+5hp EVERY LEVEL";
                    colorToChangeTo = new Color32(223, 55, 255, 255); // R, G, B, ALPHA
                    if(isPrimary) {
                        ConjurerPrimColors.normalColor = new Color32(255, 255, 255, 255);
                        ConjurerPrimButton.colors = ConjurerPrimColors;
                    } else {
                        ConjurerSecColors.normalColor = new Color32(255, 255, 255, 255);
                        ConjurerSecButton.colors = ConjurerSecColors;
                    }
                    // set icon when we have icons
                    break;
                case "Priest":
                    ClassIcon.sprite = priestClassIcon;
                    ClassTitleText.text = "Priest";
                    ClassDescriptionText.text = "Holy divine, devote <color=#FFBC36>spiritual healers<color=#FFFF> able to <color=#FFBC36>buff and grace<color=#FFFF> their allies.";
                    ClassAsSubclassStatGainText.text = "AS SUBCLASS: +1d/+6hp EVERY LEVEL";
                    colorToChangeTo = new Color32(255, 188, 54, 255); // R, G, B, ALPHA
                    if(isPrimary) {
                        PriestPrimColors.normalColor = new Color32(255, 255, 255, 255);
                        PriestPrimButton.colors = PriestPrimColors;
                    } else {
                        PriestSecColors.normalColor = new Color32(255, 255, 255, 255);
                        PriestSecButton.colors = PriestSecColors;
                    }
                    // set icon when we have icons
                    break;
                default:
                    // code block
                    break;
            }
        ClassIcon.color = colorToChangeTo;
        ClassTitleText.color = colorToChangeTo; // changing color of class title text
        Image lineItemImage = ClassInfoPanel.GetComponent<Image>(); // this should be in scope of script not just here
        lineItemImage.color = colorToChangeTo; // R, G, B, ALPHA

        SetTitleClassElements();
    }

    void SetTitleClassElements()
    {
        MyriadInfo info = new MyriadInfo();
        // set title class
        SelectedTitleClass = info.GetTitleClass(SelectedPrimaryClass, SelectedSubClass);
        // set text elements
        TitleClassText.text = SelectedTitleClass;
        TitleClassText.color = info.GetUnitsTitleClassColor(SelectedPrimaryClass, SelectedSubClass);
        // anim of title
        TitleClassText.gameObject.GetComponent<Animator>().SetTrigger("Pop");
        // set color of panel
        NamePanel.color = info.GetUnitsTitleClassColor(SelectedPrimaryClass, SelectedSubClass);
        // prim + sub text
        string PrimaryColorString = "";
        string SubColorString = "";
        switch(SelectedPrimaryClass)
        {
            case "Warrior":
                    PrimaryColorString = "FF2712";
                    break;
                case "Mage":
                    PrimaryColorString = "129CFF";
                    break;
                case "Ranger":
                    PrimaryColorString = "57C339";
                    break;
                case "Assassin":
                    PrimaryColorString = "4724D7";
                    break;
                case "Conjurer":
                    PrimaryColorString = "DF37FF";
                    break;
                case "Priest":
                    PrimaryColorString = "FFBC36";
                    break;
                default:
                    // code block
                    break;
                
        }
        switch(SelectedSubClass)
        {
            case "Warrior":
                    SubColorString = "FF2712";
                    break;
                case "Mage":
                    SubColorString = "129CFF";
                    break;
                case "Ranger":
                    SubColorString = "57C339";
                    break;
                case "Assassin":
                    SubColorString = "4724D7";
                    break;
                case "Conjurer":
                    SubColorString = "DF37FF";
                    break;
                case "Priest":
                    SubColorString = "FFBC36";
                    break;
                default:
                    // code block
                    break;
                
        }
        PrimPlusSubText.text = "<color=#" + PrimaryColorString + ">" + SelectedPrimaryClass + "<color=#FFFF> + " + "<color=#" + SubColorString + ">" + SelectedSubClass;
    }

    // called when a player hits a race button to change their race
    public void RaceButtonSelected(string selectedRace)
    {
        if(SelectedRace == selectedRace) return; // already selected so do nothing
        SoundManager.instance.PlaySound(tune);
        SelectedRace = selectedRace;
        GMunit.GetComponent<UnitStats>().race = selectedRace;
        
        if(RacialTraitScriptApplied == "OrcRacialTrait") GMunit.GetComponent<OrcRacialTrait>().Revoke();
        if(RacialTraitScriptApplied == "ElfRacialTrait") GMunit.GetComponent<ElfRacialTrait>().Revoke();
        if(RacialTraitScriptApplied == "UndeadRacialTrait") GMunit.GetComponent<UndeadRacialTrait>().Revoke();
        Destroy(GMunit.GetComponent(System.Type.GetType(RacialTraitScriptApplied)));
        string traitToApply = GMunit.GetComponent<UnitStats>().race + "RacialTrait";
        GMunit.AddComponent(System.Type.GetType(traitToApply)); // add the script 
        RacialTraitScriptApplied = traitToApply;

        ChangeRaceSprites(selectedRace);
        RefreshRaceInfoPanel(selectedRace);
    }

    private void RefreshRaceInfoPanel(string selectedRace)
    {  
        // play animation
        RaceInfoPanel.GetComponent<Animator>().SetTrigger("Slide");

        // buttons and button colors
        // reset every race button colors
        ColorBlock HumanColors = HumanButton.colors; // human
        HumanColors.normalColor = new Color32(255, 235, 158, 255);
        HumanButton.colors = HumanColors;
        ColorBlock OrcColors = OrcButton.colors; // orc
        OrcColors.normalColor = new Color32(248, 119, 65, 255);
        OrcButton.colors = OrcColors;
        ColorBlock ElfColors = ElfButton.colors; // elf
        ElfColors.normalColor = new Color32(90, 235, 255, 255);
        ElfButton.colors = ElfColors;
        ColorBlock UndeadColors = UndeadButton.colors; // undead
        UndeadColors.normalColor = new Color32(59, 176, 147, 255);
        UndeadButton.colors = UndeadColors;
        ColorBlock DemonColors = DemonButton.colors; // demon
        DemonColors.normalColor = new Color32(255, 73, 71, 255);
        DemonButton.colors = DemonColors;

        // change info + color
        Color colorToChangeTo = new Color32(128, 255, 128, 255);
        switch(selectedRace)
            {
                case "Human":
                    RaceIcon.sprite = HumanRaceIcon;
                    RaceTitleText.text = "Human";
                    RaceDescriptionText.text = "A prosperous nomadic race capable of building kingdoms and conquering the environment.";
                    RaceTraitTitleText.text = "RACIAL TRAIT: Human Spirit";
                    RaceTraitDescriptionText.text = "Heal <color=#A2FF1F>+1HP<color=#FFFFFF> every round.";
                    colorToChangeTo = new Color32(255, 235, 158, 255); // R, G, B, ALPHA
                    HumanColors.normalColor = new Color32(255, 255, 255, 255);
                    HumanButton.colors = HumanColors;
                    // set icon when we have icons
                    break;
                case "Orc":
                    RaceIcon.sprite = OrcRaceIcon;
                    RaceTitleText.text = "Orc";
                    RaceDescriptionText.text = "Decedents of ancient elves, but were shaped by an ever growing blood-lust for warfare.";
                    RaceTraitTitleText.text = "RACIAL TRAIT: Hardened Skin";
                    RaceTraitDescriptionText.text = "Have <color=#FF4242>+5 extra health<color=#FFFFFF>.";
                    colorToChangeTo = new Color32(248, 119, 65, 255); // R, G, B, ALPHA
                    OrcColors.normalColor = new Color32(255, 255, 255, 255);
                    OrcButton.colors = OrcColors;
                    // set icon when we have icons
                    break;
                case "Elf":
                    RaceIcon.sprite = ElfRaceIcon;
                    RaceTitleText.text = "Elf";
                    RaceDescriptionText.text = "The eldest race, and thus one most connected with the earth and elements.";
                    RaceTraitTitleText.text = "RACIAL TRAIT: Ancient Precision";
                    RaceTraitDescriptionText.text = "Have <color=#1EF1FF>+10% Crit Chance<color=#FFFFFF> and <color=#1EF1FF>+5 Crit Damage<color=#FFFFFF>.";
                    colorToChangeTo = new Color32(90, 235, 255, 255);
                    ElfColors.normalColor = new Color32(255, 255, 255, 255);
                    ElfButton.colors = ElfColors;
                    // set icon when we have icons
                    break;
                case "Undead":
                    RaceIcon.sprite = UndeadRaceIcon;
                    RaceTitleText.text = "Undead";
                    RaceDescriptionText.text = "Hollowed souls reanimated by the underbelly of evil, the unknown.";
                    RaceTraitTitleText.text = "RACIAL TRAIT: Iron Bones";
                    RaceTraitDescriptionText.text = "Have <color=#F5C857>+2 Attack<color=#FFFFFF> and <color=#FF8A1E>15% extra block chance<color=#FFFFFF>.";
                    colorToChangeTo = new Color32(59, 176, 147, 255); // R, G, B, ALPHA
                    UndeadColors.normalColor = new Color32(255, 255, 255, 255);
                    UndeadButton.colors = UndeadColors;
                    // set icon when we have icons
                    break;
                case "Demon":
                    RaceIcon.sprite = DemonRaceIcon;
                    RaceTitleText.text = "Demon";
                    RaceDescriptionText.text = "Twisted beings tainted by the inferno, descending towards a path of madness.";
                    RaceTraitTitleText.text = "RACIAL TRAIT: Blood Beast";
                    RaceTraitDescriptionText.text = "Before Attacking, deal <color=#FF4242>1<color=#FFFFFF> damage to <color=#FF4242>yourself<color=#FFFFFF>, Then gain <color=#F5C857>+1 attack<color=#FFFFFF> for each INJURED FRIENDLY unit.";
                    colorToChangeTo = new Color32(255, 73, 71, 255); // R, G, B, ALPHA
                    DemonColors.normalColor = new Color32(255, 255, 255, 255);
                    DemonButton.colors = DemonColors;
                    // set icon when we have icons
                    break;
                default:
                    // code block
                    break;
            }
        RaceIcon.color = colorToChangeTo;
        RaceTraitTitleText.color = colorToChangeTo;
        RaceTitleText.color = colorToChangeTo; // changing color of class title text
        Image lineItemImage = RaceInfoPanel.GetComponent<Image>(); // this should be in scope of script not just here
        lineItemImage.color = colorToChangeTo; // R, G, B, ALPHA
    }

    // =========== SPUM SPRITE WORK SECTION ==============
    // fires if change in race has occurred. should change the Eyes, Eye Color, Skin, Hair, Hair Color, Facial Hair, Facial Hair Color, 
    public void ChangeRaceSprites(string race)
    {
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
        IndexOfBodyType = Random.Range(0, CurrentRaceBodies.Count);
        GMUnitSpriteManager.SetBodyType(CurrentRaceBodies[IndexOfBodyType]); // sets the body type
        // ^^^ RACE IS NOW SET AND DONE!
        IndexOfEyeType = Random.Range(0, CurrentEyes.Count);
        GMUnitSpriteManager.SetEyes(CurrentEyes[IndexOfEyeType]); // sets the eye type
        // HANDLE EYES COLOR HERE!
        // ^^^ EYES IS NOW SET AND DONE
        IndexOfHairType = Random.Range(0, CurrentHair.Count);
        GMUnitSpriteManager.SetHair(CurrentHair[IndexOfHairType]); // sets the Hair type
        IndexOfFacialHairType = Random.Range(0, CurrentFacialHair.Count);
        GMUnitSpriteManager.SetFacialHair(CurrentFacialHair[IndexOfFacialHairType]); // sets the facial hair type
        // now, set the COLORS of the eyes, and hair/fhair to random colors
        IndexOfHairColor = Random.Range(0, CurrentHairColors.Count);
        GMUnitSpriteManager.SetHairColor(CurrentHairColors[IndexOfHairColor]); // sets the hair color
        IndexOfEyeColor = Random.Range(0, CurrentEyeColors.Count);
        GMUnitSpriteManager.SetEyeColor(CurrentEyeColors[IndexOfEyeColor]); // sets the eye color
        // all done!
    }

    public void ChangeClassSprites(string PrimClass)
    {
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
        IndexOfArmor = Random.Range(0, CurrentArmors.Count);
        GMUnitSpriteManager.SetArmor(CurrentArmors[IndexOfArmor]); // sets the armor
        // done with armor
        IndexOfClothes = Random.Range(0, CurrentClothes.Count);
        GMUnitSpriteManager.SetClothes(CurrentClothes[IndexOfClothes]); // sets the Clothes
        // done with clothes
        CurrentPants = GameObject.FindGameObjectWithTag("UnitSpriteHolder").GetComponent<UnitSpriteHolder>().PantsList;
        IndexOfPants = Random.Range(0, CurrentPants.Count);
        GMUnitSpriteManager.SetPants(CurrentPants[IndexOfPants]); // sets the pants
        // done with pants
        IndexOfHelmet = Random.Range(0, CurrentHelmets.Count);
        GMUnitSpriteManager.SetHelmet(CurrentHelmets[IndexOfHelmet]); // sets the helmets (will need a way to make some helms not remove hair)
        // done wit helmet
        IndexOfMainWeapon = Random.Range(0, CurrentMainWeapons.Count);
        GMUnitSpriteManager.SetWeapon(CurrentMainWeapons[IndexOfMainWeapon], true); // sets the armor
        IndexOfOffWeapon = Random.Range(0, CurrentOffHandWeapons.Count);
        GMUnitSpriteManager.SetWeapon(CurrentOffHandWeapons[IndexOfOffWeapon], false); // sets the armor

    }

    // iterative customization change
    public void ChangeSkinColor(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfBodyType += i;
        if(IndexOfBodyType >= CurrentRaceBodies.Count) IndexOfBodyType = 0;
        if(IndexOfBodyType < 0) IndexOfBodyType = CurrentRaceBodies.Count-1;
        GMUnitSpriteManager.SetBodyType(CurrentRaceBodies[IndexOfBodyType]);
    }

    public void ChangeHairStyle(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfHairType += i;
        if(IndexOfHairType >= CurrentHair.Count) IndexOfHairType = 0;
        if(IndexOfHairType < 0) IndexOfHairType = CurrentHair.Count-1;
        GMUnitSpriteManager.SetHair(CurrentHair[IndexOfHairType]);
    }

    public void ChangeHairColor(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfHairColor += i;
        if(IndexOfHairColor >= CurrentHairColors.Count) IndexOfHairColor = 0;
        if(IndexOfHairColor < 0) IndexOfHairColor = CurrentHairColors.Count-1;
        GMUnitSpriteManager.SetHairColor(CurrentHairColors[IndexOfHairColor]);
    }

    public void ChangeFacialHair(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfFacialHairType += i;
        if(IndexOfFacialHairType >= CurrentFacialHair.Count) IndexOfFacialHairType = 0;
        if(IndexOfFacialHairType < 0) IndexOfFacialHairType = CurrentFacialHair.Count-1;
        GMUnitSpriteManager.SetFacialHair(CurrentFacialHair[IndexOfFacialHairType]);
    }

    public void ChangeEyes(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfEyeType += i;
        if(IndexOfEyeType >= CurrentEyes.Count) IndexOfEyeType = 0;
        if(IndexOfEyeType < 0) IndexOfEyeType = CurrentEyes.Count-1;
        GMUnitSpriteManager.SetEyes(CurrentEyes[IndexOfEyeType]);
    }

    public void ChangeEyeColor(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfEyeColor += i;
        if(IndexOfEyeColor >= CurrentEyeColors.Count) IndexOfEyeColor = 0;
        if(IndexOfEyeColor < 0) IndexOfEyeColor = CurrentEyeColors.Count-1;
        GMUnitSpriteManager.SetEyeColor(CurrentEyeColors[IndexOfEyeColor]);
    }

    public void ChangeArmor(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfArmor += i;
        if(IndexOfArmor >= CurrentArmors.Count) IndexOfArmor = 0;
        if(IndexOfArmor < 0) IndexOfArmor = CurrentArmors.Count-1;
        GMUnitSpriteManager.SetArmor(CurrentArmors[IndexOfArmor]);
    }

    public void ChangeHelmet(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfHelmet += i;
        if(IndexOfHelmet >= CurrentHelmets.Count) IndexOfHelmet = 0;
        if(IndexOfHelmet < 0) IndexOfHelmet = CurrentHelmets.Count-1;
        GMUnitSpriteManager.SetHelmet(CurrentHelmets[IndexOfHelmet]);
    }

    public void ChangeClothes(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfClothes += i;
        if(IndexOfClothes >= CurrentClothes.Count) IndexOfClothes = 0;
        if(IndexOfClothes < 0) IndexOfClothes = CurrentClothes.Count-1;
        GMUnitSpriteManager.SetClothes(CurrentClothes[IndexOfClothes]);
    }

    public void ChangeMainWeapon(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfMainWeapon += i;
        if(IndexOfMainWeapon >= CurrentMainWeapons.Count) IndexOfMainWeapon = 0;
        if(IndexOfMainWeapon < 0) IndexOfMainWeapon = CurrentMainWeapons.Count-1;
        GMUnitSpriteManager.SetWeapon(CurrentMainWeapons[IndexOfMainWeapon], true);
    }

    public void ChangeOffWeapon(int i)
    {
        SoundManager.instance.PlaySound(tune);
        IndexOfOffWeapon += i;
        if(IndexOfOffWeapon >= CurrentOffHandWeapons.Count) IndexOfOffWeapon = 0;
        if(IndexOfOffWeapon < 0) IndexOfOffWeapon = CurrentOffHandWeapons.Count-1;
        GMUnitSpriteManager.SetWeapon(CurrentOffHandWeapons[IndexOfOffWeapon], false);
    }
}
