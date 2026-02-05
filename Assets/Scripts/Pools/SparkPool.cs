using UnityEngine;
using System.Collections.Generic;

public class SparkPool : MonoBehaviour
{
    public static SparkPool Instance; // Singleton for easy access
    public GameObject laserPrefab;
    public int poolSize = 5;

    private List<SparkInstance> pool = new List<SparkInstance>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(laserPrefab, transform);
            SparkInstance laser = obj.GetComponent<SparkInstance>();
            laser.Deactivate();
            pool.Add(laser);
        }
    }

    public SparkInstance GetLaser()
    {
        foreach (var laser in pool)
        {
            if (!laser.gameObject.activeInHierarchy)
            {
                laser.gameObject.SetActive(true);
                return laser;
            }
        }
        return null; // All lasers in use!
    }
}