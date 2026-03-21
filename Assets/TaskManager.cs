using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

/// <summary>
/// Controls task progression, timing, penalties, and end-of-experiment behavior.
/// 
/// Responsibilities:
/// - Enforce ordered task completion
/// - Track elapsed time
/// - Apply penalties for incorrect actions
/// - Handle experiment completion (video playback)
/// </summary>
public class TaskManager : MonoBehaviour
{
    [Header("Task Order")]
    [SerializeField] private List<Task> orderedTasks;
    private int currentTaskIndex = 0;

    [Header("Timer")]
    [SerializeField] private float penaltyTime = 5f;
    [SerializeField] private TMP_Text timerText;

    private float timeElapsed = 0f;
    private bool timerRunning = false;

    [Header("Completion Video")]
    [SerializeField] private VideoClip reaction;
    [SerializeField] private RawImage videoScreen;

    private VideoPlayer videoPlayer;

    void Start()
    {
        SetupVideoPlayer();
        StartTimer();
    }

    void Update()
    {
        UpdateTimer();
    }

    // =========================
    // TASK FLOW
    // =========================

    /// <summary>
    /// Validates whether a task can be completed based on order.
    /// </summary>
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
    /// Checks if the attempted task matches the expected one.
    /// </summary>
    private bool IsCorrectTask(Task task)
    {
        return task == orderedTasks[currentTaskIndex];
    }

    /// <summary>
    /// Advances to the next task or completes the experiment.
    /// </summary>
    private void AdvanceTask()
    {
        Debug.Log("Correct task completed!");

        currentTaskIndex++;

        if (currentTaskIndex >= orderedTasks.Count)
        {
            CompleteExperiment();
        }
    }

    /// <summary>
    /// Handles incorrect task attempts.
    /// </summary>
    private void HandleWrongTask()
    {
        Debug.Log("Wrong task! Penalty applied.");
        AddPenalty();
    }

    // =========================
    // TIMER
    // =========================

    private void StartTimer()
    {
        timerRunning = true;
    }

    private void UpdateTimer()
    {
        if (!timerRunning) return;

        timeElapsed += Time.deltaTime;

        if (timerText != null)
        {
            timerText.text = $"Time: {timeElapsed:F2}s";
        }
    }

    public void AddPenalty()
    {
        timeElapsed += penaltyTime;
        Debug.Log($"Penalty Applied! +{penaltyTime} seconds");
    }

    // =========================
    // COMPLETION
    // =========================

    private void CompleteExperiment()
    {
        Debug.Log($"All tasks completed! Total time: {timeElapsed:F2} seconds.");

        timerRunning = false;
        PlayReaction();
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
    }
}