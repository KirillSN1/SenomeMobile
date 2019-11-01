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

    private Animator _anim;

    private enum LookingDirections { Left = -1, Right = 1 };     // для анимации - выбор стороны для поворота

    private enum EnemyStates { Idling, Attacking, Running, ReceivingDamage, Dying };
    private EnemyStates enemyState = EnemyStates.Idling;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        _anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        ChaseThePlayer();
    }

    public IEnumerator ReceiveDamage(int takenDamage)
    {
        Health -= takenDamage;
        yield return null;
        //   yield return new WaitForSeconds(3f);      
    }

    public IEnumerator AttackThePlayer(Collider2D other)
    {
        Debug.Log("Enemy Attacking");
        if (other != null)
        {
            _anim.SetBool("isAttacking", true);
            _anim.SetBool("isRunningEnemy", false);
            var player = other.GetComponent<PlayerBehaviour>();

            Debug.Log("Attack Player");
          //  yield return null;

           // yield return new WaitForSeconds(.3f);
            StartCoroutine(player.ReceiveDamage(Attack));

            
           _anim.SetBool("isAttacking", false);

        }
        
         yield return null;

        //PlayerMovement.Anim.SetBool("isAttacking", true);
        ////  StartCoroutine(InstantiateArrow(placeToSpawn, placeToSpawnRotation));

        //yield return null;

        //yield return new WaitForSeconds(.6f);
        //StartCoroutine(InstantiateArrow(placeToSpawn, placeToSpawnRotation));

        //PlayerMovement.Anim.SetBool("isAttacking", false);
        //yield return null;
    }

    public void ChaseThePlayer()
    {
        var toHomePosition = new Vector3(HomePosition.x, transform.position.y, 0);   // сохраняем значение оси Oy врага, чтобы спрайт не прыгал
        var distanceToHome = Vector3.Distance(transform.position, toHomePosition);

        var distanceToTarget = Vector3.Distance(transform.position, Target.position);

       
        if (distanceToTarget <= ChaseRadius)  // игрок в зоне преследования
        {
            var toTarget = new Vector3(Target.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, toTarget, Speed * Time.deltaTime);

            AnimateRunning(toTarget);
        }
        else if(distanceToTarget > ChaseRadius && distanceToHome != 0)    // игрок вышел за пределы радиуса преследования, но не дошел до HomePosition
        {
             transform.position = Vector3.MoveTowards(transform.position, toHomePosition, Speed * Time.deltaTime);

             AnimateRunning(toHomePosition);          
        }
        else 
        {
            _anim.SetBool("isRunningEnemy", false);      // idle state
        }

    }

    private void AnimateRunning(Vector3 target)
    {
        _anim.SetBool("isRunningEnemy", true);

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
                _anim.SetFloat("motionH", 1);            
                break;

            case LookingDirections.Left:
                _anim.SetFloat("motionH", -1);
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
