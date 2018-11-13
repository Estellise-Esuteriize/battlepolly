using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler{

    [HideInInspector]
    public float movingPressed = 0f;
    [HideInInspector]
    public bool isPressed = false;
    

    public void OnPointerDown(PointerEventData eventData) {
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        movingPressed = 1f;
    }

    public void OnPointerExit(PointerEventData eventData) {
        movingPressed = 0f;
    }

    public void OnPointerUp(PointerEventData eventData) {
        isPressed = true;
    }


}
