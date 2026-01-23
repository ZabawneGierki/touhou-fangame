using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject miniProjectilePrefab;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private float defaultLifetime = 5f;

    private Queue<GameObject> projectilePool;
    private Queue<GameObject> miniProjectilePool;

    void Awake()
    {
        projectilePool = new Queue<GameObject>(poolSize);
        miniProjectilePool = new Queue<GameObject>(poolSize);

        FillPool(projectilePrefab, projectilePool, poolSize, isMini: false);
        FillPool(miniProjectilePrefab, miniProjectilePool, poolSize, isMini: true);
    }

    private void FillPool(GameObject prefab, Queue<GameObject> pool, int count, bool isMini)
    {
        if (prefab == null) return;
        for (int i = 0; i < count; i++)
        {
            var go = CreatePooledInstance(prefab, isMini);
            pool.Enqueue(go);
        }
    }

    private GameObject CreatePooledInstance(GameObject prefab, bool isMini)
    {
        var go = Instantiate(prefab);
        go.SetActive(false);

        var pooled = go.GetComponent<PooledProjectile>();
        if (pooled == null)
            pooled = go.AddComponent<PooledProjectile>();

        pooled.Owner = this;
        pooled.IsMini = isMini;
        return go;
    }

    private Queue<GameObject> GetPool(bool isMini) => isMini ? miniProjectilePool : projectilePool;

    // Spawn a projectile from the pool.
    // position, rotation: where to place the projectile
    // velocity: optional; if the projectile has a Rigidbody2D it will be set
    // isMini: choose which prefab/pool to use
    // lifetime: how long until it is returned automatically (falls back to defaultLifetime)
    public GameObject SpawnProjectile(Vector2 position, Quaternion rotation, Vector2? velocity = null, bool isMini = false, float? lifetime = null)
    {
        var pool = GetPool(isMini);
        GameObject go = null;

        if (pool == null)
            return null;

        if (pool.Count > 0)
        {
            go = pool.Dequeue();
        }
        else
        {
            // Grow pool on demand
            var prefab = isMini ? miniProjectilePrefab : projectilePrefab;
            go = CreatePooledInstance(prefab, isMini);
        }

        go.transform.SetPositionAndRotation(position, rotation);
        go.SetActive(true);

        var rb2d = go.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = velocity ?? Vector2.zero;
        }

        var pooled = go.GetComponent<PooledProjectile>();
        pooled?.Activate(lifetime ?? defaultLifetime);

        return go;
    }

    // Return a projectile to its pool
    public void ReturnToPool(GameObject go, bool isMini)
    {
        if (go == null) return;
        go.SetActive(false);
        var pool = GetPool(isMini);
        pool.Enqueue(go);
    }

    // Small helper component added to pooled projectiles so they can auto-return
    private class PooledProjectile : MonoBehaviour
    {
        public ProjectilePool Owner { get; set; }
        public bool IsMini { get; set; }

        private Coroutine lifeCoroutine;

        public void Activate(float lifetime)
        {
            if (lifeCoroutine != null)
                StopCoroutine(lifeCoroutine);

            lifeCoroutine = StartCoroutine(AutoReturnAfter(lifetime));
        }

        private IEnumerator AutoReturnAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Owner?.ReturnToPool(gameObject, IsMini);
        }

        private void OnDisable()
        {
            if (lifeCoroutine != null)
            {
                StopCoroutine(lifeCoroutine);
                lifeCoroutine = null;
            }
        }

        // Optional: if you want immediate return on collisions, uncomment:
         private void OnCollisionEnter2D(Collision2D other) => Owner?.ReturnToPool(gameObject, IsMini);
    }
}
