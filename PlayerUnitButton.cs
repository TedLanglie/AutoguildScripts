using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUnitButton : MonoBehaviour
{
    private int classIndex;
    [SerializeField] private TextMeshProUGUI classTitleText;
    [SerializeField] private TextMeshProUGUI classDescriptionText;
    [SerializeField] private TextMeshProUGUI classHealthText;
    [SerializeField] private TextMeshProUGUI classDamageText;
    [SerializeField] private Image classIcon;
    private Button buttonComponent;
    GameObject createPlayerUnitGameObject;
    public void Initialize(int classInput)
    {
        // initialize some variables
        classIndex = classInput;
        buttonComponent = GetComponent<Button>();
        ColorBlock colors = buttonComponent.colors; // will be used to change the buttons color depending on class
        createPlayerUnitGameObject = GameObject.FindGameObjectWithTag("CreatePlayerUnit");
        // -- TRAIT DICTIONARY:
        // 0 - PROTECTOR
        // 1 - MAGE
        // 2 - RANGER
        // 3 - ASSASSIN
        // 4 - CONJURER
        // 5 - PRIEST
        // --
        // set title class text
        // set icon
        // set description
        // set attack and damage text
        // set background image / color
        switch(classIndex)
        {
            case 0:
                // handle text
                classTitleText.text = "Protector";
                classDescriptionText.text = "Shouldering heaps of iron with brute strength, the protector excels at being a dominating force protecting and commanding the battlefield";
                classHealthText.text = "10";
                classDamageText.text = "4";
                // handle button color
                colors.normalColor = Color.gray;
                buttonComponent.colors = colors;
                break;
            case 1:
                // handle text
                classTitleText.text = "Mage";
                classDescriptionText.text = "Gifted with intellect and magical ability, they use a wide variety of magic to gain the upper hand";
                classHealthText.text = "8";
                classDamageText.text = "1";
                // handle button color
                colors.normalColor = Color.blue;
                buttonComponent.colors = colors;
                break;
            case 2:
                // handle text
                classTitleText.text = "Ranger";
                classDescriptionText.text = "A primal and precise hunter, using traps and arrows along with a fierce connection with the wild to bring a versatile and adaptable combatant";
                classHealthText.text = "6";
                classDamageText.text = "8";
                // handle button color
                colors.normalColor = Color.green;
                buttonComponent.colors = colors;
                break;
            case 3:
                // handle text
                classTitleText.text = "Assassin";
                classDescriptionText.text = "Master of shadows, efficient rogues specialized in taking down enemies as quickly as possible";
                classHealthText.text = "5";
                classDamageText.text = "9";
                // handle button color
                colors.normalColor = Color.black;
                buttonComponent.colors = colors;
                break;
            case 4:
                // handle text
                classTitleText.text = "Conjurer";
                classDescriptionText.text = "Demonic power, these warlocks can raise the brutal demons from below to fight by their side";
                classHealthText.text = "3";
                classDamageText.text = "8";
                // handle button color
                colors.normalColor = Color.magenta;
                buttonComponent.colors = colors;
                break;
            case 5:
                // handle text
                classTitleText.text = "Priest";
                classDescriptionText.text = "Holy divine, devote spiritual healers able to buff and grace their allies";
                classHealthText.text = "2";
                classDamageText.text = "9";
                // handle button color
                colors.normalColor = Color.yellow;
                buttonComponent.colors = colors;
                break;
            default:
                // code block
                break;
        }
    }

    public void ChosenClass()
    {
        createPlayerUnitGameObject.GetComponent<PlayerUnitCreator>().ClassChosen(classIndex);
    }
}
