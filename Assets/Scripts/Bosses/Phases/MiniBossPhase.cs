using System.Collections;
using UnityEngine;

 
public abstract class MiniBossPhase : ScriptableObject
{
    public abstract IEnumerator ExecutePhase(MiniBoss miniBoss);
    
}
