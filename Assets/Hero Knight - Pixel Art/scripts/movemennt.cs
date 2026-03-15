using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movemennt : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 10f;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //    Quaternion rot = transform.rotation;
        //    rot.y = 0;
        //    transform.rotation = rot;
        //}

        //if (Input.GetAxis("Horizontal") < 0)
        //{
        //    Quaternion rot = transform.rotation;
        //    rot.y = 180;
        //    transform.rotation = rot;
        //}
    }
    
}

