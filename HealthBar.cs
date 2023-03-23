using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    private GameObject _assignedUnit;
    [SerializeField] float _SpeedOfBar = 10;
    [SerializeField] float _TimeTillDecrease = 1;
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
    [Header("UI COMPONENTS")]
    private Slider redSlider;
    private Slider whiteSlider;
    [SerializeField] TextMeshProUGUI HealthText;
    [SerializeField] TextMeshProUGUI AttackText;
    [SerializeField] TextMeshProUGUI NameText;
    [SerializeField] TextMeshProUGUI LevelAndClassText;
    [SerializeField] Image BoxUIBG;
    [SerializeField] Image RaceIcon;
    [SerializeField] Image PrimClassIcon;
    [SerializeField] Image SubClassIcon;
    private UnitBattle currentUnitBattle; // holds UnitBattle script of attached unit
    [Header("Animation")]
    public Animator healthChangeAnim;
    public Animator damageChangeAnim;
    private float lastRecordedHealth = 0;
    private float lastRecordedDamage = 0;
    private UnitStats statsOfAssignedUnit;

    void Start()
    {
        // style
        StyleHPbar();
        // set assigned unit to current unit script to not say getcomponent all the time
        currentUnitBattle = _assignedUnit.GetComponent<UnitBattle>();

        whiteSlider = GetComponent<Slider>();
        redSlider = this.transform.Find("RedHealthBar").GetComponent<Slider>();

        // set the two sliders max to be the attached units max health
        redSlider.maxValue = statsOfAssignedUnit.maxHealth;
        whiteSlider.maxValue = redSlider.maxValue;
        lastRecordedHealth = currentUnitBattle.CurrentHealth;
        lastRecordedDamage = currentUnitBattle.CurrentDamage;

        // move healthbar up or down, alot of changes for enemy health bar
        if(currentUnitBattle.isPlayer) transform.parent.gameObject.transform.position += new Vector3(-1.3f, 0, 0);
        else
        {
            transform.parent.gameObject.transform.position += new Vector3(1.3f, 0, 0); // move
            Vector3 localFlipScale = transform.parent.gameObject.transform.localScale; // flip Object
            localFlipScale.x *= -1;
            transform.parent.gameObject.transform.localScale = localFlipScale;

            // flip scale of all icons
            Vector3 localIconFlipScale = RaceIcon.gameObject.transform.localScale; // flip Object
            localIconFlipScale.x *= -1;
            RaceIcon.gameObject.transform.localScale = localIconFlipScale;
            PrimClassIcon.gameObject.transform.localScale = localIconFlipScale;
            SubClassIcon.gameObject.transform.localScale = localIconFlipScale;

            // flip text
            // health and attack
            Vector3 localHealthAndAttackFlipScale = AttackText.gameObject.transform.localScale; // flip Object
            localHealthAndAttackFlipScale.x *= -1;
            AttackText.gameObject.transform.localScale = localHealthAndAttackFlipScale;
            HealthText.gameObject.transform.localScale = localHealthAndAttackFlipScale;
            // name  text
            Vector3 localNameFlipScale = NameText.gameObject.transform.localScale; // flip Object
            localNameFlipScale.x *= -1;
            NameText.gameObject.transform.localScale = localNameFlipScale;
            // Level text
            Vector3 localLevelFlipScale = LevelAndClassText.gameObject.transform.localScale; // flip Object
            localLevelFlipScale.x *= -1;
            LevelAndClassText.gameObject.transform.localScale = localLevelFlipScale;

            // change alignment of text
            NameText.alignment = TMPro.TextAlignmentOptions.Right;
            LevelAndClassText.alignment = TMPro.TextAlignmentOptions.Right;
        } 

        // setting start test elements, these shouldn't change during combat so setting them here instead of Update is better
        NameText.text = "" + statsOfAssignedUnit.name;
        LevelAndClassText.text = "Level " + statsOfAssignedUnit.level + " " + statsOfAssignedUnit.titleClass;
    }

    void Update()
    {
        if(_assignedUnit == null) return;
        // for animations
        // health
        if(lastRecordedHealth != currentUnitBattle.CurrentHealth)
        {
            healthChangeAnim.SetTrigger("ChangeOfHealth");
        } 
        lastRecordedHealth = currentUnitBattle.CurrentHealth;
        // damage
        if(lastRecordedDamage != currentUnitBattle.CurrentDamage)
        {
            damageChangeAnim.SetTrigger("ChangeOfAttack");
        } 
        lastRecordedDamage = currentUnitBattle.CurrentDamage;

        redSlider.value = currentUnitBattle.CurrentHealth;

        // for triggering white health bar effect
        if(whiteSlider.value != redSlider.value)
        {
            StartCoroutine(lerpToNewValue(_SpeedOfBar));
        }

        // set color
        UpdateColor();

        // setting text elements. might be better way to do this!
        if(currentUnitBattle.CurrentHealth > 0) HealthText.text = "" + currentUnitBattle.CurrentHealth;
        else HealthText.text = "" + 0; // if health is less than 0 just show 0!
        AttackText.text = "" + currentUnitBattle.CurrentDamage; // probably would be better to have two seperate variables for base damage and current damage, cause buffs and we dont want buffs to change base damage
    }

    private IEnumerator lerpToNewValue(float speed)
    {
        yield return new WaitForSeconds(_TimeTillDecrease);

        float time = 0;
        float lerpValue = whiteSlider.value;
        while (Mathf.Abs(whiteSlider.value - redSlider.value) > .001)
        {
            lerpValue = Mathf.Lerp(whiteSlider.value, redSlider.value, time * speed);
            whiteSlider.value = lerpValue;
            time += Time.deltaTime;
            yield return null;
        }
        whiteSlider.value = redSlider.value;
    }

    private void UpdateColor()
    {
        Color colorToChangeTo = new Color32(255, 6, 6, 255);
        // check status of unit
    
        if(lastRecordedHealth >= _assignedUnit.GetComponent<UnitStats>().maxHealth) colorToChangeTo = new Color32(183, 255, 71, 255);
        if(lastRecordedHealth < _assignedUnit.GetComponent<UnitStats>().maxHealth) colorToChangeTo = new Color32(255, 130, 59, 255);
        if(lastRecordedHealth < _assignedUnit.GetComponent<UnitStats>().maxHealth * .3) colorToChangeTo = new Color32(255, 6, 6, 255);

        // if unit is at max or above max HP, it is green, Below max it is orange, at 10% or lower, it is red
        // set color of lineitem based on primary class of associated unit
        
        redSlider.gameObject.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = colorToChangeTo;
        // ----------------------------------------------------------
    }

    private void StyleHPbar()
    {
        MyriadInfo info = new MyriadInfo();
        BoxUIBG.color = info.GetPrimaryClassColor(statsOfAssignedUnit.primaryClass);

        switch(statsOfAssignedUnit.race)
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
        }

        switch(statsOfAssignedUnit.primaryClass)
        {
            case "Warrior":
                PrimClassIcon.sprite = warriorClassIcon;
                break;
            case "Mage":
                PrimClassIcon.sprite = mageClassIcon;
                break;
            case "Ranger":
                PrimClassIcon.sprite = rangerClassIcon;
                break;
            case "Assassin":
                PrimClassIcon.sprite = assassinClassIcon;
                break;
            case "Conjurer":
                PrimClassIcon.sprite = conjurerClassIcon;
                break;
            case "Priest":
                PrimClassIcon.sprite = priestClassIcon;
                break;
        }

        switch(statsOfAssignedUnit.subClass)
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
        }
    }

    public void setAssigningUnit(GameObject unit)
    {
        _assignedUnit = unit;
        statsOfAssignedUnit = _assignedUnit.GetComponent<UnitStats>();
    }

    public GameObject getAssignedUnit()
    {
        return _assignedUnit;
    }
}
