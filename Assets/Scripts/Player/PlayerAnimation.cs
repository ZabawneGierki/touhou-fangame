using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    string isMoving = "isMoving";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        anim = GetComponent<Animator>();
        
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
       if (rb.linearVelocity.x  > 0)
        {
            anim.SetBool(isMoving, true);
            sr.flipX = false;
        }
        else if (rb.linearVelocity.x < 0)
        {
            anim.SetBool(isMoving, true);
            sr.flipX = true;
        }
        else
        {
            anim.SetBool(isMoving, false);
            

        }
    }
}
