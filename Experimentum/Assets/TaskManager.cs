using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TaskManager : MonoBehaviour
{
    // list of all tasks
    [SerializeField] private List<Task> activateTasks = new List<Task>();
    
    // video and screen
    [SerializeField] private VideoClip reaction;
    [SerializeField] private RawImage videoScreen; 
    private VideoPlayer videoPlayer;

    void Start() 
    {
        SetupVideoPlayer();
        
        Task[] foundTasks = FindObjectsOfType<Task>();
        activateTasks.AddRange(foundTasks); 

        foreach (Task task in activateTasks) 
        {
            task.OnTaskCompleted += HandleTaskCompleted;
        }
    }

    void SetupVideoPlayer() 
    {
        // create VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        
        if (reaction != null)
            videoPlayer.clip = reaction;
        
        // setup for UI display
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        
        // virtual screen
        RenderTexture rt = new RenderTexture(1920, 1080, 24);
        videoPlayer.targetTexture = rt;
        
        
        if (videoScreen != null)
        {
            videoScreen.texture = rt;
            videoScreen.gameObject.SetActive(false); // Hide initially
        }
        else
        {
            Debug.LogError("VideoScreen not assigned");
        }
        
        videoPlayer.loopPointReached += OnVideoEnded;
    }

    // removes task from list if completed and checks if all are completed
    void HandleTaskCompleted(Task completedTask) 
    {
        activateTasks.Remove(completedTask);
        completedTask.OnTaskCompleted -= HandleTaskCompleted;

        if (activateTasks.Count == 0)
        {
            Debug.Log("All tasks completed!");
            PlayReaction();
        }
    }


    void PlayReaction() 
    {
        if (videoPlayer != null && videoPlayer.clip != null)
        {
            // show the video screen
            if (videoScreen != null)
                videoScreen.gameObject.SetActive(true);
            
            videoPlayer.Play();
            Debug.Log($"Playing video: {videoPlayer.clip.name}");
        }
        else
        {
            Debug.LogWarning("No video assigned or VideoPlayer missing");
        }
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        Debug.Log("Reaction video finished");
        
        // Hide video screen
        if (videoScreen != null)
            videoScreen.gameObject.SetActive(false);
    }
}