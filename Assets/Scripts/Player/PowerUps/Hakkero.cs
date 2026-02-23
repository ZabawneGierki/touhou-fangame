
using UnityEngine;
using UnityEngine.ParticleSystemJobs;


public class Hakkero : MonoBehaviour
{
    LineRenderer lineRenderer;
    bool isLeft;
    GameObject particleEffect;
    


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        isLeft = transform.localScale.x < 0;
    }





}