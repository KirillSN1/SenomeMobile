using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public bool KeyboardInput=false; //Управление с клавиатуры
    public float MInput;
    public float Speed;
    public float JumpTime = 1;

    [Range(1, 10)]
    public float JumpingVelocity;

    //  [HideInInspector]
    public int Health = 5;

    public KeyCode JumpButton = KeyCode.Space;
    public Button Up;
    public Button Left;
    public Button Right;

    public Transform Feet;
    public float feetRadius;
    public LayerMask Groundlayer;

    public bool IsAlive = true;
    [HideInInspector]
    public Rigidbody2D rb;
    public bool isGrounded = false;

    public Animator anim;
    private float scale;
    //public Vector2 JVelos;



    //  public enum PlayerStates { Idling, Jumping, Attacking, Walking, Dying };
    // public PlayerStates playerState = PlayerStates.Idling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        scale = transform.localScale.x;
    }


    void Update()
    {
        if (Health <= 0)
        {
            IsAlive = false;
        }
        Motion();
        AnimatinCont();
    }

    public IEnumerator ReceiveDamage(int takenDamage)
    {
        Health -= takenDamage;
        yield return null;
        anim.SetTrigger("Hit");
        //   yield return new WaitForSeconds(3f);       // здесь выставить время, которое занимает анимация игрока, получившего урон
    }

    public void Motion()
    {
        if (KeyboardInput)
        MInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(MInput * Speed, rb.velocity.y);
        if (Input.GetKeyDown(JumpButton) && isGrounded)
        {
            rb.velocity = Vector2.up * JumpingVelocity;
        }

        isGrounded = Physics2D.OverlapCircle(Feet.position, feetRadius, Groundlayer);
    }

    public void AnimatinCont()
    {
        anim.SetFloat("Speed", Mathf.Abs(MInput));
        anim.SetFloat("JumpVelos", rb.velocity.y);
        if (MInput < 0)
        {
            transform.localScale = new Vector3(-scale, transform.localScale.y, transform.localScale.z);
        }
        else if (MInput > 0)
        {
            transform.localScale = new Vector3(scale, transform.localScale.y, transform.localScale.z);
        }
        if (isGrounded)
        {
            anim.SetBool("IsGrounded", true);
        }
        else anim.SetBool("IsGrounded", false);
    }

    
       
}
