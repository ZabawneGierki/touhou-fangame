using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{



    [Header("Shooting")]
    [SerializeField] Transform shootingPoint;
    [SerializeField] ShootData currentShootData;




    private void OnEnable()
    {
        InputManager.Instance.ShootAction.action.performed += OnShootPerformed;
        InputManager.Instance.ShootAction.action.canceled += OnShootCanceled;

        currentShootData.SetUpShootingPoint(shootingPoint);

    }

    private void OnDisable()
    {
        InputManager.Instance.ShootAction.action.performed -= OnShootPerformed;
        InputManager.Instance.ShootAction.action.canceled -= OnShootCanceled;

    }

    private void OnShootPerformed(InputAction.CallbackContext ctx)
    {
        StartShooting();
    }

    private void OnShootCanceled(InputAction.CallbackContext ctx)
    {
        StopShooting();
    }

    private void StartShooting()
    {


        currentShootData.StartShooting(this.gameObject);
    }

    private void StopShooting()
    {

        currentShootData.StopShooting(this.gameObject);
    }



}
