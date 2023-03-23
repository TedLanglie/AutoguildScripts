using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject InfoCanvas;
    [SerializeField] AudioClip tune;
    void Start()
    {
        // make cursor visable and unlocked
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GMcreator");
    }

    public void GoToInfoScreen()
    {
        SoundManager.instance.PlaySound(tune);
        mainCanvas.SetActive(false);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("MiddleRight");
        GameObject.FindGameObjectWithTag("MusicBox").GetComponent<MusicBoxFader>().TriggerFadeOut();
        StartCoroutine(WaitToActiveNewCanvas());
    }

    private IEnumerator WaitToActiveNewCanvas()
    {
        yield return new WaitForSeconds(.6f);
        InfoCanvas.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game!");
        Application.Quit();
    }
}
