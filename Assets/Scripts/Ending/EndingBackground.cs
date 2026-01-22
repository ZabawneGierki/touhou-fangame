using UnityEngine;
using UnityEngine.UI;

public class EndingBackground : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    public void SetBackground(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }
}
