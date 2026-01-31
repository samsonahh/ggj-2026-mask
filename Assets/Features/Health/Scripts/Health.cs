using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{   
    public int maxHP;
    public int currentHP;
    

    public UnityEvent<int> onDamage;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHP = maxHP;
    }

    [Button("Damage")]
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        onDamage?.Invoke(damage);


        if (currentHP <= 0)
        {
            onDeath?.Invoke();
        }
    }
}
