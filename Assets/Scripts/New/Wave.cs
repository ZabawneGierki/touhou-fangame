using System.Collections;
using UnityEngine;


public abstract class Wave  : ScriptableObject
{
    public string waveName = "New Wave";
    public float timeAfterThisWave = 2f;
    public IEnumerator Play()
    {
        yield return null;
        yield return PlayWave();
        yield return new WaitForSeconds(timeAfterThisWave);
        Debug.Log("Wave " + waveName + " completed.");
    }

    public abstract IEnumerator PlayWave();
     
}
