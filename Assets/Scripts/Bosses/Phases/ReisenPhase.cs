using System.Collections;
using UnityEngine;
 
[CreateAssetMenu(fileName = "ReisenPhase", menuName = "Scriptable Objects/Phases/ReisenPhase")]
public class ReisenPhase : Phase
{
    [SerializeField] Vector2 pointA, pointB;

    [Header("Bullet")]
    [SerializeField] GameObject bullet;

    [Header("Bullet settings")]
    [SerializeField] int bulletCount = 12;
    [SerializeField] float bulletSpeed = 5f;
    [SerializeField] float bulletLifetime = 6f;
    [SerializeField] float spawnRadius = 0.5f;
    [SerializeField] float shootInterval = 1.5f;

    

    public override IEnumerator ExecutePhase(Boss boss)
    {
        
        // Start shooting bullets
        boss.StartCoroutine(ShootBullets(boss));

        // move to point A
        yield return boss.StartCoroutine(boss.MoveToPoint(pointA, 1.0f));

        while (true)
        {
            // move to point B
            yield return boss.StartCoroutine(boss.MoveToPoint(pointB, 2.0f));
            // move to point A
            yield return boss.StartCoroutine(boss.MoveToPoint(pointA, 2.0f));
        }
    }

    private IEnumerator ShootBullets(Boss boss)
    {
        while (true)
        {
            Vector2 center = boss.transform.position;
            
            

            for (int i = 0; i < bulletCount; i++)
                {
                    float angle = i * Mathf.PI * 2f / bulletCount;
                    Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

                    // spawn slightly offset from boss center
                    Vector2 spawnPos = center + dir * spawnRadius;
                    GameObject newBullet = Object.Instantiate(bullet, spawnPos, Quaternion.identity);

                    // set rotation so the bullet faces its travel direction (assumes right (x+) faces forward)
                    float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    newBullet.transform.rotation = Quaternion.Euler(0f, 0f, deg);

                    // if the prefab has a Rigidbody2D, use it for movement
                    var rb = newBullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.linearVelocity = dir * bulletSpeed;
                    }
                    else
                    {
                        // fallback: translate in Update isn't added here (prefab should have Rigidbody2D)
                        Debug.LogWarning("Bullet prefab has no Rigidbody2D — consider adding one for consistent behavior.", newBullet);
                    }

                    // ensure bullet is destroyed eventually
                    Object.Destroy(newBullet, bulletLifetime);
                }

            yield return new WaitForSeconds(shootInterval);
        }
    }
}
