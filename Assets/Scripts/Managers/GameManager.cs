using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    Transform spawnPoint;


    [SerializeField]
    GameObject[] players;


    [SerializeField]
    public GameObject continueScreenPrefab, chapterEndScreenPrefab;





    private GameObject player;



    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }


    }

    private void Start()
    {
        SpawnPlayer();
    }


    private void SpawnPlayer()
    {
        int playerIndex = (int) PlayerData.selectedPlayer;
        Debug.Log("Player Index: " + playerIndex);

        player = Instantiate(players[playerIndex], spawnPoint.position, spawnPoint.rotation);

    }

    public IEnumerator OnPlayerDied()
    {
        PlayerData.playerLives--;
        Score.instance.UpdateLives(-1);
        if (PlayerData.playerLives > 0)
        {
            Debug.Log("Lives left: " + PlayerData.playerLives);

            player.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.4f);
            player.transform.position = spawnPoint.position;
            player.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            player.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.4f);
            continueScreenPrefab.SetActive(true);

            // Game Over logic here
            Debug.Log("Game Over!");
        }
    }


    internal void LoadEndingScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ending");
    }

    internal void OnBossDefeated()
    {
        //show ending screen or load ending scene
        if(chapterEndScreenPrefab != null)
            chapterEndScreenPrefab.SetActive(true);

    }

}
