using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Splines;


[Serializable]
public class MovementData
{
    public SplineContainer splineContainer;
    public Vector3[] initialPositions;
    public bool isStatic;
    [Range(0f, 1f)]
    public float StopPoint;
    public ShootingPattern shootingPattern;
    public float offsetBetweenEnemies;

}
[Serializable]
public class DynamicMovementData
{
    public SplineContainer splineContainer;
    public float offsetBetweenEnemies;
    public ShootingData shootingData;
    public float StopPoint;
}

[Serializable]
public class ShootingData
{
   public  ShootingPattern shootingPattern;
}

[Serializable]



public class StaticMovementData
{
    public Vector3 initialPosition;
    public ShootingData shootingData;
}
[Serializable]
public class WaveData
{
    public int enemyCount;
    public GameObject enemyPrefab;
    public Vector3[] initialPositions;
    public MovementData movementData;
    public float TimeBeforeNextWave;

}
