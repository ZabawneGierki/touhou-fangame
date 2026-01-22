using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{

    [SerializeField] private Button firstButton;

    private void OnEnable()
    {
        firstButton.Select();
    }

}
