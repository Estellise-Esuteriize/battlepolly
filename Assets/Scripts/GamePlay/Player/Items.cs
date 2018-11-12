using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour, IButtonHandler {

    public PlayerController playerController;
    public Animator animator;


    public bool useSheild;

    public void DownHandler() {
        throw new System.NotImplementedException();

    }

    public void PressedHandler() {
        throw new System.NotImplementedException();
    }

    public void ReleasedHandler() {
        throw new System.NotImplementedException();
    }

    public void UpHandler() {
        throw new System.NotImplementedException();
    }

    void Awake() {

        playerController = transform.root.GetComponent<PlayerController>();
        animator = transform.root.GetComponent<Animator>();

    }
    




}
