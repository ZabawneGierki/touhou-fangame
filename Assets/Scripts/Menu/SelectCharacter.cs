using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class CharacterOption
{
    public PlayerName characterName;
    public Sprite characterSprite;
    public LocalizedString characterDescription;
    public LocalizedString characterTitle;
}


[RequireComponent(typeof(CanvasGroup))] // Automatically adds CanvasGroup if missing
public class SelectCharacter : MonoBehaviour
{
    public CharacterOption[] characterOptions;
    public RectTransform characterDisplayArea;
    private CanvasGroup mainCanvasGroup; // For fading the live screen

    [Header("UI References")]
    public Image characterImage;
    public TextMeshProUGUI characterTitleText;
    public TextMeshProUGUI characterDescriptionText;

    [Header("Settings")]
    [SerializeField] float transitionDuration = 0.4f;
    [SerializeField] InputActionReference pageUp, pageDown, submit;

    private int currentCharacterIndex = 0;
    private bool isTransitioning = false;

    private void Awake()
    {
        mainCanvasGroup = characterDisplayArea.GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        pageUp.action.performed += OnPageUp;
        pageDown.action.performed += OnPageDown;
        submit.action.performed += OnSubmit;

        pageUp.action.Enable();
        pageDown.action.Enable();
        submit.action.Enable();

        UpdateUI(currentCharacterIndex);
    }

    private void OnDisable()
    {
        pageUp.action.performed -= OnPageUp;
        pageDown.action.performed -= OnPageDown;
        submit.action.performed -= OnSubmit;
    }

    private void OnPageUp(InputAction.CallbackContext context)
    {
        ChangeCharacter(-1);
    }

    private void OnPageDown(InputAction.CallbackContext context)
    {
        ChangeCharacter(1);
    }

    private void ChangeCharacter(int direction)
    {
        if (isTransitioning) return;

        int nextIndex = currentCharacterIndex + direction;
        if (nextIndex < 0 || nextIndex >= characterOptions.Length) return;

        StartCoroutine(AnimateTransition(direction, nextIndex));
    }

    private System.Collections.IEnumerator AnimateTransition(int direction, int nextIndex)
    {
        isTransitioning = true;
        float screenHeight = characterDisplayArea.rect.height;

         
        GameObject dummy = Instantiate(characterDisplayArea.gameObject, characterDisplayArea.parent);
        if (dummy.TryGetComponent<SelectCharacter>(out var script)) Destroy(script);

        RectTransform dummyRect = dummy.GetComponent<RectTransform>();
        CanvasGroup dummyGroup = dummy.GetComponent<CanvasGroup>();

         
        currentCharacterIndex = nextIndex;
        UpdateUI(currentCharacterIndex);

         
        Vector2 startPos = new Vector2(0, -screenHeight * direction);
        Vector2 endPos = new Vector2(0, screenHeight * direction);

        characterDisplayArea.anchoredPosition = startPos;
        mainCanvasGroup.alpha = 0; // Start invisible

        // 3. Execute Vertical + Fade Tweens
        // Old screen slides out and fades away
        dummyRect.DOAnchorPos(endPos, transitionDuration).SetEase(Ease.OutQuad);
        dummyGroup.DOFade(0, transitionDuration).SetEase(Ease.OutQuad);

        // New screen slides in and fades in
        characterDisplayArea.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(Ease.OutQuad);
        mainCanvasGroup.DOFade(1, transitionDuration).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(transitionDuration);

        Destroy(dummy);
        isTransitioning = false;
    }

    private void UpdateUI(int index)
    {
        CharacterOption character = characterOptions[index];
        PlayerData.SetSelectedPlayer(character.characterName);
        Debug.Log("Selected Character: " + character.characterName);

        characterImage.sprite = character.characterSprite;
        characterTitleText.text = character.characterTitle.GetLocalizedString();
        characterDescriptionText.text = character.characterDescription.GetLocalizedString();
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (isTransitioning) return;
        SceneManager.LoadScene("Gameplay");
    }
}