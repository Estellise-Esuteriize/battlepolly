using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    private Transform target;

    private Vector3 offset;

    [HideInInspector]
    public bool cameraMovement;

    GameController instance;

    IEnumerator Start() {

        instance = GameController.instance;

        while (instance == null) {
            instance = GameController.instance;

            yield return new WaitForSeconds(.1f);
        }

        instance.AddComponentForReference(this);

    }

    public void SetTarget(Transform newTarget) {

        target = newTarget;

        offset = transform.position - target.position;

    }

    void LateUpdate() {

        if (cameraMovement) {

            Vector3 velocity = Vector2.zero;

            velocity.x = target.position.x + offset.x;
            velocity.z = offset.z;

            transform.position = velocity;

        }

    }




}
