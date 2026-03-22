using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// Controls task progression, countdown timer, penalties, and game completion/failure.
/// </summary>

public class TaskManager : MonoBehaviour
{
    // =========================
    // TASK ORDER
    // =========================

    [Header("Task Order")]
    [SerializeField] private List<Task> orderedTasks;
    private int currentTaskIndex = 0;

    // =========================
    // TIMER
    // =========================

    [Header("Timer")]
    [SerializeField] private float timeLimit = 240f;
    [SerializeField] private float penaltyTime = 30f;
    [SerializeField] private TMP_Text timerText;

    private float timeRemaining;
    private bool timerRunning = false;
    private bool gameEnded = false;

    // =========================
    // PENALTY TEXT
    // =========================

    [Header("Penalty Feedback")]
    [SerializeField] private TMP_Text penaltyText;
    [SerializeField] private float penaltyTextDuration = 2f;

    private Coroutine penaltyCoroutine;

    // =========================
    // VIDEO (SUCCESS)
    // =========================

    [Header("Completion Video")]
    [SerializeField] private VideoClip reaction;
    [SerializeField] private RawImage videoScreen;

    private VideoPlayer videoPlayer;

    // =========================
    // UNITY METHODS
    // =========================

    void Start()
    {
        SetupVideoPlayer();

        timeRemaining = timeLimit;
        timerRunning = true;

        if (penaltyText != null)
            penaltyText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!timerRunning || gameEnded)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            HandleFailure();
        }

        UpdateTimerUI();
    }

    // =========================
    // TASK FLOW
    // =========================

    public bool TryCompleteTask(Task attemptedTask)
    {
        if (IsCorrectTask(attemptedTask))
        {
            AdvanceTask();
            return true;
        }

        HandleWrongTask();
        return false;
    }

    private bool IsCorrectTask(Task task)
    {
        return task == orderedTasks[currentTaskIndex];
    }

    private void AdvanceTask()
    {
        Debug.Log("Correct task completed!");

        currentTaskIndex++;

        if (currentTaskIndex >= orderedTasks.Count)
        {
            HandleSuccess();
        }
    }

    private void HandleWrongTask()
    {
        Debug.Log("Wrong task! Penalty applied.");
        AddPenalty();
    }

    // =========================
    // TIMER METHODS
    // =========================

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void AddPenalty()
    {
        timeRemaining -= penaltyTime;

        if (timeRemaining < 0f)
            timeRemaining = 0f;

        Debug.Log($"Penalty Applied! -{penaltyTime} seconds");

        ShowPenaltyText();
    }

    // =========================
    // PENALTY TEXT DISPLAY
    // =========================

    private void ShowPenaltyText()
    {
        Debug.Log("SHOWING PENALTY TEXT");

        if (penaltyText == null) return;

        if (penaltyCoroutine != null)
            StopCoroutine(penaltyCoroutine);

        penaltyCoroutine = StartCoroutine(PenaltyTextRoutine());
    }

    private System.Collections.IEnumerator PenaltyTextRoutine()
    {
        penaltyText.text = $"-{penaltyTime} seconds!";
        penaltyText.gameObject.SetActive(true);

        Color originalColor = penaltyText.color;

        float duration = penaltyTextDuration;
        float timer = 0f;

        // Keep full opacity briefly 
        float visibleTime = 0.5f;
        yield return new WaitForSecondsRealtime(visibleTime);

        // Fade out
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            penaltyText.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        // Reset
        penaltyText.gameObject.SetActive(false);
        penaltyText.color = originalColor;
    }

    // =========================
    // GAME END STATES
    // =========================

    private void HandleSuccess()
    {
        Debug.Log($"All tasks completed! Time left: {timeRemaining:F2} seconds.");

        gameEnded = true;
        timerRunning = false;

        DisableAllInteractions();
        PlayReaction();
    }

    private void HandleFailure()
    {
        Debug.Log("Time's up! Experiment failed.");

        gameEnded = true;
        timerRunning = false;

        DisableAllInteractions();
        if (losePanel != null)
            losePanel.SetActive(true); // SHOW LOSE PANEL
    }

    private void DisableAllInteractions()
    {
        Container[] containers = FindObjectsByType<Container>(FindObjectsSortMode.None);
        foreach (var c in containers)
        {
            c.enabled = false;
        }

        Interactive[] interactives = FindObjectsByType<Interactive>(FindObjectsSortMode.None);
        foreach (var i in interactives)
        {
            i.enabled = false;
        }
    }

    // =========================
    // VIDEO SYSTEM
    // =========================

    private void SetupVideoPlayer()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;

        if (reaction != null)
            videoPlayer.clip = reaction;

        videoPlayer.renderMode = VideoRenderMode.RenderTexture;

        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = rt;

        if (videoScreen != null)
        {
            videoScreen.texture = rt;
            videoScreen.gameObject.SetActive(false);
        }

        videoPlayer.loopPointReached += OnVideoEnded;
    }

    private void PlayReaction()
    {
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(true);

        videoPlayer.Play();
    }

    private void OnVideoEnded(VideoPlayer vp)
    {
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(false);

        Debug.Log("Experiment Complete!");

        if (winPanel != null)
            winPanel.SetActive(true); // SHOW WIN PANEL
    }

    // =========================
    // END UI
    // =========================

    [Header("End UI")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    // =========================
    // PAUSE / RESUME
    // =========================

    public void PauseGame()
    {
        timerRunning = false;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (gameEnded) return;

        timerRunning = true;
        Time.timeScale = 1f;
    }

    // =========================
    // RESTART SYSTEM
    // =========================

    public void RestartGame()
    {
        Time.timeScale = 1f;

        gameEnded = false;
        timerRunning = true;

        timeRemaining = timeLimit;
        currentTaskIndex = 0;

        if (penaltyText != null)
            penaltyText.gameObject.SetActive(false);

        UpdateTimerUI();

        // Re-enable interactions
        Container[] containers = FindObjectsByType<Container>(FindObjectsSortMode.None);
        foreach (var c in containers)
        {
            c.enabled = true;
        }

        Interactive[] interactives = FindObjectsByType<Interactive>(FindObjectsSortMode.None);
        foreach (var i in interactives)
        {
            i.enabled = true;
        }

        if (winPanel != null)
            winPanel.SetActive(false);

        if (losePanel != null)
            losePanel.SetActive(false);
    }
}