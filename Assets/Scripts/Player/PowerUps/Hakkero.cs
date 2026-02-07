using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hakkero : MonoBehaviour
{
    
     
     
    [SerializeField] Color[] laserColors;
    [SerializeField] Material laserMaterial;



    
    [Header("Setup")]
    public ShootData laserData;
    public Transform shootingPoint;
    public float followSpeed = 10f;
    public Vector2 offsetFromPlayer; // Set this in Inspector (e.g., 1.5, 0)

    [Header("Aiming")]
    public float sideAngle = 30f;
    private float currentAngle;
    private bool isFacingRight;
    private bool isFocusing;

    private Transform playerTransform;
   

    void Start()
    {

        StartCoroutine(ChangeLaserColorRoutine());
        // 1. Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
           

            // 2. Determine side based on X position relative to player
            isFacingRight = transform.position.x > playerTransform.position.x;

            // 3. Link to the SO
            if (laserData != null) laserData.SetUpShootingPoint(shootingPoint);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Follow Logic: Stay at the side of the player
        Vector3 targetPos = playerTransform.position + new Vector3(
            isFacingRight ? offsetFromPlayer.x : -offsetFromPlayer.x,
            offsetFromPlayer.y,
            0);

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        // Rotation Logic: 0 if focusing, sideAngle if normal
        float targetAngle = isFocusing ? 0f : (isFacingRight ? -sideAngle : sideAngle);
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * 15f);

        // Apply rotation to the orb or just the shooting point
        shootingPoint.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }

    // --- Input System Callbacks ---

    public void OnFire(InputValue value)
    {
        if (value.isPressed)
            laserData.StartShooting(gameObject); //
        else
            laserData.StopShooting(gameObject); //
    }

    public void OnFocus(InputValue value)
    {
        isFocusing = value.isPressed;
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