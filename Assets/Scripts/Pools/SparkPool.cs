using UnityEngine;
using System.Collections.Generic;

public class SparkPool : MonoBehaviour
{
    public static SparkPool Instance; // Singleton for easy access
    public GameObject laserPrefab;
    public int poolSize = 5;

    private List<LaserInstance> pool = new List<LaserInstance>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(laserPrefab, transform);
            LaserInstance laser = obj.GetComponent<LaserInstance>();

            // FIX: Actually disable the GameObject so the pool can find it
            obj.SetActive(false);

            pool.Add(laser);
        }
    }

    public LaserInstance GetLaser()
    {
        foreach (var laser in pool)
        {
            // Now this check will work because we set them to SetActive(false)
            if (!laser.gameObject.activeSelf)
            {
                laser.gameObject.SetActive(true);
                return laser;
            }
        }
        Debug.LogWarning("Pool is full! No inactive lasers found.");
        return null;
    }
}
