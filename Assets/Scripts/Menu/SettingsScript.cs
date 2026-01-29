using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] private Dropdown languageDropdown;
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


    }
}
