using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHeath : MonoBehaviour
{
    [System.Serializable]
    public class HealthEvent : UnityEvent<int> { }

    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public float invincibilityTime = 1f;

    [Header("Events")]
    public HealthEvent onDamageTaken;
    public HealthEvent onHealthChanged;
    public UnityEvent onDeath;

    private bool isInvincible;
    private float invincibilityTimer;

    void Start()
    {
        currentHealth = maxHealth;
        onHealthChanged?.Invoke(currentHealth);
    }

    void Update()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
                isInvincible = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);

        onDamageTaken?.Invoke(damage);
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Становимся неуязвимыми
            isInvincible = true;
            invincibilityTimer = invincibilityTime;

            // Мигание спрайта
            StartCoroutine(FlashSprite());
        }
    }

    System.Collections.IEnumerator FlashSprite()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float flashTime = 0.1f;
        int flashes = Mathf.FloorToInt(invincibilityTime / (flashTime * 2));

        for (int i = 0; i < flashes; i++)
        {
            sr.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(flashTime);
            sr.color = Color.white;
            yield return new WaitForSeconds(flashTime);
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        onHealthChanged?.Invoke(currentHealth);
    }

    void Die()
    {
        // Анимация смерти
        Animator anim = GetComponent<Animator>();
        anim.SetTrigger("Death");

        // Отключаем управление
        GetComponent<Player2>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        onDeath?.Invoke();

        // Уничтожаем через 2 секунды
        Destroy(gameObject, 2f);
    }
}

