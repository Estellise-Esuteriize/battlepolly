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

    GameController instance;
    ButtonHelperController helper;
    DataController data;
    SoundController sound;

    void Start() {
        instance = GameController.instance;
        helper = instance.buttonHelper;
        data = instance.dataController;
        sound = instance.soundController;

        image = GetComponent<Image>();
        music = GetComponent<EventTrigger>();


        isMusic = data.dataFile.music;

        // disable - enable music base on isMusic value

        if (isMusic)
            image.sprite = musicOn;
        else if (!isMusic)
            image.sprite = musicOff;

        sound.OnOffSFX(isMusic);


        // add event trigger 
        helper.PointerUpTriggerEvent(music, MusicControl);

    }

    void MusicControl(PointerEventData data) {

        isMusic = !isMusic;

        DataFile dta = this.data.dataFile;

        if (isMusic) 
            image.sprite = musicOn;
        else 
            image.sprite = musicOff;


        sound.OnOffSFX(isMusic);

        dta.music = isMusic;

        this.data.dataFile = dta;

    }



}
