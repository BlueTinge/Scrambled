using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMovement : MonoBehaviour {

    //Initial IO values
    public readonly string[] Vertical = { "Vertical1","Vertical2"};
    public readonly string[] Horizontal = { "Horizontal1","Horizontal2" };

    //Initial physics values
    public readonly float walkAcceleration = .25f;
    public readonly float maxWalkSpeed = 3f;
    public readonly float DashSpeed = 4f;
    public readonly float jumpForce = 350f;
    public readonly float extendedJumpForce = 50f; //holding down arrow key adds increasing force

    //IO
    public int playerNum = 1;
    
    //Physics variables, etc
    public bool touchingGround = false;
    public int framesSinceJump = 1;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public Transform groundCheck;

    //Rendering and anim variables
    bool facingLeft = true;
    Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();

    }


    void FixedUpdate()
    {
        //note: cannot jump if moving and up or down
        touchingGround = !(System.Math.Abs(GetComponent<Rigidbody2D>().velocity.y) > .01);

        float move = Input.GetAxis(Horizontal[playerNum-1]);

        float moveSpeed = GetComponent<Rigidbody2D>().velocity.x + (move * walkAcceleration);
        if (Mathf.Abs(moveSpeed) > Mathf.Abs(maxWalkSpeed))
        {
            moveSpeed = Mathf.Sign(move)* maxWalkSpeed;
        }
        GetComponent<Rigidbody2D>().velocity = new Vector3(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);

        anim.SetFloat("xSpeed", Mathf.Abs(moveSpeed));
        if(moveSpeed > .01) anim.SetBool("Running", true);
        else if (GetComponent<Rigidbody2D>().velocity.x == 0f) anim.SetBool("Running", false);

        //if on the ground and the jump key is pressed, add a jump force
        if (touchingGround && Input.GetAxis(Vertical[playerNum - 1]) > 0)
        {
            framesSinceJump++;

            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));

            touchingGround = false;
        }

        //extended jumps will increase force added to jump
        /**
        if (Input.GetAxis(Vertical[playerNum - 1]) > 0 && framesSinceJump > 0)
        {
            if (framesSinceJump > 0) framesSinceJump++;
            Debug.Log(framesSinceJump);
        }
        else framesSinceJump = 0;
        
        if (!(touchingGround)&& framesSinceJump > 2 && Input.GetAxis(Vertical[playerNum - 1]) > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, extendedJumpForce));
            if (framesSinceJump > 30)
            {
                framesSinceJump = 0;
                touchingGround = false;
            }
        }**/


        //flips if neccesary
        if (move < 0 && !facingLeft)
        {
            Flip();
        }
        else if (move > 0 && facingLeft)
        {
            Flip();
        }
    }

    void Update()
    {


    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("GroundCheck"))
        {
            touchingGround = true;
        }
    }

    //flip if needed
    void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
