using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    
    bool isSettingMenuShowing = false;
    [SerializeField] GameObject SettingsMenuPanel;
    [SerializeField] Slider SoundSlider;
    [SerializeField] AudioClip tune;
    public void ToggleSettingMenu()
    {
        if(!isSettingMenuShowing)
        {
            SettingsMenuPanel.SetActive(true);
            isSettingMenuShowing = true;
            SoundSlider.value = AudioListener.volume;
            SoundManager.instance.PlaySound(tune);
        }
        else
        {
            SettingsMenuPanel.SetActive(false);
            isSettingMenuShowing = false;
        } 
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game!");
        Application.Quit();
    }

    public void QuitRun()
    {
        Debug.Log("Quitting run!");
        foreach(GameObject unit in GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TotalUnitList)
        {
            Destroy(unit); // if unit is dead, DESTROY gameobject
        }
        Destroy(GameObject.FindGameObjectWithTag("Player")); // if unit is dead, DESTROY gameobject
        SceneManager.LoadScene("MainMenu");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingMenu();
        }
    }
}
