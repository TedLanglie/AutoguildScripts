using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScreensManager : MonoBehaviour
{
    [SerializeField] private GameObject LineupCanvas;
    [SerializeField] private GameObject NavScreen;
    [SerializeField] private GameObject FooterSection;
    private GameObject MainCam;
    bool isMapShowing = false;
    
    void Start()
    {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

/* KEEPING THIS AROUND AS EXAMPLE OF CAMERA LERP
    public void GoToNav()
    {
        // disable all canvases besides nav
        LineupCanvas.SetActive(false);

        // lerp camera in Y axis
        MainCam.GetComponent<CameraLerp>().toggleLerpCamYAxis(1.9f);

        // enable nav canvas
        NavCanvas.SetActive(true);
    }


    public void GoToLineup()
    {
        // disable all canvases besides nav
        NavCanvas.SetActive(false);
        NavScreen.SetActive(false);

        // lerp camera in Y axis
        MainCam.GetComponent<CameraLerp>().toggleLerpCamYAxis(0);

        // enable nav canvas
        LineupCanvas.SetActive(true);
    }
*/
    public void toggleDisplayNavScreen()
    {
        if(isMapShowing)
        {
            isMapShowing = false;
            NavScreen.GetComponent<Animator>().SetTrigger("MapDown");
        }
        else
        {
            isMapShowing = true;
            NavScreen.transform.SetAsLastSibling();
            FooterSection.transform.SetAsLastSibling();
            NavScreen.GetComponent<Animator>().SetTrigger("MapUp");
        } 
    }
}
