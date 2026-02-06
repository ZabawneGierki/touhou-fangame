using System.Collections;
using UnityEngine;
 
 

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    public float damagePerSecond = 20f;
    public float maxDistance = 50f;
    public LayerMask enemyLayer;

    private LaserInstance activeLaser;

    public override void StartShooting(GameObject player)
    {
        if (shootingPoint == null)
        {
            Debug.LogError("Laser failed: shootingPoint is null! Did you call SetUpShootingPoint?");
            return;
        }

        if (LaserPool.Instance == null)
        {
            Debug.LogError("Laser failed: No LaserPool found in the scene!");
            return;
        }

        if (activeLaser == null)
        {
            activeLaser = LaserPool.Instance.GetLaser();
            if (activeLaser != null)
            {
                Debug.Log("Laser spawned successfully!");
                activeLaser.Setup(shootingPoint, damagePerSecond, maxDistance, enemyLayer);
            }
            else
            {
                Debug.LogWarning("Laser failed: Pool is empty!");
            }
        }
    }

    public override void StopShooting(GameObject player)
    {
        if (activeLaser != null)
        {
            activeLaser.Deactivate();
            activeLaser.gameObject.SetActive(false);
            activeLaser = null;
        }
    }
}
