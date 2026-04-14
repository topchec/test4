using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameradeepseek : MonoBehaviour
//{
//    [Header("Follow Settings")]
//    public Transform target; // Цель (ваш персонаж)
//    public float smoothSpeed = 0.125f; // Плавность следования (сделал public)
//    public Vector3 offset; // Смещение камеры (сделал public)
//    public float y = 0.2f;
//    public float x = 0.1f;
//    public float zPosition = -10f;

//    [Header("Camera Switch")]
//    public Camera mainCamera;
//    public Camera zoneCamera;

//    private Camera previousCamera;
//    private bool isInZone = false;

//    private void Start()
//    {
//        // Инициализация камер
//        if (mainCamera == null)
//            mainCamera = Camera.main;

//        if (zoneCamera != null)
//            zoneCamera.enabled = false;

//        // Инициализация смещения, если оно не задано
//        if (offset == Vector3.zero && target != null)
//        {
//            offset = transform.position - target.position;
//        }
//    }

//    void FixedUpdate()
//    {
//        if (target == null) return;

//        // Исправленное вычисление позиции
//        Vector3 desiredPosition = target.position + offset;

//        // Применяем дополнительные смещения x и y
//        desiredPosition.x += x;
//        desiredPosition.y += y;
//        desiredPosition.z = zPosition;

//        // Плавно перемещаем камеру
//        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
//        transform.position = smoothedPosition;
//    }

//    void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && !isInZone && zoneCamera != null)
//        {
//            isInZone = true;
//            previousCamera = mainCamera;

//            // Отключаем основную камеру, включаем камеру зоны
//            if (mainCamera != null)
//                mainCamera.enabled = false;

//            zoneCamera.enabled = true;
//        }
//    }

//    void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player") && isInZone && mainCamera != null)
//        {
//            isInZone = false;

//            // Возвращаем основную камеру
//            mainCamera.enabled = true;

//            if (zoneCamera != null)
//                zoneCamera.enabled = false;
//        }
//    }
//}
{
    [Header("Follow Settings")]
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float y = 0.2f;
    public float x = 0.1f;
    public float zPosition = -10f;

    [Header("Camera Switch")]
    public Camera mainCamera;
    public string zoneCameraTag = "ZoneCamera"; // Тег для поиска камеры зоны
    private Camera zoneCamera;

    private bool isInZone = false;

    void Start()
    {
        // Находим основную камеру
        if (mainCamera == null)
            mainCamera = Camera.main;

        // Ищем камеру зоны ПО ТЕГУ
        FindZoneCameraByTag();

        // Убеждаемся, что камера зоны выключена
        if (zoneCamera != null)
            zoneCamera.enabled = false;
        else
            Debug.LogWarning("Zone camera with tag '" + zoneCameraTag + "' not found!");
    }

    void FindZoneCameraByTag()
    {
        GameObject zoneCamObject = GameObject.FindGameObjectWithTag(zoneCameraTag);

        if (zoneCamObject != null)
        {
            zoneCamera = zoneCamObject.GetComponent<Camera>();
            if (zoneCamera != null)
            {
                Debug.Log("Zone camera found: " + zoneCamera.name);
            }
            else
            {
                Debug.LogError("Object with tag '" + zoneCameraTag + "' has no Camera component!");
            }
        }
        else
        {
            Debug.LogWarning("No object with tag '" + zoneCameraTag + "' found in scene!");
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.x += x;
        desiredPosition.y += y;
        desiredPosition.z = zPosition;

        // Двигаем активную камеру
        if (isInZone && zoneCamera != null && zoneCamera.enabled)
        {
            zoneCamera.transform.position = Vector3.Lerp(zoneCamera.transform.position, desiredPosition, smoothSpeed);
        }
        else if (!isInZone && mainCamera != null && mainCamera.enabled)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredPosition, smoothSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered zone!");

            // Если камера зоны еще не найдена, ищем еще раз
            if (zoneCamera == null)
                FindZoneCameraByTag();

            if (zoneCamera != null && !isInZone)
            {
                isInZone = true;

                if (mainCamera != null)
                    mainCamera.enabled = false;

                zoneCamera.enabled = true;

                // Телепортируем камеру зоны к игроку
                Vector3 zoneCamPos = target.position + offset;
                zoneCamPos.x += x;
                zoneCamPos.y += y;
                zoneCamPos.z = zPosition;
                zoneCamera.transform.position = zoneCamPos;

                Debug.Log("Switched to zone camera");
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isInZone)
        {
            Debug.Log("Player exited zone!");

            isInZone = false;

            if (mainCamera != null)
                mainCamera.enabled = true;

            if (zoneCamera != null)
                zoneCamera.enabled = false;

            Debug.Log("Switched to main camera");
        }
    }
}
