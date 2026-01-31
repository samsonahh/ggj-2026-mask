using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
    public Slider healthSlider;
    [SerializeField] private Health playerHealth;

    void Update()
    {
        healthSlider.value = playerHealth.currentHP/(float)playerHealth.maxHP;
    }

}
