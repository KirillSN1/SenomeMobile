using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicAI : EnemySettings
{
    [Header("AI Settings")]
    public float ChaseRadius = 5f;          // радиус преследования

    [Header("Unity Settings")]
    public Transform Target;
    public Vector3 HomePosition;             // позиция, куда возвращается враг, если игрок вышел за пределы ChaseRadius
 //   public float StoppingDistance = 3f;   // максимальное расстояние, на которое враг может приблизиться к игроку

    //public float RetreatDistance = 4.0f;

    public Animator Anim;

    private enum LookingDirections { Left = -1, Right = 1 };     // для анимации - выбор стороны для поворота

    public enum EnemyStates { Idling, Attacking, Running, ReceivingDamage, Dying };
    public EnemyStates EnemyState = EnemyStates.Idling;

    void Awake()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        Anim = GetComponent<Animator>();

        IsAlive = true;
        Anim.SetBool("isAlive", IsAlive);
    }

    
    void Update()
    {
        if (Health <= 0)
        {
            IsAlive = false;
        }

        if(EnemyState != EnemyStates.Attacking && EnemyState != EnemyStates.ReceivingDamage && EnemyState != EnemyStates.Dying)        ////
            ChaseThePlayer();
    }


    private IEnumerator WanishingAnimation()
    {
        IsAlive = false;
        EnemyState = EnemyStates.Dying;

        Anim.SetBool("isAlive", IsAlive);
        yield return null;

        yield return new WaitForSeconds(.6f);

        Destroy(gameObject);

        yield return null;

    }

    public IEnumerator ReceiveDamage(int takenDamage)
    {
        Health -= takenDamage;
        EnemyState = EnemyStates.ReceivingDamage;

        Debug.Log("Enemy got hit!");

        Anim.SetBool("isRunningEnemy", false);
        Anim.SetBool("isReceivingDamage", true);
        yield return null;

        yield return new WaitForSeconds(.6f);            // Knockback доделать

        yield return null;

        if(Anim != null)
        {
          Anim.SetBool("isReceivingDamage", false);
        }
        
        if (Health <= 0)
        {
            StartCoroutine(WanishingAnimation());
        }

        yield return null;
    }

    public void ChaseThePlayer()
    {

        var toHomePosition = new Vector3(HomePosition.x, transform.position.y, 0);   // сохраняем значение оси Oy врага, чтобы спрайт не прыгал
        var distanceToHome = Vector3.Distance(transform.position, toHomePosition);

        var toTarget = new Vector3(Target.position.x, transform.position.y, 0);
        var distanceToTarget = Vector3.Distance(transform.position, Target.position);

       
        if (distanceToTarget <= ChaseRadius)  // игрок в зоне преследования
        {
            EnemyState = EnemyStates.Running;
            transform.position = Vector3.MoveTowards(transform.position, toTarget, Speed * Time.deltaTime);

            AnimateRunning(toTarget);
        }
        else if(distanceToTarget > ChaseRadius && distanceToHome != 0)    // игрок вышел за пределы радиуса преследования, но не дошел до HomePosition
        {
            EnemyState = EnemyStates.Running;
            transform.position = Vector3.MoveTowards(transform.position, toHomePosition, Speed * Time.deltaTime);

             AnimateRunning(toHomePosition);          
        }
        else 
        {
            Anim.SetBool("isRunningEnemy", false);      // idle state
            EnemyState = EnemyStates.Idling;
        }

    }

    private void AnimateRunning(Vector3 target)
    {
        Anim.SetBool("isRunningEnemy", true);

        if (target.x > transform.position.x)
        {
            ChooseDirection(LookingDirections.Right);
        }
        else if (target.x < transform.position.x)
        {
            ChooseDirection(LookingDirections.Left);
        }

    }

    private void ChooseDirection(LookingDirections motionState)
    {
        switch (motionState)
        {
            case LookingDirections.Right:               
                Anim.SetFloat("motionH", 1);            
                break;

            case LookingDirections.Left:
                Anim.SetFloat("motionH", -1);
                break;
        }
    }

    void OnDrawGizmosSelected()      // рисует радиус преследования врага, при выборе врага на сцене
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ChaseRadius);

     //   Gizmos.color = Color.green;
     //   Gizmos.DrawWireSphere(transform.position, StoppingDistance);
    }
}
