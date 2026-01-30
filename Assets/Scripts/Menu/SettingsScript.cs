using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown languageDropdown;
    public void OnChangeMusicVolume(float volume)
    {

        SoundManager.Instance.SetMusicVolume(volume);
    }

    public void OnChangeSFXVolume(float volume)
    {

        SoundManager.Instance.SetSFXVolume(volume);
    }

    public void OnChangeLanguage()
    {
        int selectedLanguageIndex = languageDropdown.value;
        PlayerPrefs.SetInt("Language", selectedLanguageIndex);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[selectedLanguageIndex];


    }
}
