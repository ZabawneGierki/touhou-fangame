using System.Collections;
using UnityEngine;
using UnityEngine.Events;
 

public class PlayerHealth : MonoBehaviour
{
     public UnityEvent onPlayerDeath;

    public bool isInvincible = false;


    [SerializeField] GameObject pichunEffect;



    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        onPlayerDeath.AddListener(() => {
            StartCoroutine(GameManager.instance.OnPlayerDied());
        });
    }
    public void StartPichun()
    {
        StartCoroutine(Pichun());
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.CompareTag("Enemy") || collision.CompareTag("Bullet"))
        {
            if (isInvincible)
                return;
            StartPichun();
            onPlayerDeath.Invoke();
            SoundManager.Instance.PlaySoundByName(ClipName.Pichun); 
             

        }
    }



    private IEnumerator Pichun()
    {
        isInvincible = true;
        GameObject effect = Instantiate(pichunEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        isInvincible = false;
    }


     
 
}
