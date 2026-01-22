using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StraightShotPattern", menuName = "Scriptable Objects/StraightShotPattern")]
public class StraightShotPattern : ShootingPattern
{

    public GameObject bulletPrefab;
    float bulletSpeed = GameData.BASE_BULLET_SPEED * PlayerData.GetDifficultyMultiplier();
    public int burstCount = 5;
    public override IEnumerator ExecutePattern(Transform shooter)
    {
        for (int i = 0; i < burstCount; i++)
        {
             
            var bullet = Instantiate(bulletPrefab, shooter.position, shooter.rotation);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = shooter.up * -bulletSpeed;
            yield return new WaitForSeconds(fireRate);
        }
    }
}
