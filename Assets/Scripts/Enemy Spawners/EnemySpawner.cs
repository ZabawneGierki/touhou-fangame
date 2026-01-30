using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Wave  wave;

    [SerializeField] CanvasGroup fadeCanvas;
    // title and description for each chapter inside the canvas
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private int currentChapterIndex = 0;    

    public bool canContinue = false; // for miniboss.


    [SerializeField] Chapter[] chapters;



    public static EnemySpawner instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(PlayChapter(0));

    }

    


    private IEnumerator PlayChapter(int chapterIndex)
    {
        Chapter chapter = chapters[chapterIndex];
        // Display chapter title and description
        titleText.text = chapter.title;
        descriptionText.text = chapter.description;
        // Fade in
        fadeCanvas.alpha = 1;
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        // Fade out
        float fadeDuration = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            fadeCanvas.alpha = 1 - (elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeCanvas.alpha = 0;

        int WaveCounter = 0;
        // Play each wave in the chapter
        foreach (var wave in chapter.waves)
        {
            yield return StartCoroutine(wave.Play());
            WaveCounter++;
            if(WaveCounter == 10 && chapter.miniBossPrefab != null)
            {
                yield return StartCoroutine(HandleMiniBoss(chapter));
            }



        }
        // After all waves, spawn the boss
        if (chapter.bossPrefab != null)
        {
            Instantiate(chapter.bossPrefab, transform.position, Quaternion.identity);
        }
    }

    public void StartNextChapter()
    {
        currentChapterIndex++;
        if (currentChapterIndex < chapters.Length)
        {
            StartCoroutine(PlayChapter(currentChapterIndex));
        }
        else
        {
            Debug.Log("All chapters completed!");
            // Handle end of game scenario here
            SceneManager.LoadScene("Ending");
        }
    }


    private IEnumerator HandleMiniBoss(Chapter chapter)
    {
        canContinue = false;
        if (chapter.miniBossPrefab != null)
        {
            Instantiate(chapter.miniBossPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => canContinue);


    }


}
