using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hakkero : MonoBehaviour
{
    [SerializeField] Color[] laserColors;
    [SerializeField] Material laserMaterial;
    
    [SerializeField] Transform shootingPoint;

    LineRenderer lineRenderer;

    private Direction currentDirection; // Default direction, can be set by the player or other logic


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            Debug.Log("Player found!");
        }
        else
        {
            Debug.LogWarning("Player not found.");
             
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

   

    private void OnEnable()
    {
        InputManager.Instance.ShootAction.action.performed += OnStartShooting;
        InputManager.Instance.ShootAction.action.canceled += OnStopShooting;

        InputManager.Instance.FocusAction.action.performed += OnStartFocus;
        InputManager.Instance.FocusAction.action.canceled += OnStopFocus;
    }

    private void OnDisable()
    {
        InputManager.Instance.ShootAction.action.performed -= OnStartShooting;
        InputManager.Instance.ShootAction.action.canceled -= OnStopShooting;
        InputManager.Instance.FocusAction.action.performed -= OnStartFocus;
        InputManager.Instance.FocusAction.action.canceled -= OnStopFocus;
    }

    private void OnStopFocus(InputAction.CallbackContext context)
    {
        // No focus behavior implemented for helper instances currently.
    }

    private void OnStartFocus(InputAction.CallbackContext context)
    {
        // No focus behavior implemented for helper instances currently.
    }

    private void OnStopShooting(InputAction.CallbackContext context)
    {
        
    }

    private void OnStartShooting(InputAction.CallbackContext context)
    {
        
    }
}