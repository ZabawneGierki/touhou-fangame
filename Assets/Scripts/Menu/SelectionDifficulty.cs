using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectionDifficulty : MonoBehaviour
{
    [SerializeField] private Button easyButton,normalButton, hardButton, lunaticButton;


    private void OnEnable()
    {
        // select button based on current difficulty
        switch(PlayerData.gameDifficulty)
        {
            case Difficulty.Easy:
                easyButton.Select();
                break;
            case Difficulty.Normal:
                normalButton.Select();
                break;
            case Difficulty.Hard:
                hardButton.Select();
                break;
            case Difficulty.Lunatic:
                lunaticButton.Select();
                break;
        }


         
    }

    private void SelectDifficulty(Difficulty difficulty)
    {
        PlayerData.gameDifficulty = difficulty;
        


    }

    public void SelectEasy()
    {
        SelectDifficulty(Difficulty.Easy);
    }
    public void SelectMedium()
    {
        SelectDifficulty(Difficulty.Normal);
    }
    public void SelectHard()
    {
        SelectDifficulty(Difficulty.Hard);
    }
    public void SelectLunatic()
    {
        SelectDifficulty(Difficulty.Lunatic);
    }


}
