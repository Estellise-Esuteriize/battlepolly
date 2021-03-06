﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public Sprite defaultCharaSprite;

    private Dictionary<float, EnemyController> enemies = new Dictionary<float, EnemyController>();

    private void OnTriggerEnter2D(Collider2D collision) {

        string isEnemies = collision.tag;

        float addWideRange = .9f;

        if (isEnemies == "Enemies") {
           
            float myY = transform.position.y + ((defaultCharaSprite.bounds.max.y / 3) * -1);

            float enemyY = collision.transform.position.y;
            
            if (myY + addWideRange >= enemyY && myY + -addWideRange <= enemyY) {

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
            float myY = transform.position.y + ((defaultCharaSprite.bounds.max.y / 3) * -1);
            float enemyY = collision.transform.position.y - 1.55f;


            if (myY + addWideRange >= enemyY && myY + -addWideRange <= enemyY) {

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
