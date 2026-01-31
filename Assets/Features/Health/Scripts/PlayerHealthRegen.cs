using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthRegen : MonoBehaviour
{

    public float timer;
    public float maxTime;

    public Health playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer >= maxTime)
        {
            playerHealth.fullHeal();
            timer = 0;
        }
    }
}
