using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegen : MonoBehaviour
{

    public float timer;
    public float maxTime;
    public Health playerHealth;
    
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= maxTime)
        {
            Debug.Log("Full Heal");
            playerHealth.fullHeal();
            timer = 0;  
        }
    }

    public void resetTimer()
    {
        timer = 0;
    }
}
