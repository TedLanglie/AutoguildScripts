using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // needed for IPointer methods, which is how to get mouse over on UI elements

public class MouseDownExit : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] GameObject UnitCardObject;
    [SerializeField] GameObject TraitsSectionObject;
    // Player has clicked outside cards
    public void OnPointerDown(PointerEventData eventData)
    {
        // wipe all existing trait button objects (prevents stacking)
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("TraitButton");   
        foreach (GameObject foundObject in taggedObjects) {
            Destroy(foundObject); // destroy trait button object
        }

        // hide those two UIs
        UnitCardObject.SetActive(false);
        TraitsSectionObject.SetActive(false);
    }
}
