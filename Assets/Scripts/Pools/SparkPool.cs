using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkPool : MonoBehaviour
{
    public static SparkPool Instance; // Singleton for easy access
    public GameObject laserPrefab;
    public int poolSize = 5;

    [SerializeField] Color[] laserColors;
    [SerializeField] Material laserMaterial;

    private List<SparkInstance> pool = new List<SparkInstance>();

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(laserPrefab, transform);
            SparkInstance laser = obj.GetComponent<SparkInstance>();

            // FIX: Actually disable the GameObject so the pool can find it
            obj.SetActive(false);

            pool.Add(laser);
        }

        // Start the color change routine
        StartCoroutine(ChangeLaserColorRoutine());
    }



    public SparkInstance GetLaser()
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

    public SparkInstance GetLaser(Direction direction)
    {
        foreach (var laser in pool)
        {
            // Now this check will work because we set them to SetActive(false)
            if (!laser.gameObject.activeSelf)
            {
                laser.gameObject.SetActive(true);
                SparkInstance sparkInstance = laser.GetComponent<SparkInstance>();
                sparkInstance.SetDirection(direction);
                return laser;
            }
        }
        Debug.LogWarning("Pool is full! No inactive lasers found.");
        return null;

    }

    private IEnumerator ChangeLaserColorRoutine()
    {
        // change laser color every 0.5 seconds gradually
        int colorIndex = 0;
        while (true)
        {
            Color startColor = laserColors[colorIndex];
            Color endColor = laserColors[(colorIndex + 1) % laserColors.Length];
            float transitionDuration = 0.5f;
            float elapsed = 0f;
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / transitionDuration);
                Color currentColor = Color.Lerp(startColor, endColor, t);
                laserMaterial.color = currentColor;
                yield return null;
            }
            colorIndex = (colorIndex + 1) % laserColors.Length;
        }
    }
}
