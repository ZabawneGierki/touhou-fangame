using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MasterSpark", menuName = "Scriptable Objects/MasterSpark")]
public class MasterSpark : ShootData
{
    [Header("Laser Settings")]
    [SerializeField] private float maxLaserLength = 20f;
    [SerializeField] private float damagePerSecond = 50f;
    [SerializeField] private float laserWidth = 0.02f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visuals")]
    [SerializeField] private Color laserColor = new Color(1f, 0.85f, 1f, 1f);
    [SerializeField] private Color laserCoreColor = Color.white;

    [Header("Effects")]
    [SerializeField] private GameObject impactEffectPrefab;

    // Runtime state — stored per-player via a small MonoBehaviour shim
    // so multiple players (or re-entrant calls) don't bleed state between each other.
    private MasterSparkController GetOrCreateController(GameObject player)
    {
        var ctrl = player.GetComponent<MasterSparkController>();
        if (ctrl == null)
            ctrl = player.AddComponent<MasterSparkController>();
        return ctrl;
    }

    public override void StartShooting(GameObject player)
    {
        var ctrl = GetOrCreateController(player);
        ctrl.StartLaser(this, shootingPoint);
    }

    public override void StopShooting(GameObject player)
    {
        var ctrl = player.GetComponent<MasterSparkController>();
        if (ctrl != null)
            ctrl.StopLaser();
    }

    // -------------------------------------------------------------------------
    // Internal helper — exposes configuration to the controller without making
    // fields public on the ScriptableObject itself.
    // -------------------------------------------------------------------------
    internal void Configure(
        out float outMaxLength,
        out float outDamagePerSecond,
        out float outLaserWidth,
        out LayerMask outEnemyLayer,
        out Color outLaserColor,
        out Color outCoreColor,
        out GameObject outImpactPrefab)
    {
        outMaxLength = maxLaserLength;
        outDamagePerSecond = damagePerSecond;
        outLaserWidth = laserWidth;
        outEnemyLayer = enemyLayer;
        outLaserColor = laserColor;
        outCoreColor = laserCoreColor;
        outImpactPrefab = impactEffectPrefab;
    }
}