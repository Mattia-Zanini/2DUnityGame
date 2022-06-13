using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private Vector2 jump;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private enum MovementState {idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Player's movements attached");
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        jump = new Vector2(rb.velocity.x, jumpForce);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Movements();
        UpdateAnimation();
    }
    private void Movements()
    {
        dirX = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.velocity = jump;
            jumpSoundEffect.Play();
        }
    }
    private void UpdateAnimation()
    {
        MovementState state;
        //idle
        if (dirX != 0f) {
            state = MovementState.running;

            if (dirX < 0f) //player to the left
                spriteRenderer.flipX = true;
            else if (dirX > 0f) //player to the right
                spriteRenderer.flipX = false;
        }
        else //running
            state = MovementState.idle;

        if (rb.velocity.y > .1f) //player is jumping
            state = MovementState.jumping;
        else if (rb.velocity.y < -.1f) //player is falling
            state = MovementState.falling;

        animator.SetInteger("state", (int)state);
    }
    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
