using UnityEngine;



public class Hakkero : MonoBehaviour
{
    LineRenderer lineRenderer;
    bool isLeft;




    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        isLeft = transform.localScale.x < 0;
    }





}