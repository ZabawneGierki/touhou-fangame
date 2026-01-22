public enum PlayerName
{
    Reimu = 0,
    Marisa = 1,
    Reisen = 2,
}

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Lunatic
}
public static class PlayerData
{
    public static PlayerName selectedPlayer = PlayerName.Reimu;
    public static int playerLives = 3;
    public static Difficulty gameDifficulty = Difficulty.Normal;
    public static int continuesAvailable = 3;
    public static bool hasUsedContinue = false;

    public static int currentChapter = 1;



    public static void SetSelectedPlayer(PlayerName player)
    {
        selectedPlayer = player;
    }

    public static float GetDifficultyMultiplier()
    {
        switch (gameDifficulty)
        {
            case Difficulty.Easy:
                return 0.7f;
            case Difficulty.Normal:
                return 1.0f;
            case Difficulty.Hard:
                return 1.5f;
            case Difficulty.Lunatic:
                return 2.0f;
            default:
                return 1.0f;
        }
    }
}
