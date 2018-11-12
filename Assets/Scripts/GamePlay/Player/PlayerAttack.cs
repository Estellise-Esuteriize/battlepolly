using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    
    private Dictionary<float, EnemyController> enemies = new Dictionary<float, EnemyController>();


    private void OnTriggerEnter2D(Collider2D collision) {

        if (transform.name != "Spear")
            return;

        string isEnemies = collision.tag;

        if (isEnemies == "Enemies") {

            float myY = transform.position.y + -.5f;
            float enemyY = collision.transform.position.y;

            if (myY + .3f >= enemyY && myY + -.3f <= enemyY) {

                EnemyController enemy = null;

                if (!enemies.ContainsKey(collision.gameObject.GetInstanceID())) {

                    enemy = collision.gameObject.GetComponent<EnemyController>();

                    enemies.Add(collision.gameObject.GetInstanceID(), enemy);

                }
                else {
                    enemy = enemies[collision.gameObject.GetInstanceID()];
                }

                enemy.TakeDamage();
            }
        }
        else if (isEnemies == "Boss") {
            float myY = transform.position.y;
            float enemyY = collision.transform.position.y - 1f;

            if (myY + .5f >= enemyY && myY + -.5f <= enemyY) {

                EnemyController enemy = null;

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
}
