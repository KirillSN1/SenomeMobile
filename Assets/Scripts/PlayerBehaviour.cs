using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float MInput; 
    public float Speed;
    public float JumpTime = 1;

    [Range(1, 10)]
    public float JumpingVelocity;

  //  [HideInInspector]
    public int Health = 5;

    public KeyCode JumpButton = KeyCode.Space;
     
    public Transform Feet;
    public float feetRadius;
    public LayerMask Groundlayer;

    public bool IsAlive = true;
    private Rigidbody2D rb;
    public bool isGrounded = false;

    public Animator anim;
    private float scale;
    public Vector2 JVelos;
    
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        scale = transform.localScale.x;
    }


    void Update()
    {
        if(Health <= 0)
        {
            IsAlive = false;
        }
   
        MInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(MInput * Speed, rb.velocity.y);
        JVelos = rb.velocity;    
        
        if (Input.GetKeyDown(JumpButton) && isGrounded)
        {
            rb.velocity = Vector2.up * JumpingVelocity;
        }

        isGrounded = Physics2D.OverlapCircle(Feet.position, feetRadius, Groundlayer);
        AnimatinCont();
    }   

    public void ReceiveDamage(int takenDamage)
    {
        Health -= takenDamage;
    }

    public void AnimatinCont()
    {
        anim.SetFloat("Speed", Mathf.Abs(MInput));
        anim.SetFloat("JumpVelos",rb.velocity.y);
        if (MInput < 0)
        {
            transform.localScale = new Vector3(-scale, transform.localScale.y, transform.localScale.z);
        }
        else if (MInput>0)
        {
            transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
        }
        if (isGrounded == false)
        {
            anim.SetBool("isJumped",true);
        }
        else anim.SetBool("isJumped", false);
        if (rb.velocity.y<0)
        {
            anim.SetBool("falling", true);
            anim.SetBool("isJumped", false);
        }
        else if (rb.velocity.y>0)
        {
            anim.SetBool("falling", false);
            anim.SetBool("isJumped", true);
        }
        if (isGrounded)
        {
            anim.SetBool("IsGrounded", true);
        }
        else anim.SetBool("IsGrounded", false);
    }
       
}
