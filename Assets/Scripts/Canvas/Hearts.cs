using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{

    [SerializeField] Sprite fullHeart, emptyHeart;

    [SerializeField] Image[] hearts;

    private int startingHearts = 3;


    public static Hearts instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize hearts with 3 full hearts
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < startingHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

    }

    public void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHearts)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
