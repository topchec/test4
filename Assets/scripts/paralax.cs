using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paralax : MonoBehaviour
{
    [System.Serializable]
    public class Layer
    {
        public GameObject spriteObject;
        public float speedFactor = 1f;
        public Vector2 direction = Vector2.right;
        public bool loop = true;
        [HideInInspector] public float textureWidth;
    }

    [Header("Слои фона")]
    [SerializeField] private Layer[] backgroundLayers;

    [Header("Целевая камера")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float smoothing = 1f;

    private Vector3 previousCamPos;

    void Start()
    {
        previousCamPos = cameraTarget.position;

        // Инициализация текстур
        foreach (var layer in backgroundLayers)
        {
            if (layer.spriteObject != null)
            {
                SpriteRenderer sr = layer.spriteObject.GetComponent<SpriteRenderer>();
                if (sr != null && sr.sprite != null)
                {
                    layer.textureWidth = sr.sprite.texture.width / sr.sprite.pixelsPerUnit;
                }
            }
        }
    }

    void Update()
    {
        foreach (var layer in backgroundLayers)
        {
            if (layer.spriteObject == null) continue;

            // Параллакс движение
            float parallax = (previousCamPos.x - cameraTarget.position.x) * layer.speedFactor;
            float parallaxY = (previousCamPos.y - cameraTarget.position.y) * layer.speedFactor * 0.5f;

            Vector3 newPos = layer.spriteObject.transform.position +
                            new Vector3(parallax * layer.direction.x,
                                       parallaxY * layer.direction.y,
                                       0);

            layer.spriteObject.transform.position = Vector3.Lerp(
                layer.spriteObject.transform.position,
                newPos,
                smoothing * Time.deltaTime
            );

            // Бесконечная прокрутка
            if (layer.loop)
            {
                float offset = cameraTarget.position.x - layer.spriteObject.transform.position.x;
                if (Mathf.Abs(offset) >= layer.textureWidth)
                {
                    float direction = offset > 0 ? 1 : -1;
                    layer.spriteObject.transform.position += new Vector3(
                        layer.textureWidth * direction,
                        0,
                        0
                    );
                }
            }
        }

        previousCamPos = cameraTarget.position;
    }
}

