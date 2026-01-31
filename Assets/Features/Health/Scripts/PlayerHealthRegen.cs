using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegen : MonoBehaviour
{

    public float timer;
    public float maxTime;
    public Health playerHealth;
    
    void Update()
    {
        if(playerHealth.isDead == true)
        {
            timer = 0;
            return;
        }
        if(playerHealth.currentHP == playerHealth.maxHP)
        {
            timer = 0;
            return;
        }

        timer += Time.deltaTime;
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
