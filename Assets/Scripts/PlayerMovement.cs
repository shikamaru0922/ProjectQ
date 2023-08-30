using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collision collision;
    private MagneticPlayer magneticPlayer;

    [Space]
    [Header("Stats")]
    public float speed = 10;
    public float jumpForce = 50;
    public float slideSpeed = 5;
    public float wallJumpLerp = 10;
    //public float dashSpeed = 20;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    public bool wallGrab;
    public bool wallJumped;
    public bool wallSlide;
    public bool isDashing;

    [Space]

    private bool groundTouch;
    private bool hasDashed;

    public int side = 1;

    public int limitSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<Collision>();
        rb = GetComponent<Rigidbody2D>();
        magneticPlayer = GetComponent<MagneticPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        

        Vector2 dir = new Vector2(x, y);
        if (rb.velocity.x > limitSpeed || rb.velocity.x< -limitSpeed)
            return;
        else
        Walk(dir);
        
        if (collision.onWall && Input.GetButton("Fire3") && canMove)
        {
            if (side != collision.wallSide)
                //anim.Flip(side * -1);
            wallGrab = true;
            wallSlide = false;
        }

        if (Input.GetButtonUp("Fire3") || !collision.onWall || !canMove)
        {
            wallGrab = false;
            wallSlide = false;
        }

        if (collision.onGround && !isDashing)
        {
            wallJumped = false;
            //GetComponent<BetterJumping>().enabled = true;
        }

        if (wallGrab && !isDashing)
        {
            rb.gravityScale = 0;
            if (x > .2f || x < -.2f)
                rb.velocity = new Vector2(rb.velocity.x, 0);

            float speedModifier = y > 0 ? .5f : 1;

            rb.velocity = new Vector2(rb.velocity.x, y * (speed * speedModifier));
        }
        else
        {
            rb.gravityScale = 3;
        }

        if (collision.onWall && !collision.onGround)
        {
            if (x != 0 && !wallGrab)
            {
                wallSlide = true;
                WallSlide();
            }
        }

        if (!collision.onWall || collision.onGround)
            wallSlide = false;

        if (Input.GetButtonDown("Jump"))
        {
            //anim.SetTrigger("jump");

            if (collision.onGround)
                Jump(Vector2.up, false);
            if (collision.onWall && !collision.onGround)
                WallJump();
        }

        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            //if (xRaw != 0 || yRaw != 0)
                //Dash(xRaw, yRaw);
        }

        if (collision.onGround && !groundTouch)
        {
            //GroundTouch();
            groundTouch = true;
        }

        if (!collision.onGround && groundTouch)
        {
            groundTouch = false;
        }

        //WallParticle(y);

        if (wallGrab || wallSlide || !canMove)
            return;

        if (x > 0)
        {
            side = 1;
            //anim.Flip(side);
        }
        if (x < 0)
        {
            side = -1;
            //anim.Flip(side);
        }
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

       // side = anim.sr.flipX ? -1 : 1;

        //jumpParticle.Play();
    }

    /*public void Dash(float x, float y)
    {
        //Camera.main.transform.DOComplete();
        //Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        //FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));

        hasDashed = true;

       // anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        Vector2 dir = new Vector2(x, y);

        rb.velocity += dir.normalized * dashSpeed;
        //StartCoroutine(DashWait());
    }*/

    private void Walk(Vector2 dir)
    {
        rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);
    }

    private void Jump(Vector2 dir, bool wall)
    {
        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        //particle.Play();
    }

    private void WallJump()
    {
        if ((side == 1 && collision.onRightWall) || side == -1 && !collision.onRightWall)
        {
            side *= -1;
            //anim.Flip(side);
        }

        //StopCoroutine(DisableMovement(0));
        //StartCoroutine(DisableMovement(.1f));

        Vector2 wallDir = collision.onRightWall ? Vector2.left : Vector2.right;

        Jump((Vector2.up / 1.5f + wallDir / 1.5f), true);

        wallJumped = true;
    }
    private void WallSlide()
    {
        if (collision.wallSide != side)
            //anim.Flip(side * -1);

        if (!canMove)
            return;

        bool pushingWall = false;
        if ((rb.velocity.x > 0 && collision.onRightWall) || (rb.velocity.x < 0 && collision.onLeftWall))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : rb.velocity.x;

        if (magneticPlayer.isAtrracting)
            slideSpeed = 0;
        else
        {
            slideSpeed = 1;
            rb.velocity = new Vector2(push, -slideSpeed);

        }
           



    }

}
