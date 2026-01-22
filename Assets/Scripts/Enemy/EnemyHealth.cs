using System;
using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private const string ProjectileTag = "Projectile";
    public int  scoreValue = 10;
    public int maxHealth = 100;
    private int currentHealth;

    // string tag for player projectiles
    private string projectileTag = ProjectileTag;
    private string miniProjectileTag = "MiniProjectile";  


    [SerializeField] GameObject pointPickUp, powerUpPickUp;
    [SerializeField] float spawnRadius = 0.2f; // Radius for randomized spawn locations

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        Invoke("SelfDestroy", 10f); // Self-destruct after 20 seconds
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(projectileTag))
        {
            TakeDamage(20); // Assume each player attack does 20 damage
        }
        else if (collision.CompareTag(miniProjectileTag))
        {
            TakeDamage(10); // Assume mini projectiles do 10 damage
        }
    }
 

    private void TakeDamage(int v)
    {
        if (currentHealth <= 0)
            Die();
         currentHealth -= v;
            StartCoroutine(FlashRed());
    }

    private IEnumerator FlashRed()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;

    }

    private void Die()
    {
        Score.instance.AddScore(scoreValue);
        SpawnItems();
        // Add death effects here (e.g., play animation, sound, etc.)
        Destroy(gameObject);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void SpawnItems()
    {
        Vector3 enemyPosition = transform.position;

        // Spawn 2 point items at different random locations
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition(enemyPosition);
            Instantiate(pointPickUp, spawnPosition, Quaternion.identity);
        }

        // 10% chance to spawn a power-up
        if (UnityEngine.Random.value < 0.5f)
        {
            Vector3 powerUpPosition = GetRandomSpawnPosition(enemyPosition);
            Instantiate(powerUpPickUp, powerUpPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomSpawnPosition(Vector3 centerPosition)
    {
        // Get a random point within a circle around the enemy
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
        return centerPosition + new Vector3(randomOffset.x, randomOffset.y, 0f);
    }
}
