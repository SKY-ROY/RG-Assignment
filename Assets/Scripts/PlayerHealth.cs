using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int healthValue = 100;

    private Slider health_Slider;
    private GameObject hudPanel;

    void Start()
    {
        health_Slider = GameObject.Find("HealthBar").GetComponent<Slider>();

        health_Slider.value = healthValue;

        hudPanel = GameObject.Find("HudPanel");
    }

    public void ApplyDamage(int damageAmount)
    {
        healthValue -= damageAmount;
        
        health_Slider.value = healthValue;

        if(healthValue <= 0)
        {
            healthValue = 0;
            hudPanel.SetActive(false);
            GameplayController.instance.GameOver();
        }

    }
}
