using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Animator anim;
    private float dirX = 0f;
    [SerializeField]private LayerMask jumpableGround;
    private SpriteRenderer sprite;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 14f;

    private enum MovementState{idle, running, jumping, falling}
    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        MovementState State;
        if(dirX > 0f)
        {
            State = MovementState.running;
            sprite.flipX = false;  
        }
        else if(dirX < 0f)
        {
            State = MovementState.running;
            sprite.flipX = true;    
        }
        else
        {
            State = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            State = MovementState.jumping;
        }
        else if(rb.velocity.y < -.1f)
        {
            State = MovementState.falling;
        }

        anim.SetInteger("State", (int)State);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
