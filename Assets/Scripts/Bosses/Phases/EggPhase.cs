using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "EggPhase", menuName = "Scriptable Objects/Phases/EggPhase")]
public class EggPhase : Phase
{
    public override IEnumerator ExecutePhase(Boss boss)
    {
        // Implement Egg-specific behavior here
        while (true)
        {
            Debug.Log("Egg Phase Looping Behavior");
            yield return null;
        }
         
    }
}
