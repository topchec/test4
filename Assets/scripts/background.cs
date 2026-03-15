using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    private float zpos = 4.5f;
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            Vector3 camPos = Camera.main.transform.position;
            transform.position = new Vector3(camPos.x, camPos.y, 10f);
        }
    }
}