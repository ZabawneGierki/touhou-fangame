using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MenuScene
{
    MainMenu,
    Options,
    Difficulty,
    CharacterSelect,
}

[Serializable]
public class NamedMenuScene
{
    public MenuScene sceneName;
    public GameObject sceneRef;
}

 

public class MenuSceneManager : MonoBehaviour
{
    public List<NamedMenuScene> menuScenes;
    private Stack<MenuScene> sceneHistory = new Stack<MenuScene>();

    [Header("Transition Settings")]
    [SerializeField] float transitionDuration = 0.4f;
    [SerializeField] float screenWidth = 1920f; // Adjust to your UI resolution

    [SerializeField] InputActionReference cancelAction;

    private NamedMenuScene currentActiveScene = null;
    private bool isTransitioning = false;

    private void Awake()
    {
        if (menuScenes != null)
        {
            foreach (var scene in menuScenes)
            {
                if (scene != null && scene.sceneRef != null)
                {
                    scene.sceneRef.SetActive(false);
                    // Prepare CanvasGroups to be invisible initially
                    if (scene.sceneRef.TryGetComponent<CanvasGroup>(out var group))
                    {
                        group.alpha = 0;
                    }
                }
            }
        }
    }

    private void Start()
    {
        ShowMenuScene(MenuScene.MainMenu);
    }

    public void ShowMenuScene(MenuScene v, bool pushHistory = true)
    {
        if (isTransitioning || menuScenes == null) return;

        NamedMenuScene target = menuScenes.Find(s => s.sceneName == v);
        if (target == null || target.sceneRef == null) return;

        // If this is the very first screen, show it without animation
        if (currentActiveScene == null)
        {
            currentActiveScene = target;
            target.sceneRef.SetActive(true);
            if (target.sceneRef.TryGetComponent<CanvasGroup>(out var g)) g.alpha = 1;
            if (pushHistory) sceneHistory.Push(v);
            return;
        }

        // Determine direction: pushHistory true means "Forward" (Right to Left)
        StartCoroutine(AnimateTransition(currentActiveScene, target, pushHistory));

        if (pushHistory)
        {
            if (sceneHistory.Count == 0 || sceneHistory.Peek() != v)
            {
                sceneHistory.Push(v);
            }
        }

        currentActiveScene = target;
    }

    private IEnumerator AnimateTransition(NamedMenuScene outgoing, NamedMenuScene incoming, bool isForward)
    {
        isTransitioning = true;

        RectTransform outRect = outgoing.sceneRef.GetComponent<RectTransform>();
        CanvasGroup outGroup = outgoing.sceneRef.GetComponent<CanvasGroup>();

        RectTransform inRect = incoming.sceneRef.GetComponent<RectTransform>();
        CanvasGroup inGroup = incoming.sceneRef.GetComponent<CanvasGroup>();

        // 1. Prepare Incoming Screen
        incoming.sceneRef.SetActive(true);
        // Forward: New screen starts at Right. Back: New screen starts at Left.
        float startX = isForward ? screenWidth : -screenWidth;
        float endX = isForward ? -screenWidth : screenWidth;

        inRect.anchoredPosition = new Vector2(startX, 0);
        inGroup.alpha = 0;

        // 2. Execute Animations
        // Outgoing moves opposite to incoming
        outRect.DOAnchorPos(new Vector2(endX, 0), transitionDuration).SetEase(Ease.OutQuad);
        outGroup.DOFade(0, transitionDuration).SetEase(Ease.OutQuad);

        // Incoming moves to center
        inRect.DOAnchorPos(Vector2.zero, transitionDuration).SetEase(Ease.OutQuad);
        inGroup.DOFade(1, transitionDuration).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(transitionDuration);

        // 3. Cleanup
        outgoing.sceneRef.SetActive(false);
        isTransitioning = false;
    }

    private void OnEnable()
    {
        if (cancelAction != null)
        {
            cancelAction.action.performed += GoBack;
            cancelAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (cancelAction != null)
        {
            cancelAction.action.performed -= GoBack;
            cancelAction.action.Disable();
        }
    }

    private void GoBack(InputAction.CallbackContext context)
    {
        if (isTransitioning || sceneHistory.Count <= 1) return;

        sceneHistory.Pop();
        MenuScene previousScene = sceneHistory.Peek();

        // Pass pushHistory: false to trigger the "Back" direction logic
        ShowMenuScene(previousScene, pushHistory: false);
    }
    public void CloseGame() => Application.Quit();

    public void ShowMainMenu() => ShowMenuScene(MenuScene.MainMenu);
    public void ShowOptions() => ShowMenuScene(MenuScene.Options);
    public void ShowDifficulty() => ShowMenuScene(MenuScene.Difficulty);
    public void ShowCharacterSelect() => ShowMenuScene(MenuScene.CharacterSelect);
}