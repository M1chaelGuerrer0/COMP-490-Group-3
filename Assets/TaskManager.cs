using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls task progression, countdown timer, penalties, and game completion/failure.
/// </summary>

public class TaskManager : MonoBehaviour
{
    // Static flag to globally lock interactions (e.g. during video playback, pause, etc.)
    public static bool IsInteractionLocked = false;

    // experiment index number to keep track of which experiment the user is on for database purposes
    [SerializeField] private int experimentIndex;

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

    // Initialize timer, UI, and video player at scene start
    void Start()
    {
        SetupVideoPlayer();

        timeRemaining = timeLimit;
        timerRunning = true;

        if (penaltyText != null)
            penaltyText.gameObject.SetActive(false);

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Handle pause input and update the countdown timer each frame
    void Update()
    {
        // Handle pause/resume input ESCAPE key
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            TogglePause();
        }

        // Update timer
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

    // Attempt to complete the current task and advance progression if correct
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

    // Update the on-screen countdown timer text
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // Subtract penalty time and show visual feedback
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

    // Show the penalty text and animate its fade-out
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

        IsInteractionLocked = true; // Lock interactions globally

        PlayReaction();

        // update session progress to unlock next experiment
        SessionProgress.CurrentExperiment = Mathf.Max(
            SessionProgress.CurrentExperiment,
            experimentIndex + 1
        );

        // send completion info to database
        ExpDB db = FindFirstObjectByType<ExpDB>();

        if (db != null)
        {
            int currentProgress = db.GetProgress();

            if (currentProgress == experimentIndex)
            {
                db.UpdateProgress(currentProgress + 1);
                Debug.Log("Progress advanced!");
            }
            else
            {
                Debug.Log("Experiment already completed. No progress change.");
            }
        }
    }

    private void HandleFailure()
    {
        Debug.Log("Time's up! Experiment failed.");

        gameEnded = true;
        timerRunning = false;

        IsInteractionLocked = true; // Lock interactions globally

        if (losePanel != null)
            losePanel.SetActive(true); // SHOW LOSE PANEL
    }
    // =========================
    // VIDEO SYSTEM
    // =========================

    // Configure the video player used for experiment completion feedback
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

    // Play the completion reaction video and show the render surface
    private void PlayReaction()
    {
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(true);

        videoPlayer.Play();
    }

    // Called when the completion video finishes playing
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

    [Header("Pause UI")]
    [SerializeField] private GameObject pausePanel;
    
    // Pause the game, stop the timer, and show the pause UI
    public void PauseGame()
    {
        Time.timeScale = 0f;
        timerRunning = false;

        IsInteractionLocked = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    // Resume the game and restore interaction state
    public void ResumeGame()
    {
        if (gameEnded) return;

        Time.timeScale = 1f;
        timerRunning = true;

        IsInteractionLocked = false;

        // Reset any dragging ingredients to prevent bugs when resuming while dragging
        Draggable[] draggables = FindObjectsByType<Draggable>(FindObjectsSortMode.None);
        foreach (var d in draggables)
        {
            d.ResetIfDragging();
        }

        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    public void TogglePause()
    {
        if (pausePanel != null && pausePanel.activeSelf)
        {
            ResumeGame(); // already paused → resume
        }
        else
        {
            PauseGame(); // not paused → pause
        }
    }
    // =========================
    // RESTART SYSTEM
    // =========================

    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsInteractionLocked = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Task GetCurrentTask()
    {
        if (currentTaskIndex >= orderedTasks.Count)
            return null;

        return orderedTasks[currentTaskIndex];
    }
}