using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    [Header("Master Spark")]
    public LineRenderer line;
    public float maxDistance = 20f;
    public float damagePerSecond = 25f;
    public LayerMask hitMask;


    [Header("Effects")]
    public ParticleSystem hitParticles;
    public override void StartShooting(GameObject player)
    {
        Debug.Log("Master Spark Activated");
    }

    public override void StopShooting(GameObject player)
    {
       Debug.Log("Master Spark Deactivated");
    }

    void FireLaser()
    {
        line.enabled = true;

        Vector2 origin = shootingPoint.position;
        Vector2 direction = Vector2.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, hitMask);

        Vector3 endPos;

        if (hit.collider != null)
        {
            endPos = hit.point;

            // Damage
            //if (hit.collider.TryGetComponent<IDamageable>(out var dmg))
            //{
            //    dmg.TakeDamage(damagePerSecond);
            //}

            // Particles
            if (!hitParticles.isPlaying)
                hitParticles.Play();

            hitParticles.transform.position = hit.point;
        }
        else
        {
            endPos = origin + direction * maxDistance;

            if (hitParticles.isPlaying)
                hitParticles.Stop();
        }

        line.SetPosition(0, origin);
        line.SetPosition(1, endPos);
    }
}
