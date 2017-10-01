using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggMovement : MonoBehaviour {

    //Initial IO values
    public readonly string[] Vertical = { "Vertical1","Vertical2"};
    public readonly string[] Horizontal = { "Horizontal1","Horizontal2" };

    //Initial physics values
    public readonly float maxWalkSpeed = 2f;
    public readonly float maxDashSpeed = 3f;
    public readonly float jumpForce = 200f;

    //IO
    public int playerNum = 1;
    
    //Physics variables, etc
    public bool touchingGround = false;
    public float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public Transform groundCheck;

    //Rendering and anim variables
    bool facingLeft = true;
    Animator anim;

    // Use this for initialization
    void Start()
    {


        //anim = GetComponent<Animator>();

    }


    void FixedUpdate()
    {
        //note: cannot jump if moving and up or down
        touchingGround = !(System.Math.Abs(GetComponent<Rigidbody2D>().velocity.y) > .01);

        float move = Input.GetAxis(Horizontal[playerNum-1]);

        GetComponent<Rigidbody2D>().velocity = new Vector3(move * maxWalkSpeed, GetComponent<Rigidbody2D>().velocity.y);

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
        //if on the ground and the jump key is pressed, add a jump force
        if (touchingGround && Input.GetAxis(Vertical[playerNum-1]) > 0)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        }


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
