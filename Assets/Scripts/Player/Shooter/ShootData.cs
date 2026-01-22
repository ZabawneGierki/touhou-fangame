
using UnityEngine;

[CreateAssetMenu(fileName = "ShootData", menuName = "Scriptable Objects/ShootData")]
public abstract class ShootData : ScriptableObject
{
    protected Transform shootingPoint;


    public abstract void StartShooting(GameObject player);
    public abstract void StopShooting(GameObject player);
    public void SetUpShootingPoint(Transform shootingPoint) { this.shootingPoint = shootingPoint; }

}
