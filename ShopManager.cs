using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("Needed Components")]
    GameObject UnitCardObject;
    [SerializeField] private GameObject ShopCanvas;
    [SerializeField] private GameObject ShopInfoPanel;
    [SerializeField] private GameObject ShopUnitsPanel;
    [SerializeField] private TextMeshProUGUI ShopInfoDescriptionText;
    [SerializeField] private GameObject ShopInvisPanel;
    [SerializeField] GameObject UnitLineItemPrefab;
    [Header("Background Sprites and Component")]
    [SerializeField] private SpriteRenderer ShopBGSprite;
    [SerializeField] private List<Sprite> ShopBGS = new List<Sprite>(); // index should be same as Shop Types
    [Header("Sound Components")]
    [SerializeField] private AudioClip ReturnButtonSound;
    
    [Header("Generation Variables")]
    int HighestPlayerLevel;
    public int numOfUnitsToGenerate;
    public List<GameObject> UnitsInShop = new List<GameObject>(); // keep track of what units are in shop
    private string[] ShopTypes = {"Magistral Academy", "Thieves Den", "Holy Keep", "Hunters Clan", "Lich Crypt", "Elven City", "Warmongers Barracks", "Onyx Crusade", "Akuma Hellpit", "Necromancy Citadel", 
                                    "Nomad Outpost", "Mercenary Band"};
    private string[] ClassNamesArray = {"Warrior", "Mage", "Ranger", "Assassin", "Conjurer", "Priest"}; // these two names arrays are for when you want all of them
    private string[] RaceNamesArray = {"Human", "Orc", "Elf", "Undead", "Demon"};
    private string ChosenShopType = "";
    private List<string> ChosenRacialTypes = new List<string>();
    private List<string> ChosenPrimaryClassTypes = new List<string>();
    private List<string> ChosenSubClassTypes = new List<string>();


    void Start()
    {
        UnitCardObject = GameObject.FindGameObjectWithTag("UnitCard");
        RollNewUnitLineItems();
        GetAllUnitsOffscreen();
    }

    private void GetAllUnitsOffscreen()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("Unit");   
        foreach (GameObject foundObject in taggedObjects) {
            foundObject.transform.position = new Vector3(20, 20, 0); // move unit offscreen
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            UnitCardObject.SetActive(false); // remove card manager if user clicks no matter what
    }

    public void ReturnToNav()
    {
        StartCoroutine(ReturnNavRoutine());
    }

    private IEnumerator ReturnNavRoutine()
    {
        GameObject.FindGameObjectWithTag("MusicBox").GetComponent<MusicBoxFader>().TriggerFadeOut();
        SoundManager.instance.PlaySound(ReturnButtonSound);
        UnitCardObject.SetActive(false);
        ShopInfoPanel.GetComponent<Animator>().SetTrigger("GoAway");
        ShopUnitsPanel.GetComponent<Animator>().SetTrigger("GoAway");
        GameObject.FindGameObjectWithTag("PlayerCrewWindowManager").GetComponent<PlayerCrewWindowManager>().GoAway(); 
        GameObject.FindGameObjectWithTag("FooterImage").GetComponent<FooterImage>().ActivateDropDown();
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("MiddleRight");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Nav"); // loads scene of current location
    }

    public void RollNewUnitLineItems()
    {
        HighestPlayerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().getHighestLevelOfUnitsPlayerHas();
        // choose shop type
        int ShopTypeRoll = Random.Range(0, ShopTypes.Length);
        ChosenShopType = ShopTypes[ShopTypeRoll]; // now chosen shop type is correct, lets get generation variables for units set
        ShopBGSprite.sprite = ShopBGS[ShopTypeRoll]; // set BG
        Image ShopInfoPanelImage = ShopInfoPanel.GetComponent<Image>(); // get image of info panel
        Color32 ColorToChangeTo = new Color32(223, 255, 255, 255);
        switch(ChosenShopType)
        {
            case "Magistral Academy": 
                ChosenRacialTypes = new List<string>(RaceNamesArray);
                ChosenPrimaryClassTypes.Add("Conjurer");
                ChosenPrimaryClassTypes.Add("Priest");
                ChosenPrimaryClassTypes.Add("Mage");
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(40, 235, 255, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#28EBFF>Magistral Academy<color=#FFFF>, with many <color=#129CFF>spellcasters<color=#FFFF> from all different creeds.";
                break;
            case "Thieves Den": 
                ChosenRacialTypes = new List<string>(RaceNamesArray);
                ChosenPrimaryClassTypes.Add("Assassin");
                ChosenSubClassTypes.Add("Warrior");
                ChosenSubClassTypes.Add("Assassin");
                ChosenSubClassTypes.Add("Ranger");
                ChosenSubClassTypes.Add("Conjurer");
                ColorToChangeTo = new Color32(28, 22, 91, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#1D165C>Thieves Den<color=#FFFF>, a deadly group of <color=#4724D7>assassins<color=#FFFF>, looking for work and looking for kills.";
                break;
            case "Holy Keep": 
                ChosenRacialTypes.Add("Human");
                ChosenRacialTypes.Add("Elf");
                ChosenPrimaryClassTypes.Add("Priest");
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(255, 210, 45, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#FFD22D>Holy Keep<color=#FFFF>, a divine and spiritual group consisting of <color=#FFBC36>priests<color=#FFFF>.";
                break;
            case "Hunters Clan": 
                ChosenRacialTypes = new List<string>(RaceNamesArray);
                ChosenPrimaryClassTypes.Add("Ranger");
                ChosenPrimaryClassTypes.Add("Assassin");
                ChosenSubClassTypes.Add("Warrior");
                ChosenSubClassTypes.Add("Assassin");
                ChosenSubClassTypes.Add("Ranger");
                ColorToChangeTo = new Color32(136, 200, 86, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#88C856>Hunters Clan<color=#FFFF>, a relentless team of gifted <color=#57C339>rangers<color=#FFFF>.";
                break;
            case "Lich Crypt": 
                ChosenRacialTypes.Add("Undead");
                ChosenPrimaryClassTypes = new List<string>(ClassNamesArray);
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(115, 255, 255, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#00FFF5>Lich Crypt<color=#FFFF>, housing <color=#00FFF5>Undeads<color=#FFFF> with various skill sets.";
                break;
            case "Elven City": 
                ChosenRacialTypes.Add("Elf");
                ChosenPrimaryClassTypes = new List<string>(ClassNamesArray);
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(255, 184, 255, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across an <color=#00FFB8>Elvish City<color=#FFFF>, containing <color=#00FFB8>Elves<color=#FFFF> with various skill sets."; 
                break;
            case "Warmongers Barracks": 
                ChosenRacialTypes.Add("Orc");
                ChosenRacialTypes.Add("Demon");
                ChosenPrimaryClassTypes = new List<string>(ClassNamesArray);
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(255, 49, 0, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#FF3100>Warmongers Barracks<color=#FFFF>, these <color=#FF4242>Demons<color=#FFFF> and <color=#FF3100>Orcs<color=#FFFF> are hungry for a chance at battle.";
                break;
            case "Onyx Crusade": 
                ChosenRacialTypes = new List<string>(RaceNamesArray);
                ChosenPrimaryClassTypes.Add("Warrior");
                ChosenPrimaryClassTypes.Add("Priest");
                ChosenSubClassTypes.Add("Warrior");
                ChosenSubClassTypes.Add("Priest");
                ChosenSubClassTypes.Add("Mage");
                ColorToChangeTo = new Color32(145, 32, 0, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across the <color=#912000>Onyx Crusade<color=#FFFF>, a righteous group of <color=#FFD22D>paladins<color=#912000> on a divine conquest.";
                break;
            case "Akuma Hellpit": 
                ChosenRacialTypes.Add("Demon");
                ChosenPrimaryClassTypes = new List<string>(ClassNamesArray);
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(255, 0, 0, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across an <color=#FF0000>Akuma Hellpit<color=#FFFF>, a gruesome sight, filled to the brim with deadly <color=#FF4242>demons<color=#FFFF>.";
                break;
            case "Necromancy Citadel": 
                ChosenRacialTypes.Add("Demon");
                ChosenRacialTypes.Add("Undead");
                ChosenRacialTypes.Add("Orc");
                ChosenPrimaryClassTypes.Add("Conjurer");
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(123, 0, 116, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#7B0074>Necromancy Citadel<color=#FFFF>, a cold and eerie ancient building, with all sorts of <color=#DF37FF>evil<color=#FFFF> brewing.";
                break;
            case "Nomad Outpost": 
                ChosenRacialTypes.Add("Human");
                ChosenPrimaryClassTypes.Add("Warrior");
                ChosenPrimaryClassTypes.Add("Mage");
                ChosenPrimaryClassTypes.Add("Ranger");
                ChosenPrimaryClassTypes.Add("Assassin");
                ChosenPrimaryClassTypes.Add("Priest");
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(213, 255, 255, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#D5FFFF>Nomad Outpost<color=#FFFF>, a bustling and quiet village with many <color=#FFCA6E>Human<color=#FFFF> adventurers.";
                break;
            case "Mercenary Band": 
                ChosenRacialTypes.Add("Human");
                ChosenRacialTypes.Add("Elf");
                ChosenRacialTypes.Add("Orc");
                ChosenPrimaryClassTypes = new List<string>(ClassNamesArray);
                ChosenSubClassTypes = new List<string>(ClassNamesArray);
                ColorToChangeTo = new Color32(176, 140, 137, 255); // R, G, B, ALPHA; // R, G, B, ALPHA
                ShopInfoDescriptionText.text = "You've come across a <color=#B08C89>Mercenary Band<color=#FFFF>, a cold-blooded group of various creeds and capability.";
                break;
        }
        ShopInfoPanelImage.color = ColorToChangeTo; // R, G, B, ALPHA; // R, G, B, ALPHA
        Debug.Log(ChosenShopType);
        // generate "numOfUnitsToGenerate" number of units to place in UnitsInShop list.
        // then, loop through the list, and spawn a LineItemShop prefab element and give each unit their own.
        for(int i = 0; i < numOfUnitsToGenerate; i++)
        {
            GameObject testObject = Instantiate(UnitLineItemPrefab, ShopInvisPanel.transform.position + new Vector3(0, -85*i, 0), Quaternion.identity, ShopInvisPanel.transform);
            testObject.GetComponent<UnitLineItemShop>().SetAssociatedUnit(GenerateUnit(), true);
        }

    }

    GameObject GenerateUnit()
    {
        // calculate level
        int levelToPassToGenerator = 0;
        int rollMin = HighestPlayerLevel - 2;
        if(rollMin <= 0) rollMin = 1;
        int rollMax = HighestPlayerLevel + 2;
        if(rollMax >= 10) rollMax = 10; // 10 WILL NOT ALWAYS BE THE MAX, THIS NUM SHOULD CHANGE TO WHATEVER I DECIDE MAX TO BE

        levelToPassToGenerator = Random.Range(rollMin, rollMax+1);

        if(HighestPlayerLevel <= 1) levelToPassToGenerator = 1; // only give level 1 if highest level is 1

        // randomly choose race, class, subclass
        string chosenRace = ChosenRacialTypes[Random.Range(0, ChosenRacialTypes.Count)];
        string chosenClass = ChosenPrimaryClassTypes[Random.Range(0, ChosenPrimaryClassTypes.Count)];
        string chosenSubclass = ChosenSubClassTypes[Random.Range(0, ChosenSubClassTypes.Count)];
        return GetComponent<GenerateUnit>().GenerateRandomUnit(levelToPassToGenerator, true, chosenRace, chosenClass, chosenSubclass);
    }
}
