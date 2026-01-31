using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{   
    private int basehealth;
    public int maxHP;
    public int currentHP;
    

    public UnityEvent<int> onDamage;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        onDamage?.Invoke(damage);


        if (currentHP <= 0)
        {
            onDeath?.Invoke();
        }
    }

    [ContextMenu("Test / Deal 10 Damage")]
    private void ContextDamageTest()
    {
        TakeDamage(10);
    }




}
