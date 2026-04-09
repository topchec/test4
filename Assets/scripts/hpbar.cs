using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpbar : MonoBehaviour
{
    Image healthBar;
    public float maxhp = 100f;
    public HeroKnight HP;
    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
        HP = GameObject.Find("Player").GetComponent<HeroKnight>();
        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = HP.hp / maxhp;
    }
}
