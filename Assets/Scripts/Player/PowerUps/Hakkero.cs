
using UnityEngine;


public class Hakkero : MonoBehaviour
{
    LineRenderer lineRenderer;
    bool isLeft;
    


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        isLeft = transform.localScale.x < 0;
    }





}