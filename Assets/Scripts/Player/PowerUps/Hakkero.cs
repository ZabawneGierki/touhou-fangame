using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hakkero : MonoBehaviour
{
    
     
     
    [SerializeField] Color[] laserColors;
    [SerializeField] Material laserMaterial;



    private void Start()
    {
        StartCoroutine(ChangeLaserColorRoutine());
    }



    private IEnumerator ChangeLaserColorRoutine()
    {
        // change laser color every 0.5 seconds gradually
        int colorIndex = 0;
        while (true)
        {
            Color startColor = laserColors[colorIndex];
            Color endColor = laserColors[(colorIndex + 1) % laserColors.Length];
            float transitionDuration = 0.5f;
            float elapsed = 0f;
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / transitionDuration);
                Color currentColor = Color.Lerp(startColor, endColor, t);
                laserMaterial.color = currentColor;
                yield return null;
            }
            colorIndex = (colorIndex + 1) % laserColors.Length;

        }
    }
         

}