using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpbar : MonoBehaviour
{
    private Image healthBar;
    public HeroKnight HP;
    private float maxHp;

    void Start()
    {
        healthBar = GetComponent<Image>();
        if (HP == null)
            HP = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroKnight>();
        
        if (HP != null)
            maxHp = HP.hp;   // сохраняем начальное здоровье как максимум
    }

    void Update()
    {
        if (HP != null && healthBar != null)
            healthBar.fillAmount = (float)HP.hp / maxHp;
    }
}