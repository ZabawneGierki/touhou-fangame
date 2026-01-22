using DG.Tweening;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] Vector2 centerOfScreen;

    [SerializeField] Image healthBarFillImage;

    [Header("Dialogue")]
    [SerializeField] public TextAsset inkAsset;
    Story bossDialogue;

    [Header("Health")]
    [SerializeField] int maxHealth = 100;
    private int health;

    [Header("Phases (in order)")]
    [SerializeField] List<Phase> phases = new List<Phase>();


    public int bossIndex = 0; // for multiple bosses in a level
    // runtime state
    private int currentPhaseIndex = 0;
    Coroutine currentPhaseCoroutine;
    private bool isTransitioning = false;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb2d;
    private Color hitTint = new Color(1f, 0.5f, 0.5f, 0.2f);

    private void Awake()
    {
        bossDialogue = inkAsset != null ? new Story(inkAsset.text) : null;
        health = maxHealth;
    }

    void Start()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(centerOfScreen, 1.3f).SetEase(Ease.InOutSine));
        sequence.AppendCallback(PlayDialogue);

        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();


    }

    private void StartFirstPhase()
    {
        if (phases != null && phases.Count > 0)
        {
            currentPhaseIndex = 0;
            currentPhaseCoroutine = StartCoroutine(phases[currentPhaseIndex].ExecutePhase(this));
            Debug.Log($"Boss: starting phase {currentPhaseIndex} ({phases[currentPhaseIndex].name})");
        }
    }

    /// <summary>
    /// Advance to the next phase immediately (stops current phase coroutine).
    /// Call this from your Phase implementation when the phase completes,
    /// or let the boss call it automatically when health drops to zero.
    /// </summary>
    public void NextPhase()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        // stop current running phase
        StopCurrentPhaseCoroutine();

        currentPhaseIndex++;

        if (currentPhaseIndex < phases.Count)
        {
            // reset health for next phase and start it
            health = maxHealth;
            if (healthBarFillImage != null)
            {
                healthBarFillImage.fillAmount = 1f;
            }
            currentPhaseCoroutine = StartCoroutine(phases[currentPhaseIndex].ExecutePhase(this));
            Debug.Log($"Boss: starting phase {currentPhaseIndex} ({phases[currentPhaseIndex].name})");
            isTransitioning = false;
        }
        else
        {
            Debug.Log("Boss: all phases completed.");
            HandleBossDefeated();
        }
    }

    private void StopCurrentPhaseCoroutine()
    {
        if (currentPhaseCoroutine != null)
        {
            try
            {
                StopCoroutine(currentPhaseCoroutine);
                StopAllCoroutines();
            }
            catch { /* ignore if already stopped */ }
            currentPhaseCoroutine = null;
        }
    }

    private void HandleBossDefeated()
    {
        Debug.Log("Boss defeated!");
        if (bossIndex == 5)
            GameManager.instance.LoadEndingScene();
        else
            GameManager.instance.OnBossDefeated();
        // play defeat VFX / sounds here if needed
        // for now destroy the boss object
        Destroy(gameObject);
    }

    private void PlayDialogue()
    {
        if (bossDialogue != null && DialogueBox.instance != null)
        {
            // subscribe to dialogue end and then start dialogue
            DialogueBox.instance.onDialogueEnd += OnBossDialogueEnded;
            DialogueBox.instance.StartDialogue(bossDialogue);
        }
        else
        {
            // no dialogue � start immediately
            StartFirstPhase();
        }
    }

    private void OnBossDialogueEnded()
    {
        // unsubscribe and start the first phase
        if (DialogueBox.instance != null)
            DialogueBox.instance.onDialogueEnd -= OnBossDialogueEnded;

        StartFirstPhase();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        spriteRenderer.color = hitTint;
        if (collision.CompareTag("Projectile"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Apply damage to the boss. When health <= 0, automatically advance to next phase.
    /// </summary>
    /// <param name="v">Damage amount</param>
    public void TakeDamage(int v)
    {
        if (isTransitioning) return;

        health -= v;
        if (healthBarFillImage != null)
        {
            float fillAmount = Mathf.Clamp01((float)health / maxHealth);
            healthBarFillImage.fillAmount = fillAmount;
        }
        //Debug.Log($"Boss took {v} damage (health = {health}).");

        if (health <= 0)
        {
            // advance to next phase
            NextPhase();
             


        }
    }

    public IEnumerator MoveToPoint(Vector2 targetPoint, float duration)
    {
        Vector2 startPoint = rb2d.position;
        Vector2 distance = targetPoint - startPoint;

        // velocity = distance / time
        rb2d.linearVelocity = distance / duration;

        // Wait for the duration of the movement
        yield return new WaitForSeconds(duration);

        // Stop the movement and snap to the exact point to handle float inaccuracies
        rb2d.linearVelocity = Vector2.zero;
        rb2d.position = targetPoint;
    }




    private void OnDestroy()
    {
        Debug.Log("Boss destroyed, showing chapter ending screen.");
        GameManager.instance.OnBossDefeated();
    }

}