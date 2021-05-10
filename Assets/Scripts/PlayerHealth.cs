using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    public int healthValue = 100;

    private Slider health_Slider;
    private GameObject hudPanel;
    private PlayerController player;

    void Start()
    {
        health_Slider = GameObject.Find("HealthBar").GetComponent<Slider>();

        hudPanel = GameObject.Find("HudPanel");

        health_Slider.value = healthValue;

        player = GetComponent<PlayerController>();
    }

    public void ApplyDamage(int damageAmount)
    {
        healthValue -= damageAmount;
        
        health_Slider.value = healthValue;

        player.bloodeffect.Play();

        if(healthValue <= 0)
        {
            healthValue = 0;
            hudPanel.SetActive(false);
            GameplayController.Instance.GameOver();
        }
    }
}
