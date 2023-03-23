using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    /*
        ===========================
        This script is attached to partymanager

        PURPOSE:
        Keep track of what location the player is at as well as
        hold data for locations, and interacting with the Nav Canvas
        ===========================
    */ 
    public string currentLocation;

    public void PartyLocationSet(int dayNum)
    {
        switch(dayNum)
        {
            case 1:
                currentLocation = "Arena";
            break;
            case 2:
                currentLocation = "Shop";
            break;
            case 3:
                currentLocation = "Arena";
            break;
            case 4:
                currentLocation = "Arena";
            break;
            case 5:
                currentLocation = "Shop";
            break;
            default:
                if(dayNum % 5 == 0) currentLocation = "Shop";
                else currentLocation = "Arena";
            break;
        }
    }

    public void ChangeLocation(string inputLocation)
    {
        Debug.Log("Change Location to: " + inputLocation);
        currentLocation = inputLocation;
    }

    // this will run when player hits Embark* (beam down, go, etc)
    public void EmbarkLocation()
    {
        GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().GiveLineupToPlayer(); // sets lineup list in player object

        SceneManager.LoadScene(currentLocation); // loads scene of current location
    }
}
