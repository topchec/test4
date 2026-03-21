using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hpbar : MonoBehaviour
{
    Image healthbar;
    public float maxhp = 100f;
    public float HP;
    // Start is called before the first frame update
    void Start()
    {
        healthbar = GetComponent<Image>();
        HP = maxhp;
    }

    // Update is called once per frame
    void Update()
    {
         healthbar.fillAmount = HP/maxhp;
    }
}
