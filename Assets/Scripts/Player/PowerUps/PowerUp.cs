using UnityEngine;

public class PowerUp : MonoBehaviour
{
    
    private int powerUpPoints = 0;
    private int powerUpLevel = 0;
    private const int maxPowerUpLevel = 4;

    [SerializeField] private Transform helpersParent;

    [SerializeField] private PowerUpData powerUpData;


     

    public void AddPoints(int points)
    {
        if (powerUpLevel >= maxPowerUpLevel)
        {
            

            return;

        }
           
        powerUpPoints += points;
        Score.instance.UpdatePower(points);

        if (powerUpPoints >= 100)
        {
            powerUpLevel++;
            powerUpPoints = 0;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        powerUpData.PowerUp( helpersParent, powerUpLevel - 1);
         
    }
}
