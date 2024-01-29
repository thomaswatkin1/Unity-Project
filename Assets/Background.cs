using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
     
    public float speed = 10f;

    Vector3 startPos;
    float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider2D> ().size.x/2;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameObject.Find("Player").GetComponent<Player>().isDead)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);

            if (transform.position.x < startPos.x - repeatWidth)
            {
                transform.position = startPos;
            }
        }
        
    }
}
