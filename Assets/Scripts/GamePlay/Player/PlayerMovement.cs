using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float speed;

    public LayerMask wallMask;

    public int verticalRayCount = 4;


    private RaycastOrigins raycastOrigins;

    private float verticalRaySpacing;

    private bool isGameComplete = false;
    private bool isGameOver = false;
    private bool isGamePause = false;

    private Rigidbody2D rgbody;
    private BoxCollider2D boxCollider;
    private Animator anim;

    private Vector3 velocity;



    private const float skinWidth = 0.015f;

    void Start() {

        rgbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }


    IEnumerator MovingHorizontalAndVertical() {
        
        while (!isGameComplete && !isGameOver && !isGamePause) {
            
            UpdateRaycastOrigins();

            velocity = Vector3.zero;

            float moveX = 1f;
            float moveY = Input.GetAxisRaw("Vertical");
           
            if (moveX > 0)
                anim.SetFloat("Movement", moveX);
            else if (moveY > 0)
                anim.SetFloat("Movement", moveY);
            else
                anim.SetFloat("Movement", moveX);

            moveX = moveX * speed * Time.deltaTime;
            moveY = moveY * speed * Time.deltaTime;

            velocity.x = moveX;
            velocity.y = moveY;

            CheckWall(ref velocity);

            rgbody.MovePosition(rgbody.transform.position + velocity);



            yield return null;

        }


    }

    void CheckWall(ref Vector3 velocity) {

        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {

            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, wallMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit) {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
    }

    void CalculateRaySpacing() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);

        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public void UpdateRaycastOrigins() {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2f);


        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }



    public struct RaycastOrigins {


        public Vector2 bottomLeft, bottomRight;
        public Vector2 topLeft, topRight;
    }

}
