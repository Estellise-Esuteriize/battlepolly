using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobilePhone : MonoBehaviour {


    public float speed;

    public float time;

    private Dictionary<float, EnemyController> enemies;

    GameController instance;

    void Start() {

        instance = GameController.instance;

        instance.AddComponentForReference(this);

        enemies = new Dictionary<float, EnemyController>();

        gameObject.SetActive(false);

    }



    public void UseMobilePhone(Vector3 playerPosition) {

        Vector3 position = playerPosition;
        position.y = 0f;
        position.x -= 5f;

        transform.position = position;

        gameObject.SetActive(true);

        StartCoroutine("Movement");
      
    }

    IEnumerator Movement() {

        float timer = 0f;

        while (timer < time) {

            timer += Time.deltaTime;

            transform.Translate(Vector3.right * speed * Time.deltaTime);

            yield return null;
        }

        gameObject.SetActive(false);

    }

    void OnCollisionEnter2D(Collision2D collision) {

        bool isEnemies = collision.gameObject.CompareTag("Enemies");
        bool isBoss = collision.gameObject.CompareTag("Boss");

        EnemyController enemy = null;

        if (isEnemies) {
            

            if (!enemies.ContainsKey(collision.gameObject.GetInstanceID())) {

                enemy = collision.gameObject.GetComponent<EnemyController>();

                enemies.Add(collision.gameObject.GetInstanceID(), enemy);

            }
            else {
                enemy = enemies[collision.gameObject.GetInstanceID()];
            }

            enemy.TakeDamage();
            
        }
        else if (isBoss) {
       

            if (!enemies.ContainsKey(collision.gameObject.GetInstanceID())) {

                enemy = collision.gameObject.GetComponent<EnemyController>();

                enemies.Add(collision.gameObject.GetInstanceID(), enemy);

            }
            else {
                enemy = enemies[collision.gameObject.GetInstanceID()];
            }

            enemy.BossTakeDamage();
            
        }


    }

}
