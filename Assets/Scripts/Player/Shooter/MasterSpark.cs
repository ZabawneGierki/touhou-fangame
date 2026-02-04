using UnityEngine;

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/Shooters/MasterSpark")]
public class MasterSpark : ShootData
{
    
    public override void StartShooting(GameObject player)
    {
        Debug.Log("Master Spark Activated");
    }

    public override void StopShooting(GameObject player)
    {
       Debug.Log("Master Spark Deactivated");
    }
}
