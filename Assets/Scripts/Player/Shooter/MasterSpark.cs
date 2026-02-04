using UnityEngine;

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    [Header("Master Spark")]
    public LineRenderer line;
    public float maxDistance = 20f;
    public float damagePerSecond = 25f;
    public LayerMask hitMask;
    public override void StartShooting(GameObject player)
    {
        Debug.Log("Master Spark Activated");
    }

    public override void StopShooting(GameObject player)
    {
       Debug.Log("Master Spark Deactivated");
    }
}
