using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float TimeBetweenAttack = .2f;  
    public float TimeTillAttack;

    private EnemyBasicAI enemy;
 
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBasicAI>();
    }


    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            Debug.Log("Time till attack" + TimeTillAttack);

            if (TimeTillAttack <= 0)
            {
                enemy.AttackThePlayer(other);
                TimeTillAttack = TimeBetweenAttack;
            }
            TimeTillAttack -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TimeTillAttack = 0;
       // enemy.AttackThePlayer(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TimeTillAttack = TimeBetweenAttack;
    }
}
