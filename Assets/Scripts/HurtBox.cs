using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    public float TimeBetweenAttack = .2f;   // это секунды
    public float TimeTillAttack;

    private EnemyBasicAI enemy;
    // Start is called before the first frame update
    void Start()
    {
        enemy = transform.parent.GetComponent<EnemyBasicAI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other != null && other.gameObject.tag == "Player")
        {
            //TimeTillAttack -= Time.deltaTime;
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
