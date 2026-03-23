// =============================================================================
// MasterSparkController — lives on the player GameObject at runtime.
// Handles the LineRenderer, raycasting, impact particles, and damage ticking.
// =============================================================================
using System.Collections;
using UnityEngine;

public class MasterSparkController : MonoBehaviour
{
    // ---- cached references --------------------------------------------------
    private LineRenderer _lineRenderer;
    private Transform _shootingPoint;
    private MasterSpark _data;

    // ---- configuration (filled from ScriptableObject) -----------------------
    private float _maxLaserLength;
    private float _damagePerSecond;
    private LayerMask _enemyLayer;
    private GameObject _impactEffectPrefab;

    // ---- runtime state ------------------------------------------------------
    private bool _isShooting;
    private GameObject _activeImpactEffect;
    private EnemyHealth _currentTarget;
    private Coroutine _damageCoroutine;

    // =========================================================================
    // Public API (called by MasterSpark ScriptableObject)
    // =========================================================================
    public void StartLaser(MasterSpark data, Transform shootingPoint)
    {
        _data = data;
        _shootingPoint = shootingPoint;

        // Unpack config
        data.Configure(
            out _maxLaserLength,
            out _damagePerSecond,
            out float laserWidth,
            out _enemyLayer,
            out Color laserColor,
            out Color coreColor,
            out _impactEffectPrefab);

        // Obtain (or reuse) the LineRenderer on the player
        _lineRenderer = GetComponent<LineRenderer>();
        if (_lineRenderer == null)
            _lineRenderer = gameObject.AddComponent<LineRenderer>();

        SetupLineRenderer(_lineRenderer, laserWidth, laserColor, coreColor);

        _isShooting = true;
        _lineRenderer.enabled = true;

        _damageCoroutine = StartCoroutine(DamageLoop());
    }

    public void StopLaser()
    {
        _isShooting = false;

        if (_lineRenderer != null)
            _lineRenderer.enabled = false;

        if (_damageCoroutine != null)
        {
            StopCoroutine(_damageCoroutine);
            _damageCoroutine = null;
        }

        ClearImpactEffect();
        _currentTarget = null;
    }

    // =========================================================================
    // Unity — Update (visual refresh every frame)
    // =========================================================================
    private void Update()
    {
        if (!_isShooting || _shootingPoint == null || _lineRenderer == null)
            return;

        UpdateLaser();
    }

    // =========================================================================
    // Laser logic
    // =========================================================================
    private void UpdateLaser()
    {
        Vector2 origin = _shootingPoint.position;
        Vector2 direction = Vector2.up;

        // Cast against enemy trigger colliders
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, _maxLaserLength, _enemyLayer);

        Vector2 endPoint;
        EnemyHealth hitEnemy = null;

        if (hit.collider != null)
        {
            endPoint = hit.point;
            hitEnemy = hit.collider.GetComponent<EnemyHealth>();

            // Place / move impact effect at the hit point
            PlaceImpactEffect(endPoint);
        }
        else
        {
            endPoint = origin + direction * _maxLaserLength;
            ClearImpactEffect();
        }

        // Track enemy target for the damage coroutine
        _currentTarget = hitEnemy;

        // Update line positions (world space)
        _lineRenderer.SetPosition(0, origin);
        _lineRenderer.SetPosition(1, endPoint);
    }

    // =========================================================================
    // Damage coroutine — ticks damage each second while a target is present
    // =========================================================================
    private IEnumerator DamageLoop()
    {
        while (_isShooting)
        {
            yield return new WaitForSeconds(1f);

            if (_currentTarget != null)
                _currentTarget.TakeDamage(_damagePerSecond);
        }
    }

    // =========================================================================
    // Impact effect helpers
    // =========================================================================
    private void PlaceImpactEffect(Vector2 position)
    {
        if (_impactEffectPrefab == null)
            return;

        if (_activeImpactEffect == null)
            _activeImpactEffect = Instantiate(_impactEffectPrefab);

        _activeImpactEffect.transform.position = position;

        // Make sure the particle system is playing
        var ps = _activeImpactEffect.GetComponent<ParticleSystem>();
        if (ps != null && !ps.isPlaying)
            ps.Play();
    }

    private void ClearImpactEffect()
    {
        if (_activeImpactEffect == null)
            return;

        var ps = _activeImpactEffect.GetComponent<ParticleSystem>();
        if (ps != null)
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Destroy(_activeImpactEffect);
        _activeImpactEffect = null;
    }

    // =========================================================================
    // LineRenderer setup (runs once when the laser starts)
    // =========================================================================
    private static void SetupLineRenderer(
        LineRenderer lr,
        float width,
        Color outerColor,
        Color coreColor)
    {
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        // Taper very slightly toward the tip for a beam look
        lr.startWidth = width;
        lr.endWidth = width * 0.6f;

        // Simple two-stop gradient: bright core → colored glow
        var gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(coreColor, 0f), new GradientColorKey(outerColor, 1f) },
            new[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0.8f, 1f) }
        );
        lr.colorGradient = gradient;

         

        lr.sortingOrder = 10; // render in front of most sprites
    }

     
    private void OnDestroy()
    {
        StopLaser();
    }
}