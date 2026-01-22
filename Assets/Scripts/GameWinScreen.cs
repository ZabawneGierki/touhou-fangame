using UnityEngine;

public class GameWinScreen : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f; // Pause the game
    }

    private void OnDisable()
    {
        Time.timeScale = 1f; // Resume the game
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
