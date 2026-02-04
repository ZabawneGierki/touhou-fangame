using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    public float maxDistance = 20f;
    public float damagePerSecond = 25f;
    public LayerMask hitMask;
    public LineRenderer linePrefab;
    public ParticleSystem hitParticlesPrefab;

    private Coroutine shootingCoroutine;

    public override void StartShooting(GameObject player)
    {
        if (shootingCoroutine == null)
        {
             
            // Fix: Get MonoBehaviour from player to start coroutine
            if (player.TryGetComponent<MonoBehaviour>(out var mono))
            {
                shootingCoroutine = mono.StartCoroutine(ShootSpark());
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
            if (player.TryGetComponent<MonoBehaviour>(out var mono))
            {
                mono.StopCoroutine(shootingCoroutine);
            }
            shootingCoroutine = null;
        }

    }

    private IEnumerator ShootSpark()
    {
        int counter = 0;
        yield return new WaitForSeconds(1f);
        Debug.Log("Counter: " + counter);
    }
}
