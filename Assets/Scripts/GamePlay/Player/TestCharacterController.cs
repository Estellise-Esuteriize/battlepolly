using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacterController : MonoBehaviour {

    //public GameObject bullet;
    public Transform direction;

    public float speed = 5f;


    private CharacterController controller;


    //private float lastMoveX = 1f;
    private bool isGameOver = false;

    void Awake() {
        controller = GetComponent<CharacterController>();
    }


    void Start() {
        StartCoroutine("GamePlay");
    }

    IEnumerator GamePlay() {

        while (!isGameOver) {

            CharacterMovement();

            yield return null;
        }


    }


    void CharacterMovement() {

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 movement = Vector2.zero;
        movement.x = moveX;
        movement.y = moveY;
        
        
        controller.Move(movement * speed * Time.deltaTime);

        if (moveX != 0f) {
            Vector2 facing = direction.transform.localPosition;
            facing.x = Mathf.Abs(facing.x);

            direction.transform.localPosition = facing * moveX;
        }

    }

    void FireBullet() {

        if (Input.GetKeyDown(KeyCode.Space)) {

        }

    }

    private void OnEnable() {
        print("parent is Enabled");
    }


    private void OnDisable() {
        print("parent is Disabled");
    }


}

