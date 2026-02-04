using UnityEngine;

public class YinYangEnemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

     
    public void Destroy()
    {
        // Spawn bullets when destroyed in circular pattern
        int bulletCount;
        if(PlayerData.gameDifficulty == Difficulty.Hard || PlayerData.gameDifficulty == Difficulty.Lunatic)
        {
            bulletCount = 16;
        }
        else
        {
            bulletCount = 8;
        }
         
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = i * (360f / bulletCount);
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);
                // move the bullet in the direction it's facing
                bullet.GetComponent<Rigidbody2D>().linearVelocity = bullet.transform.up * 5f * PlayerData.GetDifficultyMultiplier(); // Adjust speed as needed


            }
         
    }
}
