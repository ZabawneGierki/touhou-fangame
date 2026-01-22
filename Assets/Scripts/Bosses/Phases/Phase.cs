using System.Collections;
using UnityEngine;

 
public abstract class Phase : ScriptableObject
{
    public abstract IEnumerator ExecutePhase(Boss boss);
     

}
