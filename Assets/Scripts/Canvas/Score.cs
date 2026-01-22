using TMPro;
using UnityEngine;
using UnityEngine.Localization;
public struct PointsByChapter
{

    public int points;
    public int graze;
}

public class Score : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HighScoreText;
    [SerializeField] private TextMeshProUGUI ScoreText;


    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI grazeText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI powerScore;



    [SerializeField] LocalizedString EasyText;
    [SerializeField] LocalizedString NormalText;
    [SerializeField] LocalizedString HardText;
    [SerializeField] LocalizedString LunaticText;


    private int powerMax = GameData.MAX_POWER;

    private int lives;
    private int spells;
    private int power;
    private int graze;
    private int currentScore = 0;
    private int highScore = 0;

    private PointsByChapter currentPoints;





    public static Score instance;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeScores();
        InitializeDifficultyText();

    }

    private void InitializeDifficultyText()
    {
        switch (PlayerData.gameDifficulty)
        {
            case Difficulty.Easy:
                difficultyText.text = EasyText.GetLocalizedString();
                break;
            case Difficulty.Normal:
                difficultyText.text = NormalText.GetLocalizedString();
                break;
            case Difficulty.Hard:
                difficultyText.text = HardText.GetLocalizedString();
                break;
            case Difficulty.Lunatic:
                difficultyText.text = LunaticText.GetLocalizedString();
                break;
            default:
                difficultyText.text = "Unknown";
                break;
        }

    }
    private void InitializeScores()
    {
        lives = GameData.NUMBER_OF_LIVES;
        spells = GameData.NUMBER_OF_SPELLS;
        power = 0;
        graze = 0;
        currentScore = 0;

        // Ensure UI is in sync at start

        if (powerText != null)
            powerText.text = power.ToString();


        highScore = SaveManager.instance.LoadDataPoints(PlayerData.selectedPlayer, PlayerData.gameDifficulty).highScore;
        HighScoreText.text = highScore.ToString();
        ScoreText.text = "0";




    }

    public void UpdateLives(int change)
    {
        lives += change;
        //LivesText.text = lives.ToString();

        Hearts.instance.UpdateHearts(lives);
        Debug.Log("Lives updated: " + lives);
    }

    public void UpdateSpells(int change)
    {
        spells += change;
        Stars.instance.UpdateHearts(spells);

    }

    public void UpdatePower(int change)
    {

        power += change;
        if (power > powerMax) power = powerMax;
        if (power < 0) power = 0;

        float powerScoreValue = ((float)power / powerMax) * 4;

        powerText.text = powerScoreValue.ToString("0.0"); // Display with one decimal place 


    }



    public void UpdateGraze(int change)
    {
        graze += change;
        currentPoints.graze += change;
        grazeText.text = graze.ToString();
    }
    public void AddScore(int points)
    {
        currentScore += points;
        currentPoints.points += points;
        ScoreText.text = currentScore.ToString();
        if (currentScore > highScore)
        {
            highScore = currentScore;
            HighScoreText.text = highScore.ToString();
        }
    }


    private void OnDisable()
    {

        Points highScore = new Points();
        highScore.playerName = PlayerData.selectedPlayer;
        highScore.difficulty = PlayerData.gameDifficulty;

        int x = int.Parse(HighScoreText.text);

        highScore.highScore = int.Parse(HighScoreText.text);


        SaveManager.instance.SaveDataPoints(highScore);
    }

    public PointsByChapter GetCurrentPoints()
    {
        PointsByChapter currentPointsCopy = currentPoints;
        //reset current points
        currentPoints.points = 0;
        currentPoints.graze = 0;
        return currentPointsCopy;
    }








}