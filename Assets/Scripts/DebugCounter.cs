using UnityEngine;
using TMPro;
using System.Collections;

public class DebugCounter : MonoBehaviour
{
    float counter = 0f; 
    TextMeshProUGUI textComponent;

    public static DebugCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetCounter()
    {
        counter = 0f;
        textComponent.text = counter.ToString("F2");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();

        StartCoroutine(UpdateCounter());
    }

     private IEnumerator UpdateCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            counter += 0.1f;
            textComponent.text = counter.ToString("F2");
        }
            
    }
}
