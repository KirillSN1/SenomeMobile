using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicAI : EnemySettings
{
    public float ChaseRadius = 5f;          // радиус преследования

    public Transform Target;
    public Transform HomePosition;         // позиция, куда возвращается враг, если игрок вышел за пределы ChaseRadius
    public float StoppingDistance = 1.0f;   // максимальное расстояние, на которое враг может приблизиться к игроку

    //public float RetreatDistance = 4.0f;

    private enum MotionDirections { Up, Down, Left, Right };     // для анимации

  //  private enum EnemyStates { Idling, Attacking, Walking, Dying };
  //  private EnemyStates enemyState = EnemyStates.Idling;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        ChaseThePlayer();
    }

    public void AttackThePlayer(Collider2D other)
    {
        if (other != null)
        {
            var player = other.GetComponent<PlayerBehaviour>();
            StartCoroutine(player.ReceiveDamage(Attack)); 
        }

    }

    public void ChaseThePlayer()
    {
       
       var distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if(distanceToTarget <= ChaseRadius && distanceToTarget != StoppingDistance)  // игрок в зоне преследования
        {
            var toTarget = new Vector3(Target.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, toTarget, Speed * Time.deltaTime);

          //  Debug.Log("Enemy chasing!");
        }
        else if(distanceToTarget > ChaseRadius)          // игрок вышел за пределы радиуса преследования
        {
           var toHomePosition = new Vector3(HomePosition.position.x, transform.position.y, 0);
           transform.position = Vector3.MoveTowards(transform.position, toHomePosition, Speed * Time.deltaTime);

        //    Debug.Log("Enemy going Home!");
        }
        else     // = distanceToTarget == StoppingDistance
        {
            // idle state
         //   Debug.Log("Enemy waiting!");
        }
     
    }
}
