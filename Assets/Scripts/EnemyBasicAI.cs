using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicAI : EnemySettings
{
    public float ChaseRadius = 5f;          // радиус преследования

    public Transform Target;
    public Transform HomePosition;         // позиция, куда возвращается враг, если игрок вышел за пределы ChaseRadius
    public float StoppingDistance = 1.2f;   // максимальное расстояние, на которое враг может приблизиться к игроку

    //public float RetreatDistance = 4.0f;

    private Animator _anim;

    private enum LookingDirections { Left = -1, Right = 1 };     // для анимации

    private enum EnemyStates { Idling, Attacking, Walking, Dying };
    private EnemyStates enemyState = EnemyStates.Idling;

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
        _anim = GetComponent<Animator>();

        _anim.SetBool("isRunningEnemy", false);
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
        var toHomePosition = new Vector3(HomePosition.position.x, transform.position.y, 0);
        var distanceToHome = Vector3.Distance(transform.position, toHomePosition);

        var distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if (distanceToTarget <= ChaseRadius && distanceToTarget != StoppingDistance)  // игрок в зоне преследования
        {
            var toTarget = new Vector3(Target.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, toTarget, Speed * Time.deltaTime);

            AnimateRunning(toTarget);
        }
        else if(distanceToTarget > ChaseRadius && distanceToHome != 0)          // игрок вышел за пределы радиуса преследования
        {
             transform.position = Vector3.MoveTowards(transform.position, toHomePosition, Speed * Time.deltaTime);

             AnimateRunning(toHomePosition);          
        }
        else     // = distanceToTarget == StoppingDistance
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
                Debug.Log("Enemy going to right");
                _anim.SetFloat("motionH", 1);            
                break;

            case LookingDirections.Left:
                Debug.Log("Enemy going to left");
                _anim.SetFloat("motionH", -1);
                break;
        }
    }
}
