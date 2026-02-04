using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "OneAfterAnotherDynamicWave", menuName = "Scriptable Objects/Waves/OneAfterAnotherDynamicWave")]
public class OneAfterAnotherDynamicWave : Wave 
{
    public int EnemyCount;
    public GameObject Enemy;
    public SplineContainer[] splines = new SplineContainer[SplineCount];
    [Range(0f, 1f)]
    public float[ ] stopPoints = new float[SplineCount];
    public DynamicMovementData movementData;
     
    private const int SplineCount = 2;
    public override IEnumerator PlayWave()
    {
        for (int i = 0; i < EnemyCount; i++)
        {
            GameObject enemy = Instantiate(Enemy);
            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement == null)
            {
                Debug.LogError("Enemy prefab does not have an EnemyMovement component.");
                continue;
            }
            movementData.splineContainer = splines[i % splines.Length];
            movementData.StopPoint = stopPoints[i % stopPoints.Length];
            enemyMovement.SetDataAndStart(movementData, 0);
            yield return new WaitForSeconds(movementData.offsetBetweenEnemies);

        }
    }
}
