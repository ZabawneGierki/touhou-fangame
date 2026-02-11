using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Normal,
    Heavy,
    Shard
}

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject normalBulletPrefab;
    [SerializeField] private GameObject heavyBulletPrefab;
    [SerializeField] private GameObject shardBulletPrefab;
    [SerializeField] private int poolSize = 50;
    [SerializeField] private float defaultSpeed = 20f;
    [SerializeField] private float defaultLifetime = 5f;

    public static BulletPool Instance { get; private set; }

    private Queue<GameObject> normalPool;
    private Queue<GameObject> heavyPool;
    private Queue<GameObject> shardPool;

    void Awake()
    {
        // singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        normalPool = new Queue<GameObject>(poolSize);
        heavyPool = new Queue<GameObject>(poolSize);
        shardPool = new Queue<GameObject>(poolSize);

        FillPool(normalBulletPrefab, normalPool, poolSize, BulletType.Normal);
        FillPool(heavyBulletPrefab, heavyPool, poolSize, BulletType.Heavy);
        FillPool(shardBulletPrefab, shardPool, poolSize, BulletType.Shard);
    }

    private void FillPool(GameObject prefab, Queue<GameObject> pool, int count, BulletType type)
    {
        if (prefab == null) return;
        for (int i = 0; i < count; i++)
        {
            var go = CreatePooledInstance(prefab, type);
            pool.Enqueue(go);
        }
    }

    private GameObject CreatePooledInstance(GameObject prefab, BulletType type)
    {
        var go = Instantiate(prefab);
        go.SetActive(false);
        go.transform.SetParent(transform);

        var pooled = go.GetComponent<PooledBullet>();
        if (pooled == null)
        {
            pooled = go.AddComponent<PooledBullet>();
        }

        pooled.Owner = this;
        pooled.Type = type;

        var rb2d = go.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            rb2d.linearVelocity = Vector2.zero;

        return go;
    }

    private Queue<GameObject> GetPool(BulletType type)
    {
        return type switch
        {
            BulletType.Normal => normalPool,
            BulletType.Heavy => heavyPool,
            BulletType.Shard => shardPool,
            _ => normalPool
        };
    }

    public GameObject SpawnBullet(Vector2 position, Quaternion rotation, BulletType type = BulletType.Normal, Vector2? velocity = null, float? lifetime = null, float? speedOverride = null)
    {
        var pool = GetPool(type);
        GameObject go = null;

        if (pool == null)
            return null;

        if (pool.Count > 0)
        {
            go = pool.Dequeue();
        }
        else
        {
            var prefab = type switch
            {
                BulletType.Normal => normalBulletPrefab,
                BulletType.Heavy => heavyBulletPrefab,
                BulletType.Shard => shardBulletPrefab,
                _ => normalBulletPrefab
            };

            if (prefab == null) return null;
            go = CreatePooledInstance(prefab, type);
        }

        go.transform.SetPositionAndRotation(position, rotation);
        go.SetActive(true);

        var rb2d = go.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.linearVelocity = velocity ?? (Vector2.up * (speedOverride ?? defaultSpeed));
        }

        var pooled = go.GetComponent<PooledBullet>();
        pooled?.Activate(lifetime ?? defaultLifetime);

        return go;
    }

    public void ReturnToPool(GameObject go, BulletType type)
    {
        if (go == null) return;

        var rb2d = go.GetComponent<Rigidbody2D>();
        if (rb2d != null)
            rb2d.linearVelocity = Vector2.zero;

        go.SetActive(false);
        var pool = GetPool(type);
        pool.Enqueue(go);
    }

    private class PooledBullet : MonoBehaviour
    {
        public BulletPool Owner { get; set; }
        public BulletType Type { get; set; }

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
            Owner?.ReturnToPool(gameObject, Type);
        }

        private void OnDisable()
        {
            if (lifeCoroutine != null)
            {
                StopCoroutine(lifeCoroutine);
                lifeCoroutine = null;
            }
        }

        // Optional: return to pool on collision/trigger if required by your gameplay:
        // private void OnTriggerEnter2D(Collider2D other) => Owner?.ReturnToPool(gameObject, Type);
        // private void OnCollisionEnter2D(Collision2D other) => Owner?.ReturnToPool(gameObject, Type);
    }
}