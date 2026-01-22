using UnityEngine;

public class LaserGun : MonoBehaviour
{
    public float damagePerSecond = 10f;
    public float range = 10f;
    public LineRenderer lineRenderer;

    private bool firing;

     
    void Update()
    {
        firing = Input.GetButton("Fire1"); // Hold button to fire

        if (firing)
        {
            FireLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    void FireLaser()
    {
        Vector2 origin = transform.position;
        Vector2 direction = transform.up;

        // Raycast to detect enemy or obstacle
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, range);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, origin);

        if (hit.collider != null)
        {
            // End beam at the hit point
            lineRenderer.SetPosition(1, hit.point);

            // Apply damage over time
          /*  Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            } */
        }
        else
        {
            // Nothing hit → draw to max range
            lineRenderer.SetPosition(1, origin + direction * range);
        }
    }
}


