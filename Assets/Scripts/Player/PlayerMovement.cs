using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Multiline]
    [Header("Input Actions")]
    [SerializeField]
    InputActionReference MoveAction;
    [SerializeField]
    InputActionReference ShootAction;
    [SerializeField]
    InputActionReference BombAction;
    [SerializeField]
    InputActionReference FocusAction;

    Rigidbody2D rb;

    public float maxSpeed = 5f;
    private float currentSpeed;
    private Vector2 moveInput;

    [Header("The hitbox visible on focus")]
    [SerializeField] SpriteRenderer FocusPoint;


 

    private void Awake()
    {
        
        InputManager.Instance.FocusAction.action.performed += OnFocusPerformed;
        InputManager.Instance.FocusAction.action.canceled += OnFocusCanceled;

        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        // Just enable/disable the player input when this GameObject becomes active
        InputManager.Instance.SwitchToPlayerInput();
        currentSpeed = maxSpeed;
        FocusPoint.enabled = false;
    }

    private void OnDisable()
    {
        // Disable player input when this GameObject becomes inactive
        InputManager.Instance.SwitchToUIInput();
        FocusPoint.enabled = true;
         
    }

    private void OnDestroy()
    {
        // Unsubscribe from callbacks when destroyed
        InputManager.Instance.FocusAction.action.performed -= OnFocusPerformed;
        InputManager.Instance.FocusAction.action.canceled -= OnFocusCanceled;

        
    }

    private void OnFocusPerformed(InputAction.CallbackContext ctx) => FocusPoint.enabled = true;
    private void OnFocusCanceled(InputAction.CallbackContext ctx) => FocusPoint.enabled = false;

 

    

     

    
     

    void Update()
    {
        moveInput = InputManager.Instance.MoveAction.action.ReadValue<Vector2>();

        if (InputManager.Instance.FocusAction.action.IsPressed())
        {
            currentSpeed = maxSpeed * 0.5f;
        }
        else
        {
            currentSpeed = maxSpeed;
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = moveInput.normalized * currentSpeed;
        rb.linearVelocity = movement;
    }
}
