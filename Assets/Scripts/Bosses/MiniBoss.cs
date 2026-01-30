using UnityEngine;

public class MiniBoss : MonoBehaviour
{


    [SerializeField] private MiniBossPhase phase; // a single phase for the miniboss
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
        StartCoroutine(phase.ExecutePhase(this));

    }

  
}
