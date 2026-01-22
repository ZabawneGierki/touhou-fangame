using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "StaticEnemyWave", menuName = "Scriptable Objects/Waves/StaticEnemyWave")]
public class StaticEnemyWave : Wave 
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] ShootingPattern shootingPattern;

    public override IEnumerator PlayWave()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {

            GameObject enemy = Instantiate(enemyPrefab, spawnPoints[i].position, spawnPoints[i].rotation);
            EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
            StaticMovementData movementData = new StaticMovementData();
            movementData.shootingData = new ShootingData
            {
                shootingPattern = shootingPattern,
                 
            };
            movementData.initialPosition = spawnPoints[i].position;




             movement.SetDataAndStart(movementData);

            yield return null;



        }
    }
}
