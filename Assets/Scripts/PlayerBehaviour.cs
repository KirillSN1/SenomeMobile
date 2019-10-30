using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
 
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
    private bool isGrounded = false;

  //  public enum PlayerStates { Idling, Jumping, Attacking, Walking, Dying };
   // public PlayerStates playerState = PlayerStates.Idling;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
    }


    void Update()
    {
        if(Health <= 0)
        {
            IsAlive = false;
        }
   
        float MInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(MInput * Speed, rb.velocity.y);
 

        if (Input.GetKeyDown(JumpButton) && isGrounded)
        {
            rb.velocity = Vector2.up * JumpingVelocity;
        }

        isGrounded = Physics2D.OverlapCircle(Feet.position, feetRadius, Groundlayer);
    }   

    public IEnumerator ReceiveDamage(int takenDamage)
    {
        Health -= takenDamage;
        yield return null;
     //   yield return new WaitForSeconds(3f);       // здесь выставить время, которое занимает анимация игрока, получившего урон
    }
       
}
