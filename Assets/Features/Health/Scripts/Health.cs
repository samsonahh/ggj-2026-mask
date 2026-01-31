using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{   
    public int maxHP;
    public int currentHP;
    
    public bool isDead = false;

    public UnityEvent<int> onDamage;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHP = maxHP;
    }

    [Button("Damage")]
    public void TakeDamage(int damage)
    {
        if (currentHP <= 0)
        {
            currentHP = 0;
            onDeath?.Invoke();
            isDead = true;
        }
        else if(isDead == false)
        {
            currentHP -= damage;
            if (currentHP >= maxHP)
                currentHP = maxHP;
            
            onDamage?.Invoke(damage);
        }
    }
    public void revive()
    {
        isDead = false;
        fullHeal();
    }


    [Button("FullHeal")]
    public void fullHeal()
    {
        currentHP = maxHP;
        onDamage?.Invoke(-currentHP);
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }
}
