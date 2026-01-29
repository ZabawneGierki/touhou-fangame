using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;
    public void OnChangeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void OnChangeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void OnChangeLanguage()
    {
        int selectedLanguageIndex = languageDropdown.value;
        PlayerPrefs.SetInt("Language", selectedLanguageIndex);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];


    }
}
