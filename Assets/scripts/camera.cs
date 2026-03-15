using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class camera : MonoBehaviour

{
    [Header("Follow Settings")]
    public Transform target; // Цель (ваш персонаж)
    private float smoothSpeed = 0.125f; // Плавность следования
    private Vector3 offset; // Смещение камеры
    private float zPosition = -10f;
    public float y = 0.2f;
    public float x = 0.1f;


    void FixedUpdate()
    {
        if (target == null) return;

        // Вычисляем целевую позицию
        Vector3 desiredPosition = target.position + offset;
        transform.position = new Vector3(transform.position.x + x, transform.position.y + y  , zPosition);

        // Плавно перемещаем камеру
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

