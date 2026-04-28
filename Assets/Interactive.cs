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

    // =========================
    // DATA STRUCTURE
    // =========================

    [System.Serializable]
    public class InteractionAction
    {
        // Task that will be completed after animation finishes
        [Header("Task")]
        public Task taskToComplete;

        // Animator trigger used to start the interaction animation
        [Header("Animation")]
        public string animationTriggerName;
    }

    // List of possible interactions (multiple tasks + animations)
    [Header("Interactions")]
    [SerializeField] private InteractionAction[] actions;

    // Animator that plays all interaction animations
    [Header("Animation")]
    [SerializeField] private Animator targetAnimator;

    private TaskManager taskManager;

    // Stores the current valid action until animation finishes
    private InteractionAction currentAction;

    // =========================
    // UNITY METHODS
    // =========================

    void Awake()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    // on mouse up to prevent issues with draggable
    void OnMouseUp()
    {
        if (TaskManager.IsInteractionLocked) return;

        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null && draggable.WasDragged)
            return;

        foreach (var action in actions)
        {
            if (action.taskToComplete == null)
                continue;

            if (taskManager.GetCurrentTask() != action.taskToComplete)
                continue;

            currentAction = action;

            // Decide behavior based on animation
            bool hasAnimation =
                targetAnimator != null &&
                !string.IsNullOrEmpty(action.animationTriggerName);

            if (hasAnimation)
            {
                targetAnimator.SetTrigger(action.animationTriggerName);
            }
            else
            {
                Debug.Log("No animation → completing task immediately.");

                bool canProceed = taskManager.TryCompleteTask(action.taskToComplete);

                if (canProceed)
                {
                    action.taskToComplete.CompleteTask();
                }
            }

            return;
        }

        Debug.Log("Wrong interaction.");
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
        if (currentAction == null)
            return;

        Debug.Log("Animation finished → completing task.");

        // Validate and advance task order
        bool canProceed = taskManager.TryCompleteTask(currentAction.taskToComplete);

        if (!canProceed)
        {
            Debug.Log("Task invalid at animation end.");
            currentAction = null;
            return;
        }
        currentAction.taskToComplete.CompleteTask();

        currentAction = null;
    }
}