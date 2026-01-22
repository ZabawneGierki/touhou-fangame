using System.Collections;
using UnityEngine;

public class PlayerMagnet : MonoBehaviour
{
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetForce = 10f;
    [SerializeField] private LayerMask collectibleLayer;

    [SerializeField] private float itemGetBorderLine = 2f;
    [SerializeField] private float vacuumRadius = 100f; // Large radius when above border line

    private Rigidbody2D playerRb;
    private Collider2D[] nearbyCollectibles = new Collider2D[100]; // Reusable array for performance

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        if (playerRb == null)
        {
            Debug.LogError("PlayerMagnet requires a Rigidbody2D component on the player.");
        }
    }

    private void FixedUpdate()
    {
        AttrackNearbyItems();
    }

    private void AttrackNearbyItems()
    {
        // Determine the effective magnet radius
        float effectiveRadius = transform.position.y > itemGetBorderLine ? vacuumRadius : magnetRadius;

        // Find all collectibles within the effective radius
        int count = 0;
        Collider2D[] foundCollectibles = Physics2D.OverlapCircleAll(
            transform.position,
            effectiveRadius,
            collectibleLayer
        );
        // Copy foundCollectibles to nearbyCollectibles for compatibility with rest of code
        count = Mathf.Min(foundCollectibles.Length, nearbyCollectibles.Length);
        for (int i = 0; i < count; i++)
        {
            nearbyCollectibles[i] = foundCollectibles[i];
        }

        // Apply force to each nearby collectible
        for (int i = 0; i < count; i++)
        {
            Rigidbody2D itemRb = nearbyCollectibles[i].GetComponent<Rigidbody2D>();
            if (itemRb != null)
            {
                // Calculate direction towards player
                Vector2 directionToPlayer = (transform.position - nearbyCollectibles[i].transform.position).normalized;
                
                // Apply accelerating force
                itemRb.linearVelocity = Vector2.Lerp(itemRb.linearVelocity, directionToPlayer * magnetForce, Time.fixedDeltaTime * 5f);
            }
        }
    }

    // Optional: Visualize the magnet radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);

        // Draw the item border line
        Gizmos.color = Color.green;
        float lineWidth = 5f;
        Gizmos.DrawLine(
            transform.position + Vector3.left * lineWidth,
            transform.position + Vector3.right * lineWidth
        );
        Gizmos.DrawLine(
            transform.position + Vector3.up * itemGetBorderLine,
            transform.position + Vector3.up * itemGetBorderLine + Vector3.left * lineWidth
        );
        Gizmos.DrawLine(
            transform.position + Vector3.up * itemGetBorderLine,
            transform.position + Vector3.up * itemGetBorderLine + Vector3.right * lineWidth
        );

        // Draw vacuum radius when above border
        if (transform.position.y > itemGetBorderLine)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, vacuumRadius);
        }
    }
}
