using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{

    [SerializeField] Sprite fullStar, emptyStar;

    [SerializeField] Image[] stars;

    private int startingStars = 3;


    public static Stars instance;


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
        //initialize stars with 3 full stars
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < startingStars)
            {
                stars[i].sprite = fullStar;
            }
            else
            {
                stars[i].sprite = emptyStar;
            }
        }
    }

    public void UpdateHearts(int currentHearts)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (i < currentHearts)
            {
                stars[i].sprite = fullStar;
            }
            else
            {
                stars[i].sprite = emptyStar;
            }
        }
    }

}
