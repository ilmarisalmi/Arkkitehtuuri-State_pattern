using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    public static Action OnEnemyDestroyed;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        { 
            Destroy(gameObject); 
            OnEnemyDestroyed?.Invoke(); 
        }
    }
}
