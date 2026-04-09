using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameradeepseek : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // Цель (ваш персонаж)
    public float smoothSpeed = 0.125f; // Плавность следования (сделал public)
    public Vector3 offset; // Смещение камеры (сделал public)
    public float y = 0.2f;
    public float x = 0.1f;
    public float zPosition = -10f;

    [Header("Camera Switch")]
    public Camera mainCamera;
    public Camera zoneCamera;

    private Camera previousCamera;
    private bool isInZone = false;

    private void Start()
    {
        // Инициализация камер
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (zoneCamera != null)
            zoneCamera.enabled = false;

        // Инициализация смещения, если оно не задано
        if (offset == Vector3.zero && target != null)
        {
            offset = transform.position - target.position;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        // Исправленное вычисление позиции
        Vector3 desiredPosition = target.position + offset;

        // Применяем дополнительные смещения x и y
        desiredPosition.x += x;
        desiredPosition.y += y;
        desiredPosition.z = zPosition;

        // Плавно перемещаем камеру
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isInZone && zoneCamera != null)
        {
            isInZone = true;
            previousCamera = mainCamera;

            // Отключаем основную камеру, включаем камеру зоны
            if (mainCamera != null)
                mainCamera.enabled = false;

            zoneCamera.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInZone && mainCamera != null)
        {
            isInZone = false;

            // Возвращаем основную камеру
            mainCamera.enabled = true;

            if (zoneCamera != null)
                zoneCamera.enabled = false;
        }
    }
}

