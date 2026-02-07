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

    [SerializeField] private HakkeroData[] helpers;
    public override void PowerUp(Transform t, int level)
    {
        Vector3 offset = new(helpers[level].positionOffset.x, helpers[level].positionOffset.y, 0f);
        GameObject helper = Object.Instantiate(helpers[level].helper.gameObject, t.position + offset, Quaternion.identity);
        helper.transform.parent = t;
    }
}
