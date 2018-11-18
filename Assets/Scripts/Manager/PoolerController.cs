using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolingKey {
    
    Ground, Enemies, Boss

}

public class PoolerController : MonoBehaviour {

    public Pooling[] pooler;

    public bool willGrow;

    private List<Pool> pool;


    GameController instance; 

    void Awake() {

        instance = GameController.instance;

        if (instance == null) {

            pool = new List<Pool>();


            for (int i = 0; i < pooler.Length; i++) {

                for (int z = 0; z < pooler[i].enemyLimit; z++) {

                    pool.Add(InstantiateGameObject(pooler[i]));

                }

            }
        }

    }


    public GameObject PooledObject(PoolingKey key, Enemy enemy = Enemy.Obstacle) {

        if (key == PoolingKey.Ground) {

            for (int i = 0; i < pool.Count; i++) {
                if (pool[i].key == key && !pool[i].obs.activeInHierarchy) {
                    return pool[i].obs;
                }
            }

        }
        else if (key == PoolingKey.Enemies) {

            if (enemy == Enemy.Obstacle) {

                List<GameObject> obstacleMonster = new List<GameObject>();

                for(int i = 0; i < pool.Count; i++) {

                    if (pool[i].key == key && pool[i].enemy == enemy && !pool[i].obs.activeInHierarchy)
                        obstacleMonster.Add(pool[i].obs);
                    

                }

                if (obstacleMonster.Count > 0) {

                    int randomMonster = UnityEngine.Random.Range(0, obstacleMonster.Count - 1);

                    return obstacleMonster[randomMonster];
                }

            }
            else {

                for (int i = 0; i < pool.Count; i++) {

                    if (pool[i].key == key && pool[i].enemy == enemy && !pool[i].obs.activeInHierarchy) {
                        return pool[i].obs;
                    }

                }
            }

        }

        if (willGrow) {

            List<Pool> obsect = new List<Pool>();

            for (int i = 0; i < pooler.Length; i++) {

                if (pooler[i].enemyKey == key && pooler[i].enemyType == enemy) {

                    Pool obs = InstantiateGameObject(pooler[i]);

                    pool.Add(obs);

                    obsect.Add(obs);

                    //return pool[pool.Count - 1].obs;

                }

            }

            int rand = 0;

            if (obsect.Count > 1) 
                rand = UnityEngine.Random.Range(0, obsect.Count);

            return obsect[rand].obs;

        }
   
        return null;
    }


    public GameObject GetBoss(int level) {

        List<GameObject> boss = new List<GameObject>();

        for (int i = 0; i < pool.Count; i++) {
            if (pool[i].key == PoolingKey.Boss)
                boss.Add(pool[i].obs);
            
        }

        return boss[level - 1];

    }



    public void ResetPooledObjects() {

        for (int i = 0; i < pool.Count; i++) {

            GameObject obs = pool[i].obs;
            
            obs.transform.parent = null;

            DontDestroyOnLoad(obs);

            obs.SetActive(false);

               
        }
    }


    Pool InstantiateGameObject(Pooling pooler) {

        Pool pol;

        GameObject polObs = pooler.enemy;
        PoolingKey key = pooler.enemyKey;
        Enemy enem = pooler.enemyType;

        GameObject obs = (GameObject)Instantiate(polObs, transform.position, Quaternion.identity);
        DontDestroyOnLoad(obs);
        obs.tag = key.ToString();

        obs.SetActive(false);

        pol.obs = obs;
        pol.key = key;
        pol.enemy = enem;

       
        return pol;

    }


}

struct Pool {

    public GameObject obs;

    public PoolingKey key;

    public Enemy enemy;

}

[System.Serializable]
public struct Pooling {

    public GameObject enemy;

    public int enemyLimit;

    public PoolingKey enemyKey;

    public Enemy enemyType;


}

