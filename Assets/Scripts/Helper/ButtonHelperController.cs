using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonHelperController : MonoBehaviour {

    public static ButtonHelperController instance;

    private Dictionary<EventTrigger, EventTrigger.Entry> listOfListener;

    void Awake() {



        if (instance == null)
            instance = this;
        else if (instance != this) {
            Destroy(gameObject);
        }

        listOfListener = new Dictionary<EventTrigger, EventTrigger.Entry>();
    }

    /*[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void OnButtonHelperNewScene() {
        instance.listOfListener = new Dictionary<EventTrigger, EventTrigger.Entry>();

    }*/

    public void PointerUpTriggerEvent(EventTrigger button, Action<PointerEventData> call) {

        if (listOfListener.ContainsKey(button)) {
            button.triggers.Remove(listOfListener[button]);
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(data => call.Invoke((PointerEventData)data));
        button.triggers.Add(entry);

        listOfListener.Add(button, entry);
    }

    public void PointerUpTriggerEventWithParameters(EventTrigger button, UnityEngine.Events.UnityAction<BaseEventData> call) {

        if (listOfListener.ContainsKey(button)) {
            button.triggers.Remove(listOfListener[button]);
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(call);
        button.triggers.Add(entry);

        listOfListener.Add(button, entry);
    }

}
