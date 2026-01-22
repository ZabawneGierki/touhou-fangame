using Ink.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public struct NamedSprite
{
    public string key;
    public Sprite sprite;
}

public enum DialogueSide
{
    Player,
    Enemy
}

public enum PlayerNames
{
    Reimu,
    Marisa,
}
public enum EnemyNames
{
    Reisen,
    Suika,
}

public class DialogueBox : MonoBehaviour
{
    [Header("Dialogue Reference")]
    [SerializeField] CanvasGroup playerDialogue;
    [SerializeField] CanvasGroup enemyDialogue;
    [SerializeField] Image playerImage;
    [SerializeField] Image enemyImage;

    [SerializeField] TextMeshProUGUI playerDialogueText, enemyDialogueText;

    [Header("Input Action Reference")]
    [SerializeField] InputActionReference interactAction;

    [Header("Expression maps (key -> sprite)")]
    [SerializeField] NamedSprite[] playerExpressions;
    [SerializeField] NamedSprite[] enemyExpressions;

    private Story currentStory;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool skipTyping;

    // active speaker info
    private DialogueSide activeSide = DialogueSide.Player;
    private string activeSpeaker = "";

    public static DialogueBox instance;

    // NEW: event for others to know when dialogue ends
    public event Action onDialogueEnd;

    private void Awake()
    {
        instance = this;
        // make sure UI starts hidden
        SetCanvasGroup(playerDialogue, false);
        SetCanvasGroup(enemyDialogue, false);
    }

    private void OnEnable()
    {
        if (interactAction != null)
            interactAction.action.performed += DisplayNextLine;
    }

    private void OnDisable()
    {
        if (interactAction != null)
            interactAction.action.performed -= DisplayNextLine;
    }

    // Public entry
    public void StartDialogue(Story newStory)
    {
        if (newStory == null) return;
        currentStory = newStory;

        // Setup and pause gameplay input
        OnStartDialogue();

        // Clear UI
        playerDialogueText.text = "";
        enemyDialogueText.text = "";

        // Show first line
        ContinueStory();
    }

    private void OnStartDialogue()
    {
        InputManager.Instance?.SwitchToUIInput();
         
        Time.timeScale = 0f;
    }

    private void OnEndDialogue()
    {
        InputManager.Instance?.SwitchToPlayerInput();   
         
        Time.timeScale = 1f;

        // Hide UI
        SetCanvasGroup(playerDialogue, false);
        SetCanvasGroup(enemyDialogue, false);

        currentStory = null;

        // Invoke completion event
        onDialogueEnd?.Invoke();
    }

    private void DisplayNextLine(InputAction.CallbackContext context)
    {
        // If typing, request skip (finish immediately)
        if (isTyping)
        {
            skipTyping = true;
            return;
        }

        // If no story, ignore
        if (currentStory == null) return;

        if (currentStory.canContinue)
        {
            ContinueStory();
        }
        else
        {
            // nothing more to show -> end dialogue
            OnEndDialogue();
        }
    }

    private void ContinueStory()
    {
        if (currentStory == null) return;

        if (!currentStory.canContinue)
        {
            OnEndDialogue();
            return;
        }

        // Get next line and tags
        string line = currentStory.Continue().Trim();
        List<string> tags = currentStory.currentTags;

        // Default values
        DialogueSide side = DialogueSide.Player;
        string speaker = "";
        string expr = "";

        // parse tags e.g. "speaker:Reimu", "expr:Angry", "side:enemy"
        foreach (var tag in tags)
        {
            var trimmed = tag.Trim();
            var parts = trimmed.Split(new[] { ':' }, 2);
            if (parts.Length != 2) continue;
            var key = parts[0].Trim().ToLowerInvariant();
            var value = parts[1].Trim();

            switch (key)
            {
                case "speaker":
                    speaker = value;
                    break;
                case "expr":
                case "expression":
                    expr = value;
                    break;
                case "side":
                    if (value.Equals("player", StringComparison.OrdinalIgnoreCase))
                        side = DialogueSide.Player;
                    else if (value.Equals("enemy", StringComparison.OrdinalIgnoreCase))
                        side = DialogueSide.Enemy;
                    break;
            }
        }

        // If side not specified, try to infer from speaker name matching enums
        if (string.IsNullOrEmpty(speaker) == false && tags.Exists(t => t.StartsWith("side:", StringComparison.OrdinalIgnoreCase)) == false)
        {
            if (Enum.TryParse<PlayerNames>(speaker, out _))
                side = DialogueSide.Player;
            else if (Enum.TryParse<EnemyNames>(speaker, out _))
                side = DialogueSide.Enemy;
        }

        // Apply UI for active speaker
        activeSide = side;
        activeSpeaker = speaker;
        SetActiveSpeakerUI(side, speaker, expr);

        // Type line
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line, side));
    }

    private IEnumerator TypeLine(string line, DialogueSide side)
    {
        isTyping = true;
        skipTyping = false;

        TextMeshProUGUI targetText = side == DialogueSide.Player ? playerDialogueText : enemyDialogueText;
        targetText.text = "";

        float charDelay = 0.02f;

        for (int i = 0; i < line.Length; i++)
        {
            if (skipTyping)
            {
                targetText.text = line;
                break;
            }

            targetText.text += line[i];
            yield return new WaitForSecondsRealtime(charDelay);
        }

        // ensure full text shown
        targetText.text = line;

        isTyping = false;
        skipTyping = false;
        yield break;
    }

    private void SetActiveSpeakerUI(DialogueSide side, string speaker, string expr)
    {
        if (side == DialogueSide.Player)
        {
            SetCanvasGroup(playerDialogue, true);
            SetCanvasGroup(enemyDialogue, false);
            // set player expression sprite if provided
            var sprite = GetExpressionSprite(playerExpressions, expr);
            if (sprite != null)
                playerImage.sprite = sprite;
        }
        else
        {
            SetCanvasGroup(playerDialogue, false);
            SetCanvasGroup(enemyDialogue, true);
            var sprite = GetExpressionSprite(enemyExpressions, expr);
            if (sprite != null)
                enemyImage.sprite = sprite;
        }
    }

    private Sprite GetExpressionSprite(NamedSprite[] map, string key)
    {
        if (string.IsNullOrEmpty(key) || map == null) return null;
        for (int i = 0; i < map.Length; i++)
        {
            if (map[i].key.Equals(key, StringComparison.OrdinalIgnoreCase))
                return map[i].sprite;
        }
        return null;
    }

    private void SetCanvasGroup(CanvasGroup cg, bool visible)
    {
        if (cg == null) return;
        cg.alpha = visible ? 1f : 0f;
        cg.interactable = visible;
        cg.blocksRaycasts = visible;
    }
}
