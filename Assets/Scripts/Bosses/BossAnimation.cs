using UnityEngine;

public class BossAnimation : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    Rigidbody2D rb;
    string isMoving = "IsMoving";
    string attackTrigger = "Attack";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.linearVelocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            animator.SetBool(isMoving, true);
        }
        else
        {
            animator.SetBool(isMoving, false);
        }


    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(attackTrigger);
        Debug.Log("BossAnimation: PlayAttackAnimation called");
    }
}
