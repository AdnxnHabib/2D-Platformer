using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : Enemy   
{
    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    [SerializeField] private float jumpLength = 10f;
    [SerializeField] private float jumpHeight = 15f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;
    
   
    
    private bool facingLeft = true;

    protected override void Start()
    {
        base.Start();        
        coll = GetComponent<Collider2D>();
        
         
    }

    private void Update()
    {
        //transition from jump to fall
        if (anim.GetBool("Jumping"))
        {
            if(rb.velocity.y < 0.1f)
            {
                anim.SetBool("Falling", true);
                anim.SetBool("Jumping", false);
            }
        }
        //transition from fall to idle


        if (coll.IsTouchingLayers(ground) && anim.GetBool("Falling"))
        {
            anim.SetBool("Falling", false);
            
        }

    }

    private void Move()
    {
        if (facingLeft)
        {
            //test to seee if we're beyond the left cap
            if (transform.position.x > leftCap)
            {
                //make sure sprite is focusing right location
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                //test to ssee if the ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);
                }
            }
            else
            {
                facingLeft = false;

            }
            //if it is not, we are going to face right
        }

        else
        {
            if (transform.position.x < rightCap)
            {
                //make sure sprite is focusing right location
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                //test to ssee if the ground, if so jump
                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("Jumping", true);

                }
            }
            else
            {
                facingLeft = true;

            }
            //if it is not, we are going to face right
        }
    }

   

   
}
