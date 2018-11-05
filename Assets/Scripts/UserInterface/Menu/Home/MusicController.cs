using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicController : MonoBehaviour {

    public Sprite musicOn;
    public Sprite musicOff;

    private EventTrigger music;
    private Image image;

    private bool isMusic;

    private ButtonHelperController helper;
    private DataController data;

    void Start() {

        helper = ButtonHelperController.instance;
        data = DataController.instance;

        image = GetComponent<Image>();
        music = GetComponent<EventTrigger>();


        isMusic = data.dataFile.music;

        // disable - enable music base on isMusic value


        // add event trigger 
        helper.PointerUpTriggerEvent(music, MusicControl);

    }

    void MusicControl(PointerEventData data) {

        isMusic = !isMusic;

        print(isMusic);

        if (isMusic) {
            image.sprite = musicOn;
            // enabile music in here
        }
        else {
            image.sprite = musicOff;
            // disable music in here

        }

    }



}
