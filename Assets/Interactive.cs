using UnityEngine;

public class Interactive : MonoBehaviour {

    [System.Serializable]
    public class InteractionAction
    {
        public Task taskToComplete;
        public string animationTriggerName;
    }

    [Header("Interactions")]
    [SerializeField] private InteractionAction[] actions;

    [Header("Animation")]
    [SerializeField] private Animator targetAnimator;

    private TaskManager taskManager;

    private Draggable draggable;
    void Awake()
    {
        taskManager = FindFirstObjectByType<TaskManager>();
        draggable = GetComponent<Draggable>();

    }

    void OnMouseUp()
    {
        if (TaskManager.IsInteractionLocked) return;

        Draggable draggable = GetComponent<Draggable>();
        if (draggable != null && draggable.WasDragged)
            return;

        InteractionAction validAction = null;

        // 🔍 Step 1: Find the correct task WITHOUT triggering penalties
        foreach (var action in actions)
        {
            if (action.taskToComplete == null)
                continue;

            // Only check match, don't call TryCompleteTask yet
            if (taskManager != null && action.taskToComplete == taskManager.GetCurrentTask())
            {
                validAction = action;
                break;
            }
        }

        //  No valid action → wrong click
        if (validAction == null)
        {
            Debug.Log("Wrong interaction.");
            taskManager.AddPenalty(); // optional
            return;
        }

        //  Step 2: Now execute ONLY the correct one
        bool canProceed = taskManager.TryCompleteTask(validAction.taskToComplete);

        if (!canProceed) return;

        if (targetAnimator != null && !string.IsNullOrEmpty(validAction.animationTriggerName))
        {
            targetAnimator.SetTrigger(validAction.animationTriggerName);
        }

        validAction.taskToComplete.CompleteTask();
    }
}