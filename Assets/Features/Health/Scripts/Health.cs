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
    public UnityEvent<int> onHeal;
    public UnityEvent onDeath;

    void Awake()
    {
        currentHP = maxHP;
    }

    [Button("Damage")]
    public void TakeDamage(int damage)
    {
        if (isDead)
            return;
        
        currentHP -= damage;
        if (currentHP >= maxHP)
            currentHP = maxHP;
            
        onDamage?.Invoke(damage);
        
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            onDeath?.Invoke();
        }
    }
    
    [Button("Revive")]
    public void revive()
    {
        isDead = false;
        fullHeal();
    }


    [Button("FullHeal")]
    public void fullHeal()
    {
        Heal(maxHP);
    }

    public void Heal(int healAmount)
    {
        currentHP += healAmount;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
        onHeal.Invoke(healAmount);
    }
}
