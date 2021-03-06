﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    //Start variables
    private Rigidbody2D rb;
   private Animator anim;

    //states
    private enum State {idle, running, jumping, falling, hurt}
    private State state = State.idle;
    private Collider2D coll;
    

  

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float hurtForce = 10f;
    

    [SerializeField] private AudioSource cherry;
    [SerializeField] private AudioSource footstep;
   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();


    }
    private void Update()
    {
        if(state != State.hurt)
        {
            Movement();
        }
        

        AnimationState();

        anim.SetInteger("state", (int)state); //sets animation based on Enumerator state
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            PermanentUI.perm.cherries += 1;
            PermanentUI.perm.cherryText.text = PermanentUI.perm.cherries.ToString();
        }

        if(collision.tag == "Powerup")
        {
            Destroy(collision.gameObject);
            jumpForce = 25f;
            GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(ResetPower());
        }
    }




    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(state == State.falling)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HandleHealth(); //deals with healh and will reset level if health is 0
                if (other.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to my right therfore i should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);

                }
                else
                {
                    //Enemy is to my left therefore I should be damage and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);
                }
            }

        }
    }

    private void HandleHealth()
    {
        PermanentUI.perm.health -= 1;
        PermanentUI.perm.healthAmount.text = PermanentUI.perm.health.ToString();
        if (PermanentUI.perm.health <= 0)
        if (PermanentUI.perm.health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Movement()
    {
        //Movement Code 
        float hDirection = Input.GetAxis("Horizontal");
        //moving left
        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);

        } // moving right
        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);

        }

        //jumping
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }


    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }
    

    private void AnimationState()
    {

        if (state == State.jumping)
        {
            if (rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }

        else if (Mathf.Abs(rb.velocity.x) > 2f)
        {
            state = State.running;

        }
        else
        {
            state = State.idle;
        }

    }

    private void FootstepSound()
    {
        footstep.Play();
    }

    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(10);
        jumpForce = 10f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
