using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] ItemData itemData;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
             if(itemData.itemType == ItemType.PowerUp)
            {
                PowerUp powerUp = collision.GetComponent<PowerUp>();
                if(powerUp != null)
                {
                    powerUp.AddPoints(itemData.value);
                    
                    Score.instance.AddScore(itemData.value);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("PowerUp component not found on Player.");
                }
            }
             else if(itemData.itemType == ItemType.Points)
            {
                // Assuming there's a ScoreManager to handle points
                Score.instance.AddScore(itemData.value);
                Destroy(gameObject);
            }


        }
    }
}
