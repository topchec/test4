using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    Transform Player;
    Animator anim;
    public Boss boss;
    public float inputX = -1;
    float speed = 5;
    void Start()
    {
        boss = FindFirstObjectByType<Boss>();
        anim = GetComponent<Animator>();
        rb= GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        
        inputX = boss.inputX;
        
        transform.localScale = new Vector3(6.0f*inputX, 6.0f, 1.0f);
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(inputX *speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("Explosion");
        speed = 0;
        /*inputX = 0;*/
        if (collision.GetComponent<HeroKnight>() != null )
        {
            collision.GetComponent<HeroKnight>().GetDamage(boss.damage);
        }
    }



    public void delete()
    {
        Destroy(this.gameObject);
    }
}
