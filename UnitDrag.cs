using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDrag : MonoBehaviour
{
    bool dragging = false;
    bool isCollidingWithOutOfLineupBox;
    GameObject collidingEmptyPos;
    GameObject collidingUnit;
    Vector3 initialPosition;
    Vector3 mousePositionWhenClicked;
    GameObject unitInfoHUD;
    void OnMouseDown()
    {
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) return; // to prevent dragging outside of nav scene
        initialPosition = gameObject.transform.position;
        // Destroy the gameObject after clicking on it
        GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().MakePlatformGradientsAppear(true); // line to make grads appear/disappear
        mousePositionWhenClicked = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        dragging = true;
    }

    void OnMouseUp()
    {
        if(GameObject.FindGameObjectWithTag("PartyManager") == null) return; // to prevent dragging outside of nav scene
        GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().MakePlatformGradientsAppear(false); // line to make grads appear/disappear
        dragging = false;
        transform.position = initialPosition;
        // if we are colliding with something, call the swap function with each objects index
        if(collidingUnit != null)
        {
            GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().SwapUnitIndex(gameObject, collidingUnit);  
        }
        // if were only colliding with an empty slot, swap with empty
        if(collidingUnit == null && collidingEmptyPos != null)
        {
            GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().SwapUnitWithEmpty(gameObject, collidingEmptyPos);
        }
        // if were only colliding with an the empty lineup box, remove the unit from lineup
        if(isCollidingWithOutOfLineupBox)
        {
            GameObject.FindGameObjectWithTag("PartyManager").GetComponent<PartyManager>().SwapUnitOutOfLineup(gameObject);
        }
    }

    void Update()
    {
        if(dragging && mousePositionWhenClicked != new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0))
        {
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Unit")
        {
            collidingUnit = other.gameObject;
        }
        if(other.gameObject.tag == "EmptyPos")
        {
            collidingEmptyPos = other.gameObject;
        }
        if(other.gameObject.tag == "EmptyLineupBox")
        {
            isCollidingWithOutOfLineupBox = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Unit")
        {
            collidingUnit = null;
        }
        if(other.gameObject.tag == "EmptyPos")
        {
            collidingEmptyPos = other.gameObject;
        }
        if(other.gameObject.tag == "EmptyLineupBox")
        {
            isCollidingWithOutOfLineupBox = false;
        }
    }
}
