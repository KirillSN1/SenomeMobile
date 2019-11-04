using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerBehaviour : MonoBehaviour
{
    public bool KeyboardInput=false; //Управление с клавиатуры
    [HideInInspector]
    public float MInput;
    public float Speed;
    

    [Range(1, 10)]
    public float JumpingVelocity;
    public bool DoubleJump=false;
    [Range(1,1.3f)]
    public float AccelerationValue=1.066f;
    [HideInInspector]
    public int JumpsNum;
    
    //  [HideInInspector]
    public int Health = 5;

    public KeyCode JumpButton = KeyCode.Space;
    //public Button Up;
    //public Button Left;
    //public Button Right;

    public Transform Feet;
    public float feetRadius;
    public LayerMask Groundlayer;

    public bool IsAlive = true;
    [HideInInspector]
    public Rigidbody2D rb;
    public bool isGrounded = false;

    public Animator anim;
    private float scale;

    private Collider2D Capsule;
    private Collider2D Box;



    //  public enum PlayerStates { Idling, Jumping, Attacking, Walking, Dying };
    // public PlayerStates playerState = PlayerStates.Idling;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        scale = transform.localScale.x;
        Capsule = GetComponent<CapsuleCollider2D>();
        Box = GetComponent<BoxCollider2D>();
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
        if (!DoubleJump)
        {

            if (Input.GetKeyDown(JumpButton) && isGrounded)
            {
                rb.velocity = Vector2.up * JumpingVelocity;
            }
            if (rb.velocity.y < 0) //Ускорение падения
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * AccelerationValue);
            }
        }
        else
        {
            if (Input.GetKeyDown(JumpButton) && JumpsNum < 1)
            {
                ++JumpsNum;
                rb.velocity = (Vector2.up * JumpingVelocity) + new Vector2(rb.velocity.x,0);
            }
            else
            if (isGrounded && JumpsNum >0)
            {
                JumpsNum = 0;
            }
        }

        isGrounded = Physics2D.OverlapCircle(Feet.position, feetRadius, Groundlayer);
    }

    public void AnimatinCont()
    {
        anim.SetFloat("Speed", Mathf.Abs(MInput));
        anim.SetFloat("JumpVeloc", rb.velocity.y);
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

    public void InputModeChange()
    {
        if (KeyboardInput)
        {
            KeyboardInput = false;
        }
        else KeyboardInput = true;
    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Feet.transform.position, feetRadius);
    }



}
