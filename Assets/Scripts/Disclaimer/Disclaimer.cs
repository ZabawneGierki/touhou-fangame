using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Disclaimer : MonoBehaviour
{

    [SerializeField] private InputActionReference acceptAction;

    private void Start()
    {
        Debug.Log("Disclaimer screen loaded. Waiting for user acceptance.");
    }

    private void OnEnable()
    {
        if (acceptAction == null || acceptAction.action == null)
        {
            Debug.LogWarning("Disclaimer: acceptAction reference is not set.");
            return;
        }

        // Subscribe and ensure the action is enabled so callbacks run
        acceptAction.action.performed += OnAcceptPerformed;
        acceptAction.action.Enable();

        Debug.Log($"Disclaimer: subscribed to '{acceptAction.action.name}' (enabled: {acceptAction.action.enabled})");
    }

    private void OnAcceptPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Disclaimer accepted, proceeding to main menu.");
        SceneManager.LoadScene("Menu");
    }

    private void OnDisable()
    {
        if (acceptAction == null || acceptAction.action == null)
            return;

        // Unsubscribe then disable to clean up
        acceptAction.action.performed -= OnAcceptPerformed;
        acceptAction.action.Disable();
    }
}
