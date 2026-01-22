using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{


    [SerializeField] private LocalizedString continuesAvailableText;

    [SerializeField] TextMeshProUGUI continuesTextBox;

    [SerializeField] private Button continueButton, backToMenuButton;

    public void RestartGame()
    {
        if (PlayerData.continuesAvailable > 0)
        {
            PlayerData.continuesAvailable--;
            PlayerData.playerLives = 3; // Reset player lives on continue
        }
        else
        {
            PlayerData.playerLives = 3; // Reset player lives if no continues left
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void Resign()
    {

        PlayerData.continuesAvailable = 3; // Reset continues for next game
        PlayerData.playerLives = 3; // Reset player lives for next game
        PlayerData.hasUsedContinue = false; // Reset continue usage flag
        SceneManager.LoadScene("Menu");
    }


    private void OnEnable()
    {
        continuesTextBox.text = continuesAvailableText.GetLocalizedString(new object[] { PlayerData.continuesAvailable });
        InputManager.Instance.SwitchToUIInput();
        Time.timeScale = 0f; // Pause the game
        continueButton.Select();
    }

    private void OnDisable()
    {
        InputManager.Instance.SwitchToPlayerInput();
        Time.timeScale = 1f; // Resume the game
    }
}
