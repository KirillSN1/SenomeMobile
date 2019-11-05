using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerBehaviour : MonoBehaviour
{
    public bool KeyboardInput=false; //Управление с клавиатуры
    [HideInInspector]
    public float MInput;
    public float Speed;
    public float JumpTime = 1;
    public int Attack = 1;

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
    public KeyCode AttackButton = KeyCode.E;
     
    public Transform Feet;
    public float feetRadius;
    public LayerMask Groundlayer;

    public bool IsAlive = true;
    [HideInInspector]
    public Rigidbody2D rb;
    public bool isGrounded = false;

    public Animator anim;
    private float scale;
    public Vector2 JVelos;

    private Vector2 currentPosition;
    private Vector2 endPosition;
    private GameObject enemy;

    public int SightDistance = 1;
    private KnockBack _knockBack;

    //  public enum PlayerStates { Idling, Jumping, Attacking, Walking, Dying };
    // public PlayerStates playerState = PlayerStates.Idling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        scale = transform.localScale.x;

        _knockBack = GetComponent<KnockBack>();
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

        if (Input.GetKeyDown(AttackButton) && isGrounded)      // атаковать enemy
        {
            DetectEnemy();
        }

        isGrounded = Physics2D.OverlapCircle(Feet.position, feetRadius, Groundlayer);
        AnimatinCont();
    }


    public void DetectEnemy()
    {
        currentPosition = new Vector2(transform.position.x, transform.position.y);
        endPosition = new Vector2(transform.position.x + SightDistance, transform.position.y + SightDistance);

        var hits = Physics2D.LinecastAll(currentPosition, endPosition);

        foreach (var obj in hits)
        {
            var targetObj = obj.collider.gameObject;
            if (targetObj.CompareTag("Enemy"))
            {
                AttackTheEnemy(targetObj);
                _knockBack.HitSomeObject(targetObj);
            }
        }
    }

    private void AttackTheEnemy(GameObject enemy)
    {
        if(enemy!= null)
        {
           StartCoroutine(enemy.GetComponent<EnemyBasicAI>().ReceiveDamage(Attack));
        }
        
    }

    //public void ReceiveDamage(int takenDamage)
    //{
    //    Health -= takenDamage;
    //    Debug.Log("Player got hit!");
    //  //  yield return null;
    // //   yield return new WaitForSeconds(3f);       // здесь выставить время, которое занимает анимация игрока, получившего урон
    //}

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

    void OnDrawGizmosSelected()      // рисует радиус атаки игрока
    {
       Gizmos.color = Color.blue;
       Gizmos.DrawWireSphere(transform.position, SightDistance);
    }
}
