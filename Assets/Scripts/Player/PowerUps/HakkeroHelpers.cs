using UnityEngine;

[CreateAssetMenu(fileName = "HakkeroHelpers", menuName = "Scriptable Objects/PowerUps/HakkeroHelpers")]
public class HakkeroHelpers : PowerUpData
{
    [System.Serializable]
    private struct HakkeroData
    {
        public string name;
        public Hakkero helper;
        public Vector2 positionOffset;
    }
    public override void PowerUp(Transform transform, int level)
    {
        throw new System.NotImplementedException();
    }
}
