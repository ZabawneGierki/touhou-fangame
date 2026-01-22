using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyMovement : MonoBehaviour
{
    public SplineAnimate splineAnimate;
    public DynamicMovementData dynamicMovementData;
    public StaticMovementData staticMovementData;

    private SpriteRenderer spriteRenderer;
    private float appearDuration = 1.5f;

    public bool hasStopped = false;

    // for static enemies
    public void SetDataAndStart(StaticMovementData data)
    {
        staticMovementData = data;
        Appear();
        StartCoroutine(StartShootingStatic());
    }

    // for moving enemies
    public void SetDataAndStart(DynamicMovementData data, int EnemyIndex)
    {
        dynamicMovementData = data;

        // Calculate offset as a normalized value (0-1) along the spline
        // offsetBetweenEnemies should be a small normalized value like 0.05 or 0.1
        float normalizedOffset = EnemyIndex * dynamicMovementData.offsetBetweenEnemies;
        splineAnimate.StartOffset = normalizedOffset;

        splineAnimate.Container = dynamicMovementData.splineContainer;
        splineAnimate.Loop = SplineAnimate.LoopMode.Once;

        FollowPath();

        StartCoroutine(StartShootingDynamic());
    }

    public void StopMovement(bool stop)
    {
        if (stop)
        {
            splineAnimate.Pause();
        }
        else
        {
            splineAnimate.Play();
        }
    }

    private void FollowPath()
    {
        splineAnimate.Play();
    }

    private void Appear()
    {
        transform.localScale = Vector3.zero;
        transform.position = staticMovementData.initialPosition;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

        transform.DOScale(Vector3.one, appearDuration).SetEase(Ease.InOutCirc);
        spriteRenderer.DOFade(1f, appearDuration).SetEase(Ease.InOutCirc);
    }

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private IEnumerator StartShootingStatic()
    {
        // Wait for appear animation to finish
        yield return new WaitForSeconds(appearDuration);

        // Ensure we have the pattern to execute and use this enemy's transform (no spline for static)
        if (staticMovementData?.shootingData?.shootingPattern != null)
            yield return staticMovementData.shootingData.shootingPattern.ExecutePattern(transform);
    }

    private IEnumerator StartShootingDynamic()
    {
        // Wait until splineAnimate and dynamicMovementData are assigned (prevents race)
        yield return new WaitUntil(() => splineAnimate != null && dynamicMovementData != null);

        // wait until spline reaches stop point (StopPoint is normalized)
        yield return new WaitUntil(() => splineAnimate.NormalizedTime >= dynamicMovementData.StopPoint);

        // pause cleanly at current position
        StopMovement(true);

        // execute shooting pattern while paused (use spline transform for moving enemies)
        if (dynamicMovementData.shootingData?.shootingPattern != null)
            yield return dynamicMovementData.shootingData.shootingPattern.ExecutePattern(splineAnimate.transform);

        // resume movement
        StopMovement(false);

        // wait until end of path
        yield return new WaitUntil(() => splineAnimate.NormalizedTime >= 1f);

        // reached end of path, destroy enemy
        Destroy(gameObject);
    }



    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}