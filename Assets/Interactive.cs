using UnityEngine;

/// <summary>
/// Handles player interaction via mouse clicks.
/// 
/// Responsibilities:
/// - Validate if interaction is allowed (task order)
/// - Trigger animation
/// - Complete task AFTER animation finishes (via Animation Event)
/// 
/// Does NOT handle:
/// - Task ordering logic (TaskManager)
/// - Visual state changes (SpriteOnTaskComplete)
/// </summary>
public class Interactive : MonoBehaviour {

    // Task that will be completed after animation finishes
    [Header("Task")]
    [SerializeField] private Task taskToComplete;

    // Animator trigger used to start the interaction animation
    [Header("Animation")]
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string animationTriggerName = "Play";

    // Prevents repeated triggering of the same interaction
    private TaskManager taskManager;
    private bool hasInteracted = false;

    void Awake()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    void OnMouseDown()
    {
        if (!CanInteract())
            return;

        PlayAnimation();
        hasInteracted = true;
    }

    // =========================
    // INTERACTION FLOW
    // =========================

    /// <summary>
    /// Determines whether this interaction is allowed.
    /// </summary>
    private bool CanInteract()
    {
        // Prevent spam clicking
        if (hasInteracted)
            return false;

        // Validate task order
        if (taskManager != null && taskToComplete != null)
        {
            bool canProceed = taskManager.TryCompleteTask(taskToComplete);

            if (!canProceed)
            {
                Debug.Log("Wrong step, cannot interact yet.");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Triggers the animation for this interaction.
    /// </summary>
    private void PlayAnimation()
    {
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(animationTriggerName);
        }
    }

    // =========================
    // ANIMATION EVENT
    // =========================

    /// <summary>
    /// Called at the end of the animation via Animation Event.
    /// Completes the associated task.
    /// </summary>
    public void OnAnimationFinished()
    {
        Debug.Log("Animation finished → completing task.");

        if (taskToComplete != null)
        {
            taskToComplete.CompleteTask();
        }
    }
}