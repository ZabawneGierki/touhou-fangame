using DG.Tweening;
using UnityEngine;
 

public class ExplosionEffect : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        spriteRenderer.DOFade(0f, 0.2f).OnComplete(() => Destroy(gameObject));
        spriteRenderer.transform.DOScale(Vector3.one * 5f, 0.2f);
    }

     
}
