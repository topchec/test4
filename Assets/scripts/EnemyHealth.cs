using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 30;
    [SerializeField] private int currentHealth;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private GameObject deathEffect;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D enemyCollider;

    // Animation parameters
    private readonly int animHurt = Animator.StringToHash("Hurt");
    private readonly int animDeath = Animator.StringToHash("Death");

    private bool isDead = false;

    void Start()
    {
        InitializeComponents();
        currentHealth = maxHealth;
    }

    void InitializeComponents()
    {
        // Автоматически находим компоненты если не установлены
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (enemyCollider == null)
            enemyCollider = GetComponent<Collider2D>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");

        // Анимация получения урона
        if (animator != null)
            animator.SetTrigger(animHurt);

        // Проверяем смерть
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Отбрасывание при получении урона
            ApplyKnockback();
        }
    }

    void ApplyKnockback()
    {
        if (rb != null)
        {
            // Определяем направление от игрока
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector2 direction = (transform.position - player.transform.position).normalized;
                rb.velocity = direction * knockbackForce;
            }
        }
    }

    void Die()
    {
        isDead = true;

        // Анимация смерти
        if (animator != null)
            animator.SetTrigger(animDeath);

        // Отключаем физику и коллайдер
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }

        if (enemyCollider != null)
            enemyCollider.enabled = false;

        // Эффект смерти
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Уничтожаем объект через 2 секунды
        Destroy(gameObject, 2f);

        // Здесь можно добавить выпадение лута, очков и т.д.
        Debug.Log($"{gameObject.name} died!");
    }

    // Метод для лечения (опционально)
    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log($"{gameObject.name} healed for {amount}. Health: {currentHealth}/{maxHealth}");
    }

    // Метод для проверки смерти
    public bool IsDead()
    {
        return isDead;
    }

    // Метод для получения текущего здоровья
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}

