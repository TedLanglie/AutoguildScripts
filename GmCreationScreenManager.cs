using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GmCreationScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject UnitCreationCanvas;
    [SerializeField] private AudioClip confirm;
    private GameObject MainCam;
    
    void Start()
    {
        MainCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // this function will be called by both buttons to swap between "screens", parameter "true" means switch
    // to guild naming scene, false means go back to unit creation screen
    public void ContinueToNav()
    {
            StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        GameObject.FindGameObjectWithTag("MusicBox").GetComponent<MusicBoxFader>().TriggerFadeOut();
        SoundManager.instance.PlaySound(confirm);
        // disable all canvases besides nav
        UnitCreationCanvas.SetActive(false);
        MainCam.GetComponent<Animator>().SetTrigger("MiddleRight");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Nav");
    }
}
