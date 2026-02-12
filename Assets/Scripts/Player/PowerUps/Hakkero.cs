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

    public Direction currentDirection; // Default direction, can be set by the player or other logic


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
           if(transform.position.x < player.transform.position.x)
            {
                currentDirection = Direction.Right;
            }
            else
            {
                currentDirection = Direction.Left;
            }
        }
        else
        {
            Debug.LogWarning("Player not found.");
             
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