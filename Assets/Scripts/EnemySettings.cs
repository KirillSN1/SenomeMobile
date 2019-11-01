using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySettings : MonoBehaviour
{
    private enum EnemyStates { Idling, Attacking, Running, ReceivingDamage, Dying };
    private EnemyStates enemyState = EnemyStates.Idling;

    [Header("Enemy Attributes")]
    public string Name = "Kevin";
    public int Attack = 1;    
    public float Speed = 2.0f;

  //  [HideInInspector]
    public int Health = 5;       // значение не менять, т.к. здоровье над врагом состоит из 5 сердечек

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
 }
