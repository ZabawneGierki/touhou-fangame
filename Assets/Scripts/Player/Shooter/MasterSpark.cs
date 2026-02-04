using System.Collections;
using UnityEngine;
 
 

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    [Header("Laser Settings")]
    public float damagePerSecond = 10f;
    public float maxDistance = 20f;
    public LayerMask enemyLayer;
    public Material laserMaterial;
    public float laserWidth = 0.1f;

    private LineRenderer currentLaser;
    private Coroutine shootRoutine;
    public override void StartShooting(GameObject player)
    {
        // 1. Setup the Line Renderer if it doesn't exist
        if (currentLaser == null)
        {
            GameObject laserObj = new GameObject("Laser_Effect");
            currentLaser = laserObj.AddComponent<LineRenderer>();
            currentLaser.material = laserMaterial;
            currentLaser.startWidth = laserWidth;
            currentLaser.endWidth = laserWidth;
            currentLaser.useWorldSpace = true;
        }

        currentLaser.enabled = true;

        // 2. Start the shooting logic loop
        var playerMono = player.GetComponent<MonoBehaviour>();
        if (shootRoutine != null && playerMono != null) playerMono.StopCoroutine(shootRoutine);
        if (playerMono != null) shootRoutine = playerMono.StartCoroutine(UpdateLaser(player));
    }

    public override void StopShooting(GameObject player)
    {
        if (currentLaser != null) currentLaser.enabled = false;
        var playerMono = player.GetComponent<MonoBehaviour>();
        if (shootRoutine != null && playerMono != null) playerMono.StopCoroutine(shootRoutine);
    }

    private IEnumerator UpdateLaser(GameObject player)
    {
        while (true)
        {
            Vector2 origin = shootingPoint.position;
            Vector2 direction = shootingPoint.up; // "Upwards" relative to the gun

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, enemyLayer);

            currentLaser.SetPosition(0, origin);

            if (hit.collider != null)
            {
                // Stop the laser at the enemy
                currentLaser.SetPosition(1, hit.point);

                // Damage logic: Look for a script on the enemy (e.g., "EnemyHealth")
                // Replace 'EnemyHealth' with your actual enemy script name
                var enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }
            else
            {
                // No hit: Extend laser to max distance
                currentLaser.SetPosition(1, origin + direction * maxDistance);
            }

            yield return null; // Wait for next frame
        }
    }
}
