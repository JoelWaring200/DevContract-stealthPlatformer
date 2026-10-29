using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Global variables can be accessed by the entire script!
    //public variables can be accessed by other scripts and
    //are visible in the editor!
    public float moveSpeed;

    //Jump stuff
    public float jumpHeight;
    public float fallMultiplier = 2.5f; 
    public float lowJumpMultiplier = 1.5f; 
    public bool isGrounded;
    public LayerMask groundLayer;  

    //Length of the ground check.
    public float groundCheckLength = 1.25f;

    //Coyote time/ledge forgiveness
    public float coyoteTime;
    public float coyoteTimeMax = 0.2f;

    //Variable of type Rigidbody2D (a component!)
    private Rigidbody2D rb2d;

    //Variables for the Animator and SpriteRenderer
    private Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();   
        sr = GetComponent<SpriteRenderer>(); 

        //Reset the coyoteTime
        coyoteTime = coyoteTimeMax;
    }

    // Update is called once per frame
    void Update()
    {
     
        float h = Input.GetAxis("Horizontal");

        //Call the custom function and send it the value of h
        MoveCharacter(h);

        //Initialize isGrounded with whatever the Jump() function
        isGrounded = Jump();

        //If the player is on the ground, reset coyoteTime. If they are not, count down
        if (isGrounded) {
            coyoteTime = coyoteTimeMax;
        }
        else {
            coyoteTime -= Time.deltaTime;
        }  //deltaTime is the time between frames.

        //If coyoteTime is active and the player presses the spacebar, they can jump
        if (coyoteTime > 0 && Input.GetKeyDown(KeyCode.Space))
        {
            //Jump using the Rigidbody velocity
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, jumpHeight);
        }

        //Gravity Modifications for Fancy Jumps (tm)
        if (rb2d.linearVelocity.y < 0)    //If the player is falling
        {
            //Add to the velocity.
            //Vector2.up (0, 1) * gravity (default is -9.8) * fallMultiplier scalar
            //and then scaled by deltaTime to fix potential frame jitter
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y *
                (fallMultiplier * Time.deltaTime));
        }
        //If the player is ascending && the Spacebar is NOT being held
        //GetKey checks for a press every frame (holding it down)
        else if (rb2d.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            //If they are in the air and not pressing the key, apply the
            //low jump multiplier.
            rb2d.linearVelocity += (Vector2.up * Physics2D.gravity.y *
                (lowJumpMultiplier * Time.deltaTime));
        }

        //SetFloat will assign a value to the animator variables
                            //Mathf.Abs means 'absolute value' 
                            //it ignores the + or - sign.
        anim.SetFloat("walkSpeed", Mathf.Abs(h));

        //If the player is in the air, switch the state of isJumping
        //opposite of isGrounded
        anim.SetBool("isJumping", !isGrounded);
    }

    //This needs a way to 'store' the value of h, so there is a
    //float variable in the brackets!
    void MoveCharacter(float hSpeed)
    {
        //Use the rb2d velocity parameter to move horizontally!
        /*Set x to the Horizontal axis * moveSpeed, and set the y
         * to whatever the rb2d y velocity currently is*/
        rb2d.linearVelocity = new Vector2(hSpeed * moveSpeed,
                            rb2d.linearVelocity.y);

        //Flip the player left and right
        if (hSpeed < 0)
        {
            sr.flipX = true;
        } else if (hSpeed > 0)
        {
            sr.flipX = false;
        }
    }

    bool Jump()
    {

        bool check = Physics2D.Raycast(transform.position, Vector2.down, groundCheckLength,
                    groundLayer);

        //Return the value of check (yes there was a collision, no there wasn't)
        return check;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hitting things");
        if (collision.gameObject.tag == "bad")
        {
            transform.position = new Vector3(0, 1, 0);
        }
    }
}
