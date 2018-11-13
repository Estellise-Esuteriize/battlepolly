using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
   
    private PlayerController playerController;
    private CameraController cameraController;
    private EnvinronmentController envinronmentController;
    private GamePlayMainController playUIController;
    private NoteCompleteController notiController;

    public float startingTime;
    
    private List<EnemyController> enemies;

    private GameController instance;


    IEnumerator Start() {

        instance = GameController.instance;

        while (instance == null) {

            instance = GameController.instance;

            yield return new WaitForSeconds(.1f);

        }

        instance.AddComponentForReference(this);

        playerController = instance.GetComponentForReference<PlayerController>();
        cameraController = instance.GetComponentForReference<CameraController>();
        envinronmentController = instance.GetComponentForReference<EnvinronmentController>();
        playUIController = instance.GetComponentForReference<GamePlayMainController>();
        notiController = instance.GetComponentForReference<NoteCompleteController>();

        while (playerController == null || cameraController == null || envinronmentController == null || playUIController == null || notiController == null) {
            
            playerController = instance.GetComponentForReference<PlayerController>();
            cameraController = instance.GetComponentForReference<CameraController>();
            envinronmentController = instance.GetComponentForReference<EnvinronmentController>();
            playUIController = instance.GetComponentForReference<GamePlayMainController>();
            notiController = instance.GetComponentForReference<NoteCompleteController>();


            yield return new WaitForSeconds(.1f);
        }


        envinronmentController.InitializeGround(playerController.gameObject);

        while (startingTime > 0) {

            startingTime -= Time.deltaTime;

            playUIController.StartingGame(startingTime);

            yield return null;

        }



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

 
    IEnumerator PauseGame() {

        playerController.StopCoroutine("Movement");
        playerController.StopCoroutine("Attack");
        playerController.StopMovement();
        cameraController.cameraMovement = false;

        if (enemies != null) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].PauseEnemy();
            }

        }

        yield return new WaitForSeconds(.1f);
    }

    IEnumerator ResumeGame() {

        playerController.StartCoroutine("Movement");
        playerController.StartCoroutine("Attack");
        cameraController.cameraMovement = true;

        if (enemies != null) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].StartEnemy();
            }

        }

        yield return new WaitForSeconds(.1f);

    }


    IEnumerator GameOver() {

        playerController.StopCoroutine("Movement");
        playerController.StopCoroutine("Attack");
        cameraController.cameraMovement = false;

        if (enemies != null) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].PauseEnemy();
            }

        }

        yield return new WaitForSeconds(.2f);


    }

    IEnumerator CompleteGame(Transform boss) {

        playerController.StopCoroutine("Movement");
        playerController.StopCoroutine("Attack");
        cameraController.cameraMovement = false;

        if (enemies != null) {

            for (int i = 0; i < enemies.Count; i++) {
                enemies[i].PauseEnemy();
            }

        }

        yield return playUIController.StartCoroutine("ShowFairy", boss.position);

        yield return new WaitForSeconds(3f);

        notiController.ShowNotif();

    }



    public void SetEnemies(EnemyController enemy) {

        if(enemies == null)
            enemies = new List<EnemyController>();

        if (!enemies.Contains(enemy)) {
            enemies.Add(enemy);
        }

    }


}
