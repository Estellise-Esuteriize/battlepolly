using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {


    public AudioSource sfx;

    public AudioClip playerHit;

    public bool music;

    public void OnOffSFX(bool isMusic) {

        music = isMusic;

        sfx.mute = !isMusic;

    }

    public void PlayAttackSfx() {

        sfx.Stop();

        if (music && !sfx.isPlaying) {

            sfx.clip = playerHit;

            sfx.Play();

        }
    }



}
