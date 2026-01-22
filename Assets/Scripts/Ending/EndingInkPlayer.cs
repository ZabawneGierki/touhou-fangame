using Ink.Runtime;

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;

public class EndingInkPlayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI speakerText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private InputActionReference submitAction;

    [Header("Background")]
    [SerializeField] private EndingBackground background;
    [SerializeField] private Sprite[] backgroundSprites;

    [Header("Typewriter")]
    [SerializeField] private float typingSpeed = 0.03f;



    private Story story;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string currentLine;

    void Start()
    {
        LoadEnding();
        ContinueStory();
    }


    private void OnEnable()
    {
        submitAction.action.performed += ctx => OnClick();
        submitAction.action.Enable();
    }

    private void OnDisable()
    {
        submitAction.action.performed -= ctx => OnClick();
        submitAction.action.Disable();
    }

    void OnClick()
    {
        Debug.Log("Submit action performed");
        if (isTyping)
        {
            SkipTyping();
        }
        else
        {
            ContinueStory();
        }
    }

    void LoadEnding()
    {
        string character = PlayerData.selectedPlayer.ToString();
        string language = LocalizationSettings.SelectedLocale.Identifier.Code;

        string path = $"Endings/{character}/ending_{character.ToLower()}_{language}";
        TextAsset inkJson = Resources.Load<TextAsset>(path);

        if (inkJson == null)
        {
            Debug.LogError("Ink not found: " + path);
            return;
        }

        story = new Story(inkJson.text);
    }

    void ContinueStory()
    {
        if (!story.canContinue)
        {
            EndDialogue();
            return;
        }

        currentLine = story.Continue().Trim();
        HandleTags();
        ParseLine(currentLine);
    }

    void ParseLine(string line)
    {
        string speaker = "";
        string text = line;

        if (line.Contains(":"))
        {
            var split = line.Split(new[] { ':' }, 2);
            speaker = split[0];
            text = split[1].Trim();
        }

        speakerText.text = speaker;
        StartTyping(text);
    }

    void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = dialogueText.text = currentLine.Contains(":")
            ? currentLine.Split(new[] { ':' }, 2)[1].Trim()
            : currentLine;

        isTyping = false;
    }

    void HandleTags()
    {
        foreach (string tag in story.currentTags)
        {
            if (tag.StartsWith("BG:"))
            {
                string bgName = tag.Substring(3).Trim();
                SetBackground(bgName);
            }
        }
    }

    void SetBackground(string bgName)
    {
        foreach (var sprite in backgroundSprites)
        {
            if (sprite.name == bgName)
            {
                background.SetBackground(sprite);
                return;
            }
        }

        Debug.LogWarning("Background sprite not found: " + bgName);
    }

    void EndDialogue()
    {
        Debug.Log("Ending finished");
        // fade out / credits / return to menu
        //load main menu or credits scene
        //reset player data if necessary
        PlayerData.selectedPlayer = PlayerName.Reimu;
        PlayerData.gameDifficulty = Difficulty.Normal;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
