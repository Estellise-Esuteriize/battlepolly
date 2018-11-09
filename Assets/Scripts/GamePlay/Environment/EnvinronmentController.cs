using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvinronmentController : MonoBehaviour {

   
    public GameObject groundHolder;

    public Sprite[] grounds;

    public BoxCollider2D movementStopper;

    public int enemyCount;

    [HideInInspector]
    public float characterStartingPositionX;


    private EnemySpawnPosition enemySpawn;

    private IEnvironmentData envintData;

    private PoolerController poolerInstance;
    private DataController dataInstance;

    public void InitializeGround(GameObject obs) {

        poolerInstance = GameController.instance.pooler;
        dataInstance = DataController.instance;

        envintData = (IEnvironmentData)obs.GetComponent<IEnvironmentData>();

        InitGrounds();

    }

    void InitGrounds() {

        for (int i = 0; i < grounds.Length; i++) {

            Vector3 newPosition = Vector3.zero;

            float groundWidth = grounds[i].bounds.size.x;

            float distance = groundWidth * i;

            newPosition.x = distance;


            GameObject obs = poolerInstance.PooledObject(PoolingKey.Ground);
            obs.transform.SetParent(transform);
            obs.SetActive(true);
            obs.transform.position = newPosition;


            SpriteRenderer sprite = obs.GetComponent<SpriteRenderer>();
            sprite.sprite = grounds[i];

            if (i == 0) {
                float width = grounds[i].bounds.size.x;
                float oneThird = (width / 2) / 2;

                Vector3 position = Vector3.zero;
                position.x += -oneThird;

                envintData.CharacterStartPosition(position);


                continue;
                

            }
            else if (i + 1 == grounds.Length) {

                Vector3 movementStopperSize = Vector3.one;
                movementStopperSize.y = grounds[i].bounds.size.y;

                movementStopper.size = movementStopperSize;

                Vector3 position = Vector3.zero;

                float spriteWidth = grounds[i].bounds.size.x;

                position.x = distance - (spriteWidth / 2);

                movementStopper.transform.position = position;

                continue;

            }



            BoxCollider2D[] boxes = obs.GetComponentsInChildren<BoxCollider2D>();


            float minYDirection = Mathf.Sign(boxes[0].transform.position.y);
            float maxYDirection = Mathf.Sign(boxes[1].transform.position.y);


            float minYBounds = boxes[0].bounds.size.y / 2;
            float maxYBounds = boxes[1].bounds.size.y / 2;

            float groundW = grounds[i].bounds.size.x / 2;

            float minGroundWidth = distance - groundW;
            float maxGroundWidth = distance + groundW;

            enemySpawn.minX = (Mathf.Abs(boxes[0].transform.position.y) - minYBounds) * minYDirection;
            enemySpawn.maxY = (Mathf.Abs(boxes[1].transform.position.y) - maxYBounds) * maxYDirection;

            enemySpawn.minX = minGroundWidth;
            enemySpawn.maxX = maxGroundWidth;

            enemySpawn.boss = (i == grounds.Length - 2) ? distance + groundW - 2 : 0;

            SpawnEnemy(enemySpawn);

        }

    }


    void SpawnEnemy(EnemySpawnPosition spawn) {

        Vector3 enemyPosition = Vector3.zero;

        int enemies = enemyCount + dataInstance.dataFile.currentLevel;
        enemies = (int)Mathf.Log(enemies, 2f);
      
        GameObject obs = null;

        for (int i = 0; i < enemies; i++) {

            float xPos = Random.Range(spawn.minX, spawn.maxX);
            float yPos = Random.Range(spawn.minY, spawn.maxY);

            enemyPosition.x = xPos;
            enemyPosition.y = yPos;

            int monsterRange = Random.Range(0, (System.Enum.GetNames(typeof(Enemy)).Length) - 1);
            obs = poolerInstance.PooledObject(PoolingKey.Enemies, (Enemy)monsterRange);
            obs.transform.position = enemyPosition;
            obs.transform.parent = transform;

            SpriteRenderer render = obs.GetComponent<SpriteRenderer>();

            render.sortingOrder = Mathf.RoundToInt((yPos + .5f) * -10);

            obs.SetActive(true);

        }


        if (spawn.boss > 0) {

            enemyPosition.x = spawn.boss;
            enemyPosition.y = 0f;

            obs = poolerInstance.GetBoss(1);
            obs.transform.position = enemyPosition;
            obs.transform.parent = transform;
            obs.SetActive(true);
        }

    }


}

public struct EnemySpawnPosition {

    public float minX, maxX;
    public float minY, maxY;
    public float boss;

}

public interface IEnvironmentData {

    void CharacterStartPosition(Vector3 spawn);
    

}

