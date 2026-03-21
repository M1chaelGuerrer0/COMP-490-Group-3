using UnityEngine;

public class Interactive : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private Task taskToComplete;

    [Header("Animation")]
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string animationTriggerName = "Play";

    private TaskManager taskManager;
    private bool hasInteracted = false; // prevents spam

    void Awake()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    void OnMouseDown()
    {
        // Prevent clicking multiple times
        if (hasInteracted) return;

        // Check if this is the correct task in order
        if (taskManager != null && taskToComplete != null)
        {
            bool canProceed = taskManager.TryCompleteTask(taskToComplete);

            if (!canProceed)
            {
                Debug.Log("Wrong step, cannot interact yet.");
                return;
            }
        }

        // Play animation
        if (targetAnimator != null)
        {
            targetAnimator.SetTrigger(animationTriggerName);
        }

        hasInteracted = true;
    }

    // THIS IS CALLED BY ANIMATION EVENT
    public void OnAnimationFinished()
    {
        Debug.Log("Animation finished, completing task.");

        if (taskToComplete != null)
        {
            taskToComplete.CompleteTask();
        }
    }
}