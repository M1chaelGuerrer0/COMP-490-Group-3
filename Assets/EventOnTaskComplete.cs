using UnityEngine;
using UnityEngine.Events;

public class EventOnTaskComplete : MonoBehaviour
{
    [System.Serializable]
    public class TaskEventMapping
    {
        [Header("Trigger")]
        public Task task;

        [Header("Event")]
        public UnityEvent onTaskCompleted;
    }

    [SerializeField] private TaskEventMapping[] mappings;

    void Start()
    {
        SubscribeToTasks();
    }

    private void SubscribeToTasks()
    {
        foreach (TaskEventMapping mapping in mappings)
        {
            if (mapping.task == null)
                continue;

            mapping.task.OnTaskCompleted += (t) => InvokeEvent(mapping);
        }
    }

    private void InvokeEvent(TaskEventMapping mapping)
    {
        mapping.onTaskCompleted?.Invoke();
    }
}