using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAtStart : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(HideCardAtStart());
    }

    private IEnumerator HideCardAtStart()
    {
        yield return new WaitForSeconds(.1f);
        gameObject.SetActive(false);
    }
}
