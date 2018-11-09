using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {

    public static GameplayManager instance = null;

    public PlayerController playerController;
    public CameraController cameraController;
    public EnvinronmentController envinronmentController;

    private DataController dataInstance;
    private ButtonHelperController btnInstance;

    private List<EnemyController> enemies;

    void Awake() {

        if (instance == null)
            instance = this;
        else if (instance != this)
            instance = this;

   
    }

    void Start() {


        envinronmentController.InitializeGround(playerController.gameObject);

        StartCoroutine("StartGame");

    }

    IEnumerator StartGame() {

        yield return new WaitForSeconds(3f);

        playerController.inGame = true;
        playerController.gameOver = false;
        playerController.StartCoroutine("Movement");
        playerController.StartCoroutine("Attack");
        cameraController.SetTarget(playerController.transform);
        cameraController.cameraMovement = true;

        if (enemies != null) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].StartEnemy();
            }

        }



    }


    public void SetEnemies(EnemyController enemy) {

        if(enemies == null)
            enemies = new List<EnemyController>();

        if (!enemies.Contains(enemy)) {
            enemies.Add(enemy);
        }

    }


}
