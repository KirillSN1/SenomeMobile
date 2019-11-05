using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float TimeBetweenAttack = .2f;  
    public float TimeTillAttack;

    private Animator _anim;
  //  private bool _isAttacking = false;

    private EnemyBasicAI _enemy;


    void Start()
    {
        _enemy = transform.parent.GetComponent<EnemyBasicAI>();
        _anim = _enemy.Anim;

        if(_anim == null)
        {
            Debug.Log("Enemy doesn't have an Animator!");
        }
    }


    void Update()
    {
  
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (TimeTillAttack <= 0)
            {
                var target = other.GetComponent<Rigidbody2D>();
               
                StartCoroutine(AttackThePlayer(target));

                Debug.Log("Triggered");
                TimeTillAttack = TimeBetweenAttack;
            }
            TimeTillAttack -= Time.deltaTime;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TimeTillAttack = TimeBetweenAttack;
        _enemy.EnemyState = EnemyBasicAI.EnemyStates.Running;
    }



    //private void HitSomeObject(Collider2D other)
    //{
    //    var target = other.GetComponent<Rigidbody2D>();

    //    if (target != null)
    //    {
    //      //  Debug.Log("tag is " + target.tag);
    //        if (target.gameObject.CompareTag("Player"))
    //        {

    //            AttackThePlayer(target);
    //         //   Debug.Log("Gonna attack player");
    //        }
    //        else if (target.gameObject.CompareTag("Enemy"))
    //        {
    //            AttackTheEnemy(target);       
    //        }
    //    }
    //}

    //private void AttackTheEnemy(Rigidbody2D enemy)
    //{
    //    var amount = transform.parent.GetComponent<PlayerBehaviour>().Attack;
    //    enemy.GetComponent<EnemyBasicAI>().ReceiveDamage(amount);

    //}

    private IEnumerator AttackThePlayer(Rigidbody2D player)
    {
        _enemy.EnemyState = EnemyBasicAI.EnemyStates.Attacking;
        _anim.SetBool("isAttacking", true);
        _anim.SetBool("isRunningEnemy", false);

        yield return null;

        yield return new WaitForSeconds(.6f);

        var amount = transform.parent.GetComponent<EnemyBasicAI>().Attack;
        player.GetComponent<PlayerBehaviour>().ReceiveDamage(amount);

        _anim.SetBool("isAttacking", false);
        yield return null; 

        yield return null;
        _enemy.EnemyState = EnemyBasicAI.EnemyStates.Running;
    }
}
