using UnityEngine;

[CreateAssetMenu(fileName = "YinYangOrbs", menuName = "Scriptable Objects/PowerUps/YinYangOrbs")]
public class YinYangOrbs : PowerUpData
{
    [System.Serializable]
    private struct OrbData
    {
        public string name;
        public YinYangOrb helper;
        public Vector2 positionOffset;
    }

    [SerializeField] private OrbData[] helpers;
    public override void PowerUp(Transform t, int level)
    {
        Vector3 offset = new Vector3(helpers[level].positionOffset.x, helpers[level].positionOffset.y, 0f);
        GameObject helper = Object.Instantiate(helpers[level].helper.gameObject, t.position + offset, Quaternion.identity);
        helper.transform.parent = t;
        
    }
}
