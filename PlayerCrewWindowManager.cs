using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrewWindowManager : MonoBehaviour
{
    private GameObject playerObject;
    private Player playerComponent;
    [Header("Needed Components")]
    [SerializeField] GameObject UnitLineItemPrefab;
    [SerializeField] private GameObject PlayerInvisPanel;
    [SerializeField] private GameObject PopUpButtonObject;
    private Animator popUpAnim;
    bool popUpActive = false;
    [Header("Tutorial Component")]
    GameObject TutorialText1;
    GameObject TutorialText2;

    void Awake()
    {
        popUpAnim = GetComponent<Animator>();
        playerObject = GameObject.FindGameObjectWithTag("Player"); // get player object
        playerComponent = playerObject.GetComponent<Player>();

        if(playerComponent.DayCount == 1)
            {
                TutorialText1 = GameObject.FindGameObjectWithTag("TutorialText1");
                TutorialText2 = GameObject.FindGameObjectWithTag("TutorialText2");
            }
        else if(GameObject.FindGameObjectWithTag("PartyManager") != null) GameObject.FindGameObjectWithTag("TutorialText1").SetActive(false);
    }

    // Update is called once per frame
    void Start()
    {
        RefreshPlayerUnitsList();
    }

    public void RefreshPlayerUnitsList()
    {
        // wipe all existing PLAYER line items
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("UnitLineItem");   
        foreach (GameObject foundObject in taggedObjects) {
            if(!foundObject.GetComponent<UnitLineItemShop>().isShopItem) Destroy(foundObject); // destroy line item if it isn't a shop item
        }

        // build new line items for UI
        for(int i = 0; i < playerComponent.TotalUnitList.Count; i++)
        {
            GameObject testObject = Instantiate(UnitLineItemPrefab, PlayerInvisPanel.transform.position + new Vector3(0, -85*i -80, 0), Quaternion.identity, PlayerInvisPanel.transform);
            testObject.GetComponent<UnitLineItemShop>().SetAssociatedUnit(playerComponent.TotalUnitList[i], false);
        }
    }

    public void PopUpButtonClicked()
    {
        // if player crew window is already up and active
        if(popUpActive)
        {
            // swap button sprite to point down
            PopUpButtonObject.transform.rotation = Quaternion.Euler(0, 0, 0); // rotate X axis 180
            // play animation to move up
            popUpAnim.SetTrigger("GoUp");
            // set new bool
            popUpActive = false;
        }
        else
        {
            // swap button sprite to point up
            PopUpButtonObject.transform.rotation = Quaternion.Euler(180, 0, 0); // rotate X axis 0
            // play animation to move down
            popUpAnim.SetTrigger("GoDown");
            // set new bool
            popUpActive = true;
            if(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().DayCount == 1)
            {
                TutorialText1.SetActive(false);
                TutorialText2.SetActive(true);
            }
        }
    }

    // triggers go away animatio, should be used when leaving nav, shop, and battles
    public void GoAway()
    {
        popUpAnim.SetTrigger("GoAway");
    }
}
