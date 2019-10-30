using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasicAI : EnemySettings
{
    public float ChaseRadius = 5f;

    public Transform Target;
    public Transform HomePosition;
    public float StoppingDistance = 1.0f;
    //public float RetreatDistance = 4.0f;

    private enum MotionDirections { Up, Down, Left, Right };

    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        ChaseThePlayer();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other != null && other.gameObject.tag == "Player")
        {
            AttackThePlayer(other);
        }
    }

    public void AttackThePlayer(Collider2D other)
    {
        if (other != null)
        {
            var player = other.GetComponent<PlayerBehaviour>();
            player.ReceiveDamage(Attack);
        }

    }

    public void ChaseThePlayer()
    {
       
       var distanceToTarget = Vector3.Distance(transform.position, Target.position);

        if(distanceToTarget <= ChaseRadius && distanceToTarget != StoppingDistance)
        {
            var toTarget = new Vector3(Target.position.x, transform.position.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, toTarget, Speed * Time.deltaTime);

            Debug.Log("Enemy chasing!");
        }
        else if(distanceToTarget > ChaseRadius)
        {
           var toHomePosition = new Vector3(HomePosition.position.x, transform.position.y, 0);
           transform.position = Vector3.MoveTowards(transform.position, toHomePosition, Speed * Time.deltaTime);

           Debug.Log("Enemy going Home!");
        }
        else     // = distanceToTarget == StoppingDistance
        {
            // idle
        }
       


    }
}
