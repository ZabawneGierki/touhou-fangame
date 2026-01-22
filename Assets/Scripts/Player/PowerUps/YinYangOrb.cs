using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class YinYangOrb : MonoBehaviour
{

     
    [SerializeField] HelperData helperData;
    [SerializeField] GameObject helperBulletPrefab;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float fireRate = 0.09f;


    [Header("Settings")]
    public float updateInterval = 0.1f; // How often to find a new target
    public float horizontalSpread = 0.6f; // Controls left-right spread (0 to 1)

    private Coroutine shootingCoroutine;
    private float nextFireTime = 0f;
    private bool isFocusing = false;    
    private Vector2 shootingDirection = Vector2.up;


    void Start()
    {

         
        StartCoroutine(PingPongShootDir());
         
        // if shoot button is held down at start, begin shooting
        if (InputManager.Instance.ShootAction.action.IsPressed())
        {
            StartShooting();
        }

    }

    private void OnEnable()
    {
        InputManager.Instance.ShootAction.action.performed += OnShootPerformed;
        InputManager.Instance.ShootAction.action.canceled += OnShootCanceled;

        InputManager.Instance.FocusAction.action.performed +=  OnFocusPerformed;
        InputManager.Instance.FocusAction.action.canceled += OnFocusCanceled;



    }

    private void OnFocusCanceled(InputAction.CallbackContext context)
    {
       isFocusing = false;
    }

    private void OnFocusPerformed(InputAction.CallbackContext context)
    {
        isFocusing = true;
    }

    private void OnDisable()
    {
        InputManager.Instance.ShootAction.action.performed -= OnShootPerformed;
        InputManager.Instance.ShootAction.action.canceled -= OnShootCanceled;
        InputManager.Instance.FocusAction.action.performed -= OnFocusPerformed;
        InputManager.Instance.FocusAction.action.canceled -= OnFocusCanceled;
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
        if (shootingCoroutine == null)
        {
            nextFireTime = Time.time;
            shootingCoroutine = StartCoroutine(ShootCoroutine());
        }
    }

    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (Time.time >= nextFireTime)
            {
                GameObject bullet = Instantiate(helperBulletPrefab, shootingPoint.position, Quaternion.identity);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

                if (bulletRb != null)
                {
                    bulletRb.linearVelocity = shootingDirection * 20f;
                }
                else
                {
                    Debug.LogWarning("Bullet prefab missing Rigidbody2D component");
                }

                nextFireTime = Time.time + fireRate;
            }
            yield return null;
        }
    }

     
    private IEnumerator PingPongShootDir()
    {
        float Xpos = -horizontalSpread;
        float speed = 3f;

        while (true)
        {
            if(isFocusing)
            {
                Xpos = 0f;
                shootingDirection = Vector2.up;
                yield return new WaitForSeconds(updateInterval);
                continue; 
            }

            Xpos = Mathf.PingPong(Time.time * horizontalSpread * speed, horizontalSpread * 2) - horizontalSpread;
            shootingDirection = new Vector2(Xpos, 1).normalized;
            yield return new WaitForSeconds(updateInterval);
             
        }
    }

 



}
