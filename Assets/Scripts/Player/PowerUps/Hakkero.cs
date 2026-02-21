
using UnityEngine;


public class Hakkero : MonoBehaviour
{
    LineRenderer lineRenderer;
    private void Update()
    {
        Debug.Log("Location of " + this.gameObject.name + ":" + transform.position);
    }

}