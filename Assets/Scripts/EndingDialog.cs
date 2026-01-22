using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Ink.Runtime;

public class EndingDialog : MonoBehaviour
{
    [SerializeField]  Story[] endingText;

    [SerializeField] TextMeshProUGUI endingTextBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // print current localization index
                Debug.Log("Current Locale: " +   LocalizationSettings.SelectedLocale.Identifier.Code);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
