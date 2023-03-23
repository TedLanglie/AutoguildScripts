using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // needed for IPointer methods, which is how to get mouse over on UI elements

public class IndividualTraitSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject TraitSelectEffect;
    public string NameOfTraitScriptToApply = "";
    public GameObject UnitToAddTraitScriptTo;
    private string traitName;
    private string traitDescription;
    [SerializeField] GameObject InfoSection;
    [SerializeField] TextMeshProUGUI TraitNameText;
    [SerializeField] TextMeshProUGUI TraitDescriptionText;
    [SerializeField] Image traitIcon;
    // class icons
    [SerializeField] Sprite protClassIcon;
    [SerializeField] Sprite mageClassIcon;
    [SerializeField] Sprite rangerClassIcon;
    [SerializeField] Sprite assassinClassIcon;
    [SerializeField] Sprite conjurerClassIcon;
    [SerializeField] Sprite priestClassIcon;
    [Header("Sound Components")]
    [SerializeField] private AudioClip TraitChosenSound;
    [Header("TraitSprites")]
    public List<Sprite> TraitIcons = new List<Sprite>();
    /* TRAIT ICON INDEX====
    0 = bloodknives
    1 = evasion
    2 = eviscerate
    3 = excecute
    4 = from the grave
    5= masterswordsman
    6 = SweepingStrike
    7 = artificer of chaos
    8 = curse
    9 = exploding blood
    10 = fiend
    11 = ghoul
    12 = shapeshift
    13 = siphon power
    14 = arcane shield
    15 = dance of flames
    16 = deflection
    17 == earthshaker
    18 = fire shield
    19 = ice binding
    20 = stormbringer
    21 = bulwork
    22 = dividedsoul
    23 = ethereal spirit
    24 = guardianangel
    25 = holy bell
    26 = holy nova
    27 = judgement
    28 = bear trap
    29 = crippling arrow
    30 = dualshot
    31 = headshot
    32 = retreat
    33 = spirit beast
    34 = void arrow
    35 = bloodbath
    36 = cleave
    37 = conquerer
    38 = dominating force
    39 = tank
    40 = unrelenting force
    41 = warcry
    */

    public bool selected = false;

    // run this method when somebody clicks on the trait in the trait select tree
    public void Selected()
    {
        if(selected) return; // return if we've already selected the trait
        SoundManager.instance.PlaySound(TraitChosenSound);
        Instantiate(TraitSelectEffect, transform.position, Quaternion.identity, transform.parent);
        selected = true;
        StartCoroutine(SetInfoParentTransform()); // so that it appears over other traits

        // ===============================================================================================
        UnitToAddTraitScriptTo.AddComponent(Type.GetType(NameOfTraitScriptToApply)); // ADD SCRIPT
        // ===============================================================================================

        // add it to list in unit stats
        UnitToAddTraitScriptTo.GetComponent<UnitStats>().traits.Add(NameOfTraitScriptToApply);
        // add it to selectedlist<int> in traittreemanagerscript
        UnitToAddTraitScriptTo.GetComponent<TraitTreeManager>().TraitSelected(NameOfTraitScriptToApply, gameObject);
    }

    public IEnumerator SetInfoParentTransform()
    {
        InfoSection.transform.SetParent(gameObject.transform.parent, true); // this is so info section isn't behind other trait icons
        yield return new WaitForSeconds(0f);
        InfoSection.transform.SetAsLastSibling();
    }

    // this method is called by the manager after its NameOfTraitScriptToApply has been set. It should now set the UI elements to match the trait name given to it
    // --- THIS TAKES a PARAMETER of unit from the trait tree manager which calls this function, this is so that this script knows which unit to add the trait to
    public void initializeElement(GameObject unit)
    {
        // set variable to the passed unit gameobject, so that if this is selected, it will pass to the correct unit
        UnitToAddTraitScriptTo = unit;

        // ---- SETTING VISUALS OF TRAIT ELEMENT BLOCK

        // SO HERES A BIG OL SWITCH STATEMENT, BASED ON WHAT THE "NameOfTraitScriptToApply" VARIABLE IS.
        // WITH THIS, IT WILL SET THESE THINGS DEPENDING ON THE TRAIT:
        // set name of trait text element
        // set class of trait element
        // set trait description element
        // set BG / color etc 
        // ----
        switch(NameOfTraitScriptToApply)
        {

            // ----------------------------------- Warrior TRAITS
            case "Warcry":
                traitIcon.sprite = TraitIcons[41];
                TraitNameText.text = "Warcry";
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, give adjacent friendly units <color=#F5C857>+3 attack<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "UnrelentingWarrior":
                traitIcon.sprite = TraitIcons[40];
                TraitNameText.text = "Unrelenting Warrior";
                TraitDescriptionText.text = "After taking <color=#FF4242>fatal<color=#FFFFFF> damage, <color=#FF8A1E>ignore<color=#FFFFFF> it once.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "Tank":
                traitIcon.sprite = TraitIcons[39];
                TraitNameText.text = "Tank";
                TraitDescriptionText.text = "Blocking now halves damage a <color=#FF8A1E>SECOND TIME<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "BloodBath":
                traitIcon.sprite = TraitIcons[35];
                TraitNameText.text = "Blood Bath";
                TraitDescriptionText.text = "<color=#FF8A1E>Before Attacking<color=#FFFFFF>, gain <color=#F5C857>+7 attack<color=#FFFFFF> if both you AND your target are <color=#FF4242>injured<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "DominatingForce":
                traitIcon.sprite = TraitIcons[38];
                TraitNameText.text = "Dominating Force";
                TraitDescriptionText.text = "If the enemy <color=#F5C857>attacking<color=#FFFFFF> you has less attack than you, get 50% increase to <color=#FF8A1E>block the attack<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "Conquerer":
                traitIcon.sprite = TraitIcons[37];
                TraitNameText.text = "Conquerer";
                TraitDescriptionText.text = "Gain <color=#F5C857>+3 attack<color=#FFFFFF> every time you <color=#FF4242>take damage<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Warrior");
                break;
            case "Cleave":
                TraitNameText.text = "Cleave";
                traitIcon.sprite = TraitIcons[36];
                TraitDescriptionText.text = "Your <color=#F5C857>attacks<color=#FFFFFF> now also hit an <color=#FF8A1E>adjacent<color=#FFFFFF> enemy.";
                StyleTraitAsCertainClass("Warrior");
                break;
            // ----------------------------------- Mage TRAITS
            case "StormBringer":
                TraitNameText.text = "Storm Bringer";
                traitIcon.sprite = TraitIcons[20];
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, deal <color=#F5C857>5 damage<color=#FFFFFF> to <color=#FF8A1E>adjacent<color=#FFFFFF> enemies.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "IceBinding":
                TraitNameText.text = "Ice Binding";
                traitIcon.sprite = TraitIcons[19];
                TraitDescriptionText.text = "<color=#FF8A1E>Before Attacking<color=#FFFFFF>, apply <color=#1EF1FF>ICE BINDING<color=#FFFFFF> to a random enemy. This will <color=#FF8A1E>halve the damage<color=#FFFFFF> of its next attack.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "EarthShaker":
                TraitNameText.text = "Earth Shaker";
                traitIcon.sprite = TraitIcons[17];
                TraitDescriptionText.text = "<color=#FF8A1E>Before Attacking<color=#FFFFFF>, deal <color=#F5C857>3 damage<color=#FFFFFF> to <color=#FF8A1E>all<color=#FFFFFF> enemies.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "ArcaneShield":
                TraitNameText.text = "Arcane Shield";
                traitIcon.sprite = TraitIcons[14];
                TraitDescriptionText.text = "<color=#FF8A1E>If you block an attack<color=#FFFFFF>, deal <color=#F5C857>5 damage<color=#FFFFFF> to <color=#FF8A1E>all<color=#FFFFFF> enemies.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "BloodMagic":
                TraitNameText.text = "Blood Magic";
                traitIcon.sprite = TraitIcons[15];
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, <color=#F5C857>Damage<color=#FFFFFF> an enemy with HALF of their <color=#FF8A1E>own attack<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "Deflection":
                TraitNameText.text = "Deflection";
                traitIcon.sprite = TraitIcons[16];
                TraitDescriptionText.text = "<color=#FF8A1E>When hit with damage<color=#FFFFFF>, <color=#1EF1FF>absorb<color=#FFFFFF> half of it, and <color=#F5C857>deflect<color=#FFFFFF> it to a <color=#FF4242>random enemy<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Mage");
                break;
            case "IceShield":
                TraitNameText.text = "Ice Shield";
                traitIcon.sprite = TraitIcons[18];
                TraitDescriptionText.text = "<color=#FF8A1E>After this unit is attacked<color=#FFFFFF>, apply <color=#1EF1FF>FREEZE<color=#FFFFFF> to the <color=#F5C857>attacker<color=#FFFFFF>. This will <color=#FF8A1E>halve the damage<color=#FFFFFF> of its next attack.";
                StyleTraitAsCertainClass("Mage");
                break;
            // ----------------------------------- Ranger TRAITS
            case "VoidArrow":
                TraitNameText.text = "Void Arrow";
                traitIcon.sprite = TraitIcons[34];
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, the enemy <color=#FF4242>cannot be healed<color=#FFFFFF> for 6 rounds.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "SpiritBeast":
                TraitNameText.text = "Spirit Beast";
                traitIcon.sprite = TraitIcons[33];
                TraitDescriptionText.text = "<color=#FF8A1E>After killing an enemy<color=#FFFFFF>, gain <color=#F5C857>half<color=#FFFFFF> of its <color=#F5C857>attack<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "Retreat":
                TraitNameText.text = "Retreat";
                traitIcon.sprite = TraitIcons[32];
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, gain <color=#F5C857>100% parry chance<color=#FFFFFF> and <color=#F5C857>10% dodge chance<color=#FFFFFF> for 4 rounds.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "DualShot":
                TraitNameText.text = "Dual Shot";
                traitIcon.sprite = TraitIcons[30];
                TraitDescriptionText.text = "<color=#FF8A1E>After Attacking<color=#FFFFFF>, deal <color=#F5C857>2-6 damage<color=#FFFFFF> to <color=#FF8A1E>two<color=#FFFFFF> random enemies.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "CripplingArrow":
                TraitNameText.text = "Crippling Arrow";
                traitIcon.sprite = TraitIcons[29];
                TraitDescriptionText.text = "<color=#FF8A1E>Before Attacking<color=#FFFFFF>, deal <color=#F5C857>4 damage<color=#FFFFFF> to a random enemy AND <color=#FF4242>decrease<color=#FFFFFF> its <color=#FF8A1E>attack by 3<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "BearTrap":
                TraitNameText.text = "Bear Trap";
                traitIcon.sprite = TraitIcons[28];
                TraitDescriptionText.text = "<color=#FF8A1E>When this unit is targeted<color=#FFFFFF>, deal <color=#F5C857>5 damage<color=#FFFFFF> BEFORE the enemy attacks.";
                StyleTraitAsCertainClass("Ranger");
                break;
            case "Headshot":
                TraitNameText.text = "Headshot"; // permanently gain +5 crit damage. You're critical hits also make enemies bleed for 3 turns;
                traitIcon.sprite = TraitIcons[31];
                TraitDescriptionText.text = "Permanently gain <color=#1EF1FF>+7 crit damage<color=#FFFFFF>. Your <color=#1EF1FF>critical hits<color=#FFFFFF> also make enemies <color=#FF4242>bleed<color=#FFFFFF> for 3 turns.";
                StyleTraitAsCertainClass("Ranger");
                break;
            // ----------------------------------- Assassin TRAITS
            case "MasterSwordsman":
                TraitNameText.text = "Master Swordsman";
                traitIcon.sprite = TraitIcons[5];
                TraitDescriptionText.text = "If your attack is <color=#1EF1FF>parried<color=#FFFFFF>, <color=#1EF1FF>parry<color=#FFFFFF> their <color=#1EF1FF>parry<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "FromTheGrave":
                TraitNameText.text = "From The Grave";
                traitIcon.sprite = TraitIcons[4];
                TraitDescriptionText.text = "<color=#FF8A1E>If an attack kills you<color=#FFFFFF>, deal <color=#F5C857>10 damage<color=#FFFFFF> to the attacker.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "Execute":
                TraitNameText.text = "Execute";
                traitIcon.sprite = TraitIcons[3];
                TraitDescriptionText.text = "<color=#FF8A1E>Before attacking<color=#FFFFFF>, deal <color=#F5C857>6 damage<color=#FFFFFF> to the enemy with the <color=#FF4242>lowest HP<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "Eviscerate":
                TraitNameText.text = "Eviscerate";
                traitIcon.sprite = TraitIcons[2];
                TraitDescriptionText.text = "<color=#FF8A1E>After attacking<color=#FFFFFF>, if the enemy is above <color=#FF4242>40% HP<color=#FFFFFF> deal an <color=#F5C857>extra 7 damage<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "BloodKnives":
                TraitNameText.text = "Blood Knives";
                traitIcon.sprite = TraitIcons[0];
                TraitDescriptionText.text = "If your target is <color=#FF4242>below 40% HP<color=#FFFFFF>, gain <color=#1EF1FF>50% crit chance<color=#FFFFFF> for <color=#F5C857>that attack<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "SweepingStrike":
                TraitNameText.text = "Sweeping Strike";
                traitIcon.sprite = TraitIcons[6];
                TraitDescriptionText.text = "<color=#FF8A1E>If your attack is dodged or blocked<color=#FFFFFF>, deal its <color=#1EF1FF>INTENDED<color=#FFFFFF> damage to a <color=#FF4242>different random enemy<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Assassin");
                break;
            case "Evasion":
                TraitNameText.text = "Evasion";
                traitIcon.sprite = TraitIcons[1];
                TraitDescriptionText.text = "Gain <color=#1EF1FF>dodge chance<color=#FFFFFF> as <color=#FF4242>health decreases<color=#FFFFFF>. 5% dodge at 50% health, 10% dodge at 25% health, 15% dodge at 10% health.";
                StyleTraitAsCertainClass("Assassin");
                break;
            // ----------------------------------- Conjurer TRAITS
            case "SiphonPower":
                TraitNameText.text = "Siphon Power";
                traitIcon.sprite = TraitIcons[13];
                TraitDescriptionText.text = "<color=#FF8A1E>Before Attacking<color=#FFFFFF>, <color=#F5C857>copy your targets attack<color=#FFFFFF>. Return to normal after attacking.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "Shapeshift":
                TraitNameText.text = "Shapeshift";
                traitIcon.sprite = TraitIcons[12];
                TraitDescriptionText.text = "<color=#DF37FF>Copy<color=#FFFFFF> a trait from an enemy.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "Ghoul":
                TraitNameText.text = "Ghoul";
                traitIcon.sprite = TraitIcons[11];
                TraitDescriptionText.text = "Gain <color=#F5C857>+8 attack<color=#FFFFFF> and <color=#FF4242>+8 health<color=#FFFFFF> when a <color=#A2FF1F>friendly<color=#FFFFFF> unit dies.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "Fiend":
                TraitNameText.text = "Fiend";
                traitIcon.sprite = TraitIcons[10];
                TraitDescriptionText.text = "<color=#FF8A1E>Before attacking<color=#FFFFFF>, deal <color=#FF4242>1 damage<color=#FFFFFF> to adjacent <color=#A2FF1F>friendly<color=#FFFFFF> units. Then gain <color=#F5C857>+3 attack<color=#FFFFFF> for each unit hit.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "ExplodingBlood":
                TraitNameText.text = "Exploding Blood";
                traitIcon.sprite = TraitIcons[9];
                TraitDescriptionText.text = "Your attacks apply <color=#FF4242>bleed<color=#FFFFFF>. If the units dies <color=#FF8A1E>while the bleed is active<color=#FFFFFF>, they <color=#DF37FF>explode<color=#FFFFFF> dealing <color=#F5C857>8 damage to adjacent enemies<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "Curse":
                TraitNameText.text = "Curse";
                traitIcon.sprite = TraitIcons[8];
                TraitDescriptionText.text = "<color=#FF8A1E>After attacking<color=#FFFFFF>, place a <color=#DF37FF>curse<color=#FFFFFF> on the enemy which will <color=#FF4242>2X<color=#FFFFFF> the <color=#F5C857>next damage<color=#FFFFFF> they take.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            case "ArtificerOfChaos":
                TraitNameText.text = "Artificer Of Chaos";
                traitIcon.sprite = TraitIcons[7];
                TraitDescriptionText.text = "<color=#FF8A1E>At the start of the game<color=#FFFFFF>, <color=#DF37FF>swap<color=#FFFFFF> the enemies FIRST attacking unit with their LAST attacking unit. Reduce both of <color=#F5C857>their attacks by 3<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Conjurer");
                break;
            // ----------------------------------- Priest TRAITS
            case "Judgement":
                TraitNameText.text = "Judgement";
                traitIcon.sprite = TraitIcons[27];
                TraitDescriptionText.text = "<color=#FF8A1E>Before attacking<color=#FFFFFF>, <color=#FF4242>reduce<color=#FFFFFF> the attack of the enemy with the highest attack <color=#F5C857>-4<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "HolyNova":
                TraitNameText.text = "Holy Nova";
                traitIcon.sprite = TraitIcons[26];
                TraitDescriptionText.text = "<color=#FF8A1E>After attacking<color=#FFFFFF>, <color=#A2FF1F>heal<color=#FFFFFF> adjacent units <color=#A2FF1F>+5<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "HolyBell":
                TraitNameText.text = "Holy Bell";
                traitIcon.sprite = TraitIcons[25];
                TraitDescriptionText.text = "<color=#FF8A1E>After attacking<color=#FFFFFF>, <color=#A2FF1F>heal<color=#FFFFFF> a random <color=#FF4242>injured<color=#FFFFFF> ally by <color=#F5C857>half the damage you delt<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "GuardianAngel":
                TraitNameText.text = "Guardian Angel";
                traitIcon.sprite = TraitIcons[24];
                TraitDescriptionText.text = "<color=#FF8A1E>After attacking<color=#FFFFFF>, give your <color=#A2FF1F>lowest health ally<color=#FFFFFF> <color=#FF8A1E>75% block chance<color=#FFFFFF> for 4 turns.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "EtherealSpirit":
                TraitNameText.text = "Ethereal Spirit";
                traitIcon.sprite = TraitIcons[23];
                TraitDescriptionText.text = "<color=#FF8A1E>After death<color=#FFFFFF>, <color=#1EF1FF>revive<color=#FFFFFF> with <color=#F5C857>5 attack<color=#FFFFFF> and <color=#FF4242>10 health<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "DividedSoul":
                TraitNameText.text = "Divided Soul";
                traitIcon.sprite = TraitIcons[22];
                TraitDescriptionText.text = "<color=#FF8A1E>Before attacking<color=#FFFFFF>, If you have MORE health than your target, gain <color=#F5C857>+5 attack<color=#FFFFFF>. If you have less, <color=#A2FF1F>heal yourself +7<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            case "Bulwork":
                TraitNameText.text = "Bulwork";
                traitIcon.sprite = TraitIcons[21];
                TraitDescriptionText.text = "<color=#FF8A1E>If any friendly blocks an attack<color=#FFFFFF>, <color=#A2FF1F>heal<color=#FFFFFF> them <color=#A2FF1F>+6<color=#FFFFFF>.";
                StyleTraitAsCertainClass("Priest");
                break;
            default:
                // code block
                break;
        }
    }

    // helper function for changing visuals based on class!
    void StyleTraitAsCertainClass(string TraitsClass)
    {
        // handle button colors
        Button buttonComponent = GetComponent<Button>();
        ColorBlock colors = buttonComponent.colors; // will be used to change the buttons color depending on class
        // using MyriadInfo
        MyriadInfo info = new MyriadInfo();
        colors.normalColor = info.GetPrimaryClassColor(TraitsClass);
        // ---------------
        buttonComponent.colors = colors;
        GetComponent<Image>().color = info.GetPrimaryClassColor(TraitsClass);
        // colors of info section
        InfoSection.GetComponent<Image>().color = info.GetPrimaryClassColor(TraitsClass);
        TraitNameText.color = info.GetPrimaryClassColor(TraitsClass);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ENTERED!");
        InfoSection.SetActive(true);
        InfoSection.transform.SetAsLastSibling();
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        InfoSection.SetActive(false);
    }
}
