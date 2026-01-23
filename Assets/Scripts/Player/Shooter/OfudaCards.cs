using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "OfudaCards", menuName = "Scriptable Objects/Shooters/OfudaCards")]
public class OfudaCards : ShootData
{
    [Header("Shooting")]
    
    [SerializeField] float fireRate = 0.09f;
    private Coroutine shootingCoroutine;
    private float nextFireTime = 0f;

     

    public override void StartShooting(GameObject player)
    { 
        if (shootingCoroutine == null)
        {
            nextFireTime = Time.time;
                // Fix: Get MonoBehaviour from player to start coroutine
            var mono = player.GetComponent<MonoBehaviour>();
            if (mono != null)
            {
                shootingCoroutine = mono.StartCoroutine(ShootCoroutine());
            }
            else
            {
                Debug.LogError("Player GameObject does not have a MonoBehaviour to start coroutine.");
            }
        }
    }

    public override void StopShooting(GameObject player)
    {
        if (shootingCoroutine != null)
        {
            var mono = player.GetComponent<MonoBehaviour>();
            if (mono != null)
            {
                mono.StopCoroutine(shootingCoroutine);
            }
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        Debug.Log("OfudaCards shooting coroutine started.");
        while (true)
        {
            if (Time.time >= nextFireTime)
            {
                GameObject bullet = ProjectilePool.Instance.SpawnProjectile( shootingPoint.position, Quaternion.identity);
                // shoot the bullet upwards
                bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * 20f;

                yield return null;

                nextFireTime = Time.time + fireRate;
            }
            yield return null;
        }
    }
}
