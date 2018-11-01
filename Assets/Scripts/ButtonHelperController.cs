using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHelperController : MonoBehaviour {

    public static ButtonHelperController instance;


    void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this) {
            Destroy(gameObject);
        }
    
    }

    public void PointerUpTriggerEvent(EventTrigger button, Action<PointerEventData> call) {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(data => call.Invoke((PointerEventData)data));
        button.triggers.Add(entry);
    }


}
