using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{

    [SerializeField] private Button firstButton;
    [SerializeField] private Slider firstSlider;

    private void OnEnable()
    {
        if (firstSlider != null)
        {
            firstSlider.Select();
            return;
        }
        else if (firstButton != null)
        {
            firstButton.Select();
        }


    }

}
