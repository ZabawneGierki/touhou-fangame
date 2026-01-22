using System.Collections;
using UnityEngine;

public abstract class ShootingPattern : ScriptableObject
{
    public float fireRate = 0.2f;
    public abstract IEnumerator ExecutePattern(Transform shooter);
}

