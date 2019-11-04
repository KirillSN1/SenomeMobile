using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates { Idling, Attacking, Running, ReceivingDamage, Dying };

public class EnemySettings : MonoBehaviour
{
   
    [Header("Enemy Attributes")]
    public string Name = "Kevin";
    public int Attack = 1;    
    public float Speed = 2.0f;

  //  [HideInInspector]
    public int Health = 5;       // значение не менять, т.к. здоровье над врагом состоит из 5 сердечек

    [HideInInspector]
    public EnemyStates enemyState = EnemyStates.Idling;

    
    void Start()
    {
        
    }


    void Update()
    {
        
    }

  
 }
