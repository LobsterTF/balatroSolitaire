using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winBouncyBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        float speed = Random.Range(70,80);
        Vector2 randDir = new Vector2(Random.Range(0, 5f), Random.Range(0, 5f));
        rb.AddForce(randDir * speed * 100);
    }
    void Update()
    {
        if (rb.position.y < 0)
        {

            Vector2 randDir = new Vector2(Random.Range(-1, 2f), Random.Range(2, 5f));
            rb.AddForce(randDir * (rb.velocity.magnitude / 5) * Time.deltaTime * 1000);
        }
    }
   
}
