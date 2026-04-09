using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killzone : MonoBehaviour
{
    public Transform respawnPoint; // Точка спавна

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Перемещаем игрока на точку спавна
            other.transform.position = respawnPoint.position;
        }
    }
}