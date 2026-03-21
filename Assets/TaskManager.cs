using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

// This class manages the sequence of tasks in the game, 
//      tracks time, applies penalties for incorrect actions, 
//      and handles the reaction video when all tasks are completed.
public class TaskManager : MonoBehaviour
{
    
    [Header("Ordered Task List")]
    [SerializeField] private List<Task> orderedTasks;
    private int currentTaskIndex = 0;

    [Header("Timer")]
    [SerializeField] private float penaltyTime = 5f;
    private float timeElapsed = 0f;
    private bool timerRunning = false;
    [SerializeField] private TMP_Text timerText;

    [Header("Video")]
    [SerializeField] private VideoClip reaction;
    [SerializeField] private RawImage videoScreen;
    private VideoPlayer videoPlayer;

    // on start:
    //     Set up video player
    //     Subscribe to task completion events
    //     Start timer
    
    void Start() {
        SetupVideoPlayer();
        timerRunning = true; // start timer when the game starts
    }
    

    // on update:
    //     If timer is running, update elapsed time
    void Update() {
        if (timerRunning) {
            timeElapsed += Time.deltaTime;
        }
        if (timerText != null) {
            timerText.text = $"Time: {timeElapsed:F2}s";
        }        
    }

    // This method is called by tasks when they are attempted to be completed.
    public bool TryCompleteTask(Task attemptedTask)
    {
        // Correct task
        if (attemptedTask == orderedTasks[currentTaskIndex])
        {
            Debug.Log("Correct task completed!");

            currentTaskIndex++;

            if (currentTaskIndex >= orderedTasks.Count)
            {
                AllTasksCompleted();
            }

            return true; // allow action
        }
        // Wrong task
        else
        {
            Debug.Log("Wrong task! Penalty applied.");
            AddPenalty();
            return false; // block action
        }
    }

    // called by containers when wrong ingredient is added
    public void AddPenalty() {
    timeElapsed += penaltyTime;
    Debug.Log("Penalty Applied! +" + penaltyTime + " seconds");
    }
    
    // when all tasks are completed:
    //      Stop timer and display final time
    //      Play reaction video
    void AllTasksCompleted() {
        Debug.Log("All tasks completed! Total time: " + timeElapsed.ToString("F2") + " seconds.");
        timerRunning = false;
        PlayReaction();
    }

    // sets up the video player component and render texture
    void SetupVideoPlayer() 
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

    // plays the reaction video and shows the video screen
    void PlayReaction() 
    {
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(true);

        videoPlayer.Play();
    }

    // when video ends, hide the video screen and log completion
    void OnVideoEnded(VideoPlayer vp)
    {
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(false);

        Debug.Log("Experiment Complete!");
    }
}