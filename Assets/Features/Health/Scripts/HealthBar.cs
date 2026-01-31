using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    
    public Slider healthSlider;

    void Awake()
    {
        healthSlider.value = healthSlider.maxValue;
    }

    public void lowerHealthBar(int damage)
    {
        healthSlider.value -= damage;
    }
}
