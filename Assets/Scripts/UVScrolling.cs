using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class UVScroller : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0.2f, 0f);

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "Enemies";
        renderer.sortingOrder = 10;
        mat = GetComponent<Renderer>().material; // Instance!
    }

    void Update()
    {
        offset += scrollSpeed * Time.deltaTime;
        mat.SetTextureOffset("_BaseMap", offset); // URP

         
    }
}
