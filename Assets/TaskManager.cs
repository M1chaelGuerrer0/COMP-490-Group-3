using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Controls task progression, countdown timer, penalties, and game completion/failure.
/// </summary>

public class TaskManager : MonoBehaviour
{
    // Static flag to globally lock interactions (e.g. during video playback, pause, etc.)
    public static bool IsInteractionLocked = false;

    /// <summary>
    /// Experiment index number to track which experiment the user is on for database purposes.
    /// Used to determine progress advancement and completion tracking.
    /// </summary>
    [SerializeField] private int experimentIndex;

    // =========================
    // TASK ORDER
    // =========================

    [Header("Task Order")]
    [SerializeField] private List<Task> orderedTasks;
    private int currentTaskIndex = 0;

    [SerializeField] private List<TaskItemUI> taskUIItems;

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
    // Postive Text
    // =========================

    [Header("Positive Feedback")]
    [SerializeField] private TMP_Text successText;
    [SerializeField] private float successTextDuration = 1.5f;
    private Coroutine successCoroutine;

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

    /// <summary>
    /// Initialize the task manager at scene start.
    /// Sets up video player, initializes timer, and hides UI panels.
    /// </summary>
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

    /// <summary>
    /// Handle input and timer updates each frame.
    /// Processes pause/resume input and updates the countdown timer.
    /// </summary>
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

    /// <summary>
    /// Attempts to complete the specified task.
    /// Returns true if it was the correct next task, false otherwise.
    /// </summary>
    /// <param name="attemptedTask">The task the player is trying to complete.</param>
    /// <returns>True if the task was correct and completed, false if wrong.</returns>
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

    /// <summary>
    /// Checks if the given task is the currently required task.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <returns>True if this is the correct current task.</returns>
    private bool IsCorrectTask(Task task)
    {
        return task == orderedTasks[currentTaskIndex];
    }

    /// <summary>
    /// Advances to the next task in the sequence.
    /// Updates UI, increments task index, and checks for completion.
    /// </summary>
    private void AdvanceTask()
    {
        Debug.Log("Correct task completed!");

        // Mark current task as complete in UI
        if (currentTaskIndex < taskUIItems.Count)
        {
            TaskItemUI uiItem = taskUIItems[currentTaskIndex];

            if (uiItem != null)
            {
                uiItem.Complete();
            }
        }

        currentTaskIndex++;

        ShowSuccessFeedback();

        if (currentTaskIndex >= orderedTasks.Count)
        {
            HandleSuccess();
        }
    }

    /// <summary>
    /// Handles an incorrect task attempt by applying a penalty.
    /// </summary>
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

    public void ShowSuccessFeedback()
    {
        if (successText == null) return;

        if (successCoroutine != null)
            StopCoroutine(successCoroutine);

        successCoroutine = StartCoroutine(SuccessTextRoutine());
    }

    private IEnumerator SuccessTextRoutine()
    {
        successText.gameObject.SetActive(true);

        Color originalColor = successText.color;

        successText.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            1f
        );

        float duration = successTextDuration;
        float timer = 0f;

        // Same visible delay as penalty
        float visibleTime = 0.5f;
        yield return new WaitForSecondsRealtime(visibleTime);

        // Same fade logic as penalty
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            successText.color = new Color(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                alpha
            );

            yield return null;
        }

        // Reset
        successText.gameObject.SetActive(false);
        successText.color = originalColor;
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

    /// <summary>
    /// Handles successful completion of all tasks.
    /// Stops the timer, locks interactions, plays reaction video, and updates progress.
    /// </summary>
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

    /// <summary>
    /// Handles experiment failure due to time running out.
    /// Stops the timer, locks interactions, and shows the lose panel.
    /// </summary>
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

    /// <summary>
    /// Configures the video player component for playing completion reactions.
    /// Sets up render texture, assigns video clip, and subscribes to end event.
    /// </summary>
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

    [Header("Pause UI")]
    /// <summary>
    /// UI panel shown when the game is paused.
    /// </summary>
    [SerializeField] private GameObject pausePanel;
    
    /// <summary>
    /// Pauses the game by stopping time and the timer, locking interactions, and showing the pause panel.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0f;
        timerRunning = false;

        IsInteractionLocked = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    /// <summary>
    /// Resumes the game by restoring time scale and timer, unlocking interactions, and hiding the pause panel.
    /// Also resets any dragging objects to prevent bugs when resuming.
    /// </summary>
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

    /// <summary>
    /// Toggles between paused and resumed game states based on current pause panel visibility.
    /// </summary>
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