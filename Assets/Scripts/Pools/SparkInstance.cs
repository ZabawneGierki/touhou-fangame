using UnityEngine;

public class SparkInstance : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform originPoint;
    private float damagePerSecond;
    private float maxDistance;
    private LayerMask enemyLayer;
    private bool isActive;

    void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    public void Setup(Transform origin, float damage, float dist, LayerMask mask)
    {
        originPoint = origin;
        damagePerSecond = damage;
        maxDistance = dist;
        enemyLayer = mask;
        isActive = true;
        lineRenderer.enabled = true;
    }

    public void Deactivate()
    {
        isActive = false;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (!isActive || originPoint == null) return;

        Vector2 origin = originPoint.position;
        Vector2 direction = originPoint.up; // Shoots "up" relative to the gun

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, enemyLayer);
        lineRenderer.SetPosition(0, origin);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);

            // Damage logic (Replace 'EnemyScript' with your actual script name)
            if (hit.collider.TryGetComponent(out EnemyHealth enemy))
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
        else
        {
            lineRenderer.SetPosition(1, origin + direction * maxDistance);
        }
    }
}