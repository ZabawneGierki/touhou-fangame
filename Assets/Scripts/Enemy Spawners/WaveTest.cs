using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveTest", menuName = "Scriptable Objects/WaveTest")]
public class WaveTest : Wave 
{
     

    public override IEnumerator PlayWave()
    {
          Debug.Log("Starting Wave Test: " + waveName);
        // Simulate some wave activity
        yield return new WaitForSeconds(3f); // Simulate a 3-second wave
        Debug.Log("Wave Test " + waveName + " finished.");
    }
}
