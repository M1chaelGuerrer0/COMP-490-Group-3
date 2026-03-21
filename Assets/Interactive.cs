using UnityEngine;

public class Interactive : MonoBehaviour
{
    [Header("Task")]
    [SerializeField] private Task taskToComplete;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string animationTriggerName = "Play";

    private TaskManager taskManager;
    private bool hasBeenUsed = false;

    void Start()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
    }

    void OnMouseDown()
    {
        // Prevent repeat
        if (hasBeenUsed) return;

        if (taskToComplete == null || taskManager == null) return;

        // Ask TaskManager FIRST (same system as containers)
        bool canProceed = taskManager.TryCompleteTask(taskToComplete);

        if (!canProceed)
        {
            return; // wrong order → do nothing
        }

        // Mark used
        hasBeenUsed = true;

        // Play animation
        if (animator != null)
        {
            animator.SetTrigger(animationTriggerName);
        }

        // Complete task
        taskToComplete.CompleteTask();

        Debug.Log(gameObject.name + " clicked and task completed");
    }
}
