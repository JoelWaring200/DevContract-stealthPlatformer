using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    //Variables!
    private Animator anim;
    private Rigidbody2D rb2d;

    public float moveSpeed = 5f;
    private int direction = 1;


    public float groundCheckDistance = 1.5f;
    public LayerMask groundLayer;




    public Transform point1;
    public Transform point2;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

    }

    //FixedUpdate is like Update, but called at a fixed interval (59.8 something)
    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        //Remember direction -1 = left, +1 = right!
        rb2d.linearVelocity = new Vector2(moveSpeed * direction, rb2d.linearVelocity.y);
        anim.SetBool("isWalking", true);    //Play the walking animation

        if (transform.position.x <= point1.position.x)
        {
            direction = 1;
        }

        if (transform.position.x >= point2.position.x)
        {
            direction = -1;
        }

    }

    void StopMoving()
    {
        //Set the rb2d velocity to x = 0, y = whatever it currently is
        rb2d.linearVelocity = new Vector2(0, rb2d.linearVelocity.y);
        anim.SetBool("isWalking", false);   //Flip the animator parameter
    }

    void OnDrawGizmos()
    {
        if (point1 != null && point2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(point1.position, point2.position);
        }
    }

}
