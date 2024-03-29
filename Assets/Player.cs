using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Vector2 velocity;

    public float fall = 0;
    public float gravity;
    public float maxXVelocity = 100;
    public float maxAcceleration = 10;
    public float acceleration = 10;
    public float distance = 0;
    public float maxJumpDistance = 1.5f; 
    public float jumpVelocity = 20;
    public float groundHeight = 10;
    public bool isGrounded = false;
    public float boost = 0;
    public bool isHoldingJump = false;
    public float maxHoldJumpTime = 0.4f;
    public float maxMaxHoldJumpTime = 0.4f;
    public float holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 1;

    public bool isDead = false;

    public LayerMask groundLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask obstacle1LayerMask;
    public LayerMask obstacle2LayerMask;

    void Start()
    {
        velocity.x = 20.0f;
    }

    void Update()
    {
        animator.SetFloat("speed", velocity.x);
        animator.SetBool("grounded", isGrounded);

        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);
        
        if (isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if (Input.GetKey(KeyCode.Space) && groundDistance <= maxJumpDistance && isGrounded)
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (isDead)
        {
            return;
        }
        if (pos.y < -40)
        {
            isDead = true;
            velocity.x = 0;
        }
        if (fall > pos.y)
        {
            animator.SetBool("fallingdownwards", true);
        }
        else
        {
            animator.SetBool("fallingdownwards", false);
        }
        fall = pos.y;

        if ((!isGrounded) || (boost > 0))
        {
            // Jump physics
            if (boost > 0)
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;
                boost -= 1;
            }

            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                {
                    isHoldingJump = false;
                }
            }

            pos.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldingJump)
            {
                velocity.y += gravity * Time.fixedDeltaTime;
            }

            Vector2 rayOrigin = new Vector2(pos.x + 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if (hit2D.collider != null)
            {
                Ground ground = hit2D.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y >= ground.groundHeight)
                    {
                        groundHeight = ground.groundHeight;
                        pos.y = groundHeight;
                        velocity.y = 0;
                        isGrounded = true;
                    }
                }
            }

            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

            Vector2 wallOrigin = new Vector2(pos.x, pos.y);
            RaycastHit2D wallHit = Physics2D.Raycast(wallOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, groundLayerMask);
            if (wallHit.collider != null)
            {
                Ground ground = wallHit.collider.GetComponent<Ground>();
                if (ground != null)
                {
                    if (pos.y < ground.groundHeight)
                    {
                        velocity.x = 0;

                    }
                }
            }
        }

        distance += velocity.x * Time.fixedDeltaTime;

        // Running physics
        if (isGrounded)
        {
            float velocityRatio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRatio);
            maxHoldJumpTime = maxMaxHoldJumpTime * velocityRatio;

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            Vector2 rayOrigin = new Vector2(pos.x - 0.7f, pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, groundLayerMask);
            if (hit2D.collider == null)
            {
                isGrounded = false;
            }
            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);
        }


        // Obstacle physics
        Vector2 obstOrigin = new Vector2(pos.x, pos.y);
        RaycastHit2D obstHitX = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitX.collider != null)
        {
            Obstacle obstacle = obstHitX.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHitY = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacleLayerMask);
        if (obstHitY.collider != null)
        {
            Obstacle obstacle = obstHitY.collider.GetComponent<Obstacle>();
            if (obstacle != null)
            {
                hitObstacle(obstacle);
            }
        }

        RaycastHit2D obstHitX1 = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacle1LayerMask);
        if (obstHitX1.collider != null)
        {
            Obstacle obstacle1 = obstHitX1.collider.GetComponent<Obstacle>();
            if (obstacle1 != null)
            {
                hitObstacle1(obstacle1);
            }
        }

        RaycastHit2D obstHitY1 = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacle1LayerMask);
        if (obstHitY1.collider != null)
        {
            Obstacle obstacle1 = obstHitY1.collider.GetComponent<Obstacle>();
            if (obstacle1 != null)
            {
                hitObstacle1(obstacle1);
            }
        }

        RaycastHit2D obstHitX2 = Physics2D.Raycast(obstOrigin, Vector2.right, velocity.x * Time.fixedDeltaTime, obstacle2LayerMask);
        if (obstHitX2.collider != null)
        {
            Obstacle obstacle2 = obstHitX2.collider.GetComponent<Obstacle>();
            if (obstacle2 != null)
            {
                hitObstacle2(obstacle2);
            }
        }

        RaycastHit2D obstHitY2 = Physics2D.Raycast(obstOrigin, Vector2.up, velocity.y * Time.fixedDeltaTime, obstacle2LayerMask);
        if (obstHitY2.collider != null)
        {
            Obstacle obstacle2 = obstHitY2.collider.GetComponent<Obstacle>();
            if (obstacle2 != null)
            {
                hitObstacle2(obstacle2);
            }
        }

        // Move player
        transform.position = pos;
    }

    // Effects of obstacle on player
    void hitObstacle(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        velocity.x *= 0.7f;
    }

    void hitObstacle1(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        velocity.x *= 1.3f;
    }

    void hitObstacle2(Obstacle obstacle)
    {
        Destroy(obstacle.gameObject);
        boost += 50;
    }
}
