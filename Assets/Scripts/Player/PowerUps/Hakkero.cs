using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hakkero : MonoBehaviour
{
    [SerializeField] private LineRenderer laserLineRenderer;
    [SerializeField] private Transform laserOrigin; // Position where laser starts
    [SerializeField] private float laserSpeed = 50f;
    [SerializeField] private float maxLaserDistance = 100f;
     
     
    [SerializeField] Color[] laserColors;
    [SerializeField] Material laserMaterial;

    private Coroutine laserCoroutine;




    private IEnumerator Start()
    {
        // Delay subscription to ensure InputManager is initialized
        yield return new WaitForSeconds(1f);
        InputManager.Instance.ShootAction.action.performed += OnStartShooting;
        InputManager.Instance.ShootAction.action.canceled += OnStopShooting;
         
        // Ensure line renderer is disabled at start
        if (laserLineRenderer != null)
        {
            laserLineRenderer.enabled = false;
            laserLineRenderer.positionCount = 2;
            // Start color changing coroutine
            StartCoroutine(ChangeLaserColorRoutine());
        }
    }

    private void OnDisable()
    {
       
        InputManager.Instance.ShootAction.action.performed -= OnStartShooting;
        InputManager.Instance.ShootAction.action.canceled -= OnStopShooting;
        StopLaser();
    }

    private void OnStartShooting(InputAction.CallbackContext context)
    {
        StartLaser();
    }

    private void OnStopShooting(InputAction.CallbackContext context)
    {
        StopLaser();
    }

    private void StartLaser()
    {
        if (laserLineRenderer == null || laserOrigin == null)
        {
            Debug.LogWarning("Hakkero: Missing LineRenderer or LaserOrigin reference.");
            return;
        }

        if (laserCoroutine == null)
            laserCoroutine = StartCoroutine(LaserCoroutine());
    }

    private void StopLaser()
    {
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
            laserCoroutine = null;
        }

        if (laserLineRenderer != null)
            laserLineRenderer.enabled = false;
    }

    private IEnumerator LaserCoroutine()
    {
        // Configure visual properties
        laserLineRenderer.enabled = true;
         
        
        laserLineRenderer.positionCount = 2;

        float currentLength = 0f;

        while (true)
        {
            // grow laser length up to maximum
            currentLength += laserSpeed * Time.deltaTime;
            if (currentLength > maxLaserDistance)
                currentLength = maxLaserDistance;

            Vector3 originPos = laserOrigin.position;
            Vector3 upDir = Vector3.up;

            // Raycast in 2D (project uses 2D physics in this project)
            RaycastHit2D hit = Physics2D.Raycast(originPos, upDir, currentLength);
            Vector3 endPos;

            if (hit.collider != null)
            {
                endPos = (Vector3)hit.point;

                // Stop the laser when it hits an object tagged as Enemy
                if (hit.collider.CompareTag("Enemy"))
                {
                    // update visual one last time
                    laserLineRenderer.SetPosition(0, originPos);
                    laserLineRenderer.SetPosition(1, endPos);

                    // TODO: later add damage call here (use laserDamage)
                    StopLaser();
                    yield break;
                }
            }
            else
            {
                // No hit within currentLength: set end point to currentLength
                endPos = originPos + upDir * currentLength;
            }

            laserLineRenderer.SetPosition(0, originPos);
            laserLineRenderer.SetPosition(1, endPos);

            // If we've reached max length and no enemy was hit, keep laser until user stops it
            yield return null;
        }
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