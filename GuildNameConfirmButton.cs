using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GuildNameConfirmButton : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] TextMeshProUGUI InvalidText;
    
    public void Confirm()
    {
        if(Player.GetComponent<Player>().IsValidInput()) StartCoroutine(StartGameRoutine());
        else InvalidText.text = "Invalid Input!";
    }

    private IEnumerator StartGameRoutine()
    {
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>().SetTrigger("RightRight");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Nav");
    }
}
