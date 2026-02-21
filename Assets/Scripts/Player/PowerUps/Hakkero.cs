
using UnityEngine;


public class Hakkero : MonoBehaviour
{
    private void Update()
    {
        Debug.Log("Location of " + this.gameObject.name + ":" + transform.position);
    }

}