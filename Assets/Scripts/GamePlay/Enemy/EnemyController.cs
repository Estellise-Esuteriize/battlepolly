using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy {
    Obstacle, MiniMonster, MediumMonster, PlasticMonster, BossMonster
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour {

    public Enemy enemy;

    public float enemySpeed;

    public float knockBack;

    public int enemyHealth;

    public bool useSetBoxCollider;


    [Range(0, 5)]
    public float stopMovementPosition;

    public AnimationClip deathAnim;

    public Sprite defaultSprite;

    public RuntimeAnimatorController enemyAnim;
    private Animator anim;

    private SpriteRenderer sprite;

    private Rigidbody2D rgbody;
    private BoxCollider2D boxCollider;


    private Vector3 movement;

    private GameplayManager playInstance;

    private bool takeDamage;

    void Awake() {

        rgbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

    }

    void Start() {

        playInstance = GameplayManager.instance;

        sprite.sprite = defaultSprite;
        anim.runtimeAnimatorController = enemyAnim;

        if (useSetBoxCollider) {
            boxCollider.size = sprite.bounds.size;
            boxCollider.offset = new Vector2(0, 0);
        }


        playInstance.SetEnemies(this);
    

    }
    
    

    public void StartEnemy() {

        if (enemy == Enemy.BossMonster)
            return;

        StartCoroutine("Movement");

    }

    void OnWillRenderObject() {

        if (enemy != Enemy.BossMonster)
            return;
        

    }

    public void TakeDamage() {

        if (enemy == Enemy.Obstacle || takeDamage)
            return;

        takeDamage = true;

        StopCoroutine("Movement");

        StartCoroutine("TakingDamage");


        StartCoroutine("Movement");
   

    }

    IEnumerator TakingDamage() {

        enemyHealth--;

        Vector3 position = Vector3.right * knockBack * Time.deltaTime;

        rgbody.MovePosition(rgbody.transform.position + position);


        if (enemyHealth <= 0) {

            anim.SetInteger("Health", enemyHealth);

            StartCoroutine("DamageAction", deathAnim.length);

        }

        yield return new WaitForSeconds(.3f);

        takeDamage = false;

    }


    IEnumerator DamageAction(float miniSec) {

        yield return new WaitForSeconds(miniSec);

        gameObject.SetActive(false);

    }


    IEnumerator Movement() {

        yield return new WaitForSeconds(.5f);

        float xPosition = transform.position.x;


        while (xPosition > stopMovementPosition) {

            Vector3 position = Vector3.left * enemySpeed * Time.deltaTime;

            rgbody.MovePosition(rgbody.transform.position + position);
 
            yield return null;
        }


        gameObject.SetActive(false);

    }


    IEnumerator DetectPlayer() {

        while (enemyHealth > 0) {
           

            yield return null;
        }


    }

    
  
}
