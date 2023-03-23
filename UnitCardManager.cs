using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnitCardManager : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI LevelText;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI AttackText;
    [SerializeField] TextMeshProUGUI CritChanceText;
    [SerializeField] TextMeshProUGUI CritDamageText;
    [SerializeField] TextMeshProUGUI DodgeChanceText;
    [SerializeField] TextMeshProUGUI BlockChanceText;
    [SerializeField] TextMeshProUGUI ParryChanceText;
    [SerializeField] Image UnitSpriteImage;
    [SerializeField] Image ClassIconImage;
    [SerializeField] Image SubClassIconImage;
    [SerializeField] Image RaceIconImage;
    [SerializeField] Image CornerPiece1;
    [SerializeField] Image CornerPiece2;
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
    [Header("Card Sprite Components")]
    [SerializeField] Sprite WarriorCardIcon;
    [SerializeField] Sprite RangerCardIcon;
    [SerializeField] Sprite MageCardIcon;
    [SerializeField] Sprite AssassinCardIcon;
    [SerializeField] Sprite PriestCardIcon;
    [SerializeField] Sprite ConjurerCardIcon;
    
    // this function will take a units stats and the units sprite, and put them in the player card elements
    public void SetUIElements(UnitStats stats)
    {
        // play lil animation
        GetComponent<Animator>().SetTrigger("bubble");
        // UnitSpriteImage.sprite = UnitSprite;

        // now for all the text elements
        NameText.text = stats.name;
        LevelText.text = "Level " + stats.level + " " + stats.titleClass;
        HealthText.text = "" + stats.maxHealth;
        AttackText.text = "" + stats.baseDamage;
        CritChanceText.text = "Crit Chance: " + stats.critChance + "%";
        CritDamageText.text = "Crit Damage: " + stats.critDamage;
        DodgeChanceText.text = "Dodge Chance: " + stats.dodgeChance + "%";
        BlockChanceText.text = "Block Chance: " + stats.blockChance + "%";
        ParryChanceText.text = "Parry Chance: " + stats.parryChance + "%";

        // using MyriadInfo to set color
        MyriadInfo info = new MyriadInfo();
        GetComponent<Image>().color = info.GetPrimaryClassColor(stats.primaryClass);
        CornerPiece1.color = info.GetPrimaryClassColor(stats.subClass);
        CornerPiece2.color = info.GetPrimaryClassColor(stats.subClass);
        UnitSpriteImage.color = info.GetUnitsTitleClassColor(stats.primaryClass, stats.subClass);
        // set sprite icon
        switch(stats.primaryClass)
        {
            case "Warrior":
                    UnitSpriteImage.sprite = WarriorCardIcon;
                    ClassIconImage.sprite = warriorClassIcon;
                    break;
                case "Mage":
                    UnitSpriteImage.sprite = MageCardIcon;
                    ClassIconImage.sprite = mageClassIcon;
                    break;
                case "Ranger":
                    UnitSpriteImage.sprite = RangerCardIcon;
                    ClassIconImage.sprite = rangerClassIcon;
                    break;
                case "Assassin":
                    UnitSpriteImage.sprite = AssassinCardIcon;
                    ClassIconImage.sprite = assassinClassIcon;
                    break;
                case "Conjurer":
                    UnitSpriteImage.sprite = ConjurerCardIcon;
                    ClassIconImage.sprite = conjurerClassIcon;
                    break;
                case "Priest":
                    UnitSpriteImage.sprite = PriestCardIcon;
                    ClassIconImage.sprite = priestClassIcon;
                    break;
                default:
                    // code block
                    break;
        }
        // ----------------------------------------------------------
        // NOW FOR SUBCLASS AND RACE
        switch(stats.subClass)
        {
            case "Warrior":
                    SubClassIconImage.sprite = warriorClassIcon;
                    break;
                case "Mage":
                    SubClassIconImage.sprite = mageClassIcon;
                    break;
                case "Ranger":
                    SubClassIconImage.sprite = rangerClassIcon;
                    break;
                case "Assassin":
                    SubClassIconImage.sprite = assassinClassIcon;
                    break;
                case "Conjurer":
                    SubClassIconImage.sprite = conjurerClassIcon;
                    break;
                case "Priest":
                    SubClassIconImage.sprite = priestClassIcon;
                    break;
                default:
                    // code block
                    break;
        }
        switch(stats.race)
        {
            case "Human":
                    RaceIconImage.sprite = HumanRaceIcon;
                    break;
                case "Orc":
                    RaceIconImage.sprite = OrcRaceIcon;
                    break;
                case "Elf":
                    RaceIconImage.sprite = ElfRaceIcon;
                    break;
                case "Undead":
                    RaceIconImage.sprite = UndeadRaceIcon;
                    break;
                case "Demon":
                    RaceIconImage.sprite = DemonRaceIcon;
                    break;
                default:
                    // code block
                    break;
        }
    }

    // so the reason behind this:
    // At start the unit line items need a reference to the card object so we cannot start the card default inactive
    // so this will wait a few seconds and then inactive it for now
    // TODO: make it so the card is just default INVISABLE (no opacity) and that it doesn't register cicks. then
    // hovering lineitem will just make it full opacity
    void Start()
    {
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) StartCoroutine(HideCardAtStart());
    }

    private IEnumerator HideCardAtStart()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
