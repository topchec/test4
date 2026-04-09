using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerazoom : MonoBehaviour
{
    public Camera mainCamera;
    public float zoomOutDistance = 10f; // На сколько отдалить
    public float zoomSpeed = 2f; // Скорость отдаления

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool isZooming = false;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        originalPosition = mainCamera.transform.position;
        targetPosition = originalPosition;
    }

    void OnTriggerEnter2D(Collider2D other) // Для 2D игры
    {
        if (other.CompareTag("Player"))
        {
            targetPosition = originalPosition + new Vector3(0, 0, -zoomOutDistance);
            isZooming = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetPosition = originalPosition;
            isZooming = true;
        }
    }

    void Update()
    {
        if (isZooming)
        {
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                zoomSpeed * Time.deltaTime
            );
        }
    }
}

