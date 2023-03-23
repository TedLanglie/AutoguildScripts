using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // needed for IPointer methods, which is how to get mouse over on UI elements

public class MouseScroll : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float scrollSpeed;
    bool hovering;
    float initialYPos;
    
    void Start()
    {
        initialYPos = transform.position.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }

    void Update()
    {
        if(hovering)
        {
            Vector3 pos = transform.position;
            pos.y += Input.mouseScrollDelta.y * scrollSpeed;
            if(pos.y >= initialYPos) transform.position = pos; // move only if it wouldn't break it over the top (init Y pos)
        }
    }
}
