using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    new BoxCollider2D collider;

    bool didGenerateGround = false;

    public Obstacle boxTemplate;
    public Obstacle boxTemplate1;
    public Obstacle boxTemplate2;


    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;
    }

    // Update is called once per frame
    void Update()
    {
        groundHeight = transform.position.y + (collider.size.y / 2);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;


        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / -player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = maxJumpHeight * 0.5f;
        maxY += groundHeight;
        float minY = 3;
        float actualY = Random.Range(minY, maxY);

        pos.y = actualY - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = t + player.maxHoldJumpTime;
        float t2 = Mathf.Sqrt((2.0f * (maxY - actualY)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2; 
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + (goCollider.size.y / 2);

        int randomChoose = 0;
        int obstacleNum = Random.Range(0, 4);
        for (int i = 0; i < obstacleNum; i++)
        {
            randomChoose = Random.Range(0, 3);
            if (randomChoose < 1)
            {
                GameObject box = Instantiate(boxTemplate.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float minDistanceFromEdge = 10f; // Adjust this value based on the desired minimum distance
                float x = Random.Range(left + minDistanceFromEdge, right - minDistanceFromEdge);
                Vector2 boxPos = new Vector2(x, y);
                box.transform.position = boxPos;
            }
            else if (randomChoose < 2)
            {
                GameObject box2 = Instantiate(boxTemplate1.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float minDistanceFromEdge = 10f; // Adjust this value based on the desired minimum distance
                float x = Random.Range(left + minDistanceFromEdge, right - minDistanceFromEdge);
                Vector2 boxPos = new Vector2(x, y);
                box2.transform.position = boxPos;
            }
            else
            {
                GameObject box3 = Instantiate(boxTemplate2.gameObject);
                float y = goGround.groundHeight;
                float halfWidth = goCollider.size.x / 2 - 1;
                float left = go.transform.position.x - halfWidth;
                float right = go.transform.position.x + halfWidth;
                float minDistanceFromEdge = 10f; // Adjust this value based on the desired minimum distance
                float x = Random.Range(left + minDistanceFromEdge, right - minDistanceFromEdge);
                Vector2 boxPos = new Vector2(x, y);
                box3.transform.position = boxPos;
            }
        }        
    }
}
