using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbWave", menuName = "Scriptable Objects/Waves/OrbWave")]
public class OrbWave : Wave
{
    [SerializeField] GameObject yinYangEnemyPrefab;
    [SerializeField] Vector3[] spawnPoints;

    public int enemyCount;
    public override IEnumerator PlayWave()
    {
        yield return null;
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(yinYangEnemyPrefab, spawnPoint, Quaternion.identity);
            yield return new WaitForSeconds(2f);
            // if spawns on the left move right else move left
            if (spawnPoint.x < -4)
            {
                enemy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(5f * PlayerData.GetDifficultyMultiplier(), 2f);
            }
            else
            {
                enemy.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(-5f * PlayerData.GetDifficultyMultiplier(), -2f);
            }


        }


    }
}
