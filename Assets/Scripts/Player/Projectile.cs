using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        /*
        rb.linearVelocity = new Vector2(0, 10f);
        if (transform.position.y > 5f)
        {
            Destroy(gameObject);
        }
        */
    }
        
}
