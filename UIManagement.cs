using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthBar;


    public PickUps1 pickUps;

    public float health, maxHealth, width, height;

    public int maxRayos = 6;
    public int currentRays;

    public Image[] rayosIconos;
    public Image TickObjeto1;
    public Image TickObjeto2;
    public Image TickObjeto3;

    void Start()
    {
        currentRays = maxRayos;
        pickUps = FindAnyObjectByType<PickUps1>();
        TickObjeto1.gameObject.SetActive(false);
        TickObjeto2.gameObject.SetActive(false);
        TickObjeto3.gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (pickUps.objetoNave1)
        {
            TickObjeto1.gameObject.SetActive(true);
        }

        if (pickUps.objetoNave2)
        {
            TickObjeto2.gameObject.SetActive(true);
        }

        if (pickUps.objetoNave3)
        {
            TickObjeto3.gameObject.SetActive(true);
        }
    }

    public void SetMaxHealth(float _maxHealth)
    {
        maxHealth = _maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;
        float newwidth = (health / maxHealth) * width;

        healthBar.sizeDelta = new Vector2(newwidth, height);
    }

    public void UpdateUIRays()
    {
        for (int i = 0; i < rayosIconos.Length; i++)
        {
            if (i < currentRays)
            {
                rayosIconos[i].enabled = true; // Activa el ícono del rayo si hay rayos disponibles
            }
            else
            {
                rayosIconos[i].enabled = false; // Desactiva el ícono del rayo si no hay más rayos disponibles
            }
        }
    }
}
