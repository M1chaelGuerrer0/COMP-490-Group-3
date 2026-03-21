using UnityEngine;

/// <summary>
/// Handles visual state changes based on task completion.
/// 
/// Responsibilities:
/// - Listen for task completion events
/// - Update this object's sprite accordingly
/// 
/// Does NOT handle:
/// - Task logic (handled by TaskManager)
/// - Animation (handled by Animator)
/// </summary>
public class SpriteOnTaskComplete : MonoBehaviour {
    [System.Serializable]
    public class TaskSpriteMapping {
        
        // Task that triggers this sprite change
        [Header("Trigger")]
        public Task task;

        // Sprite to apply when task completes
        [Header("Result")]
        public Sprite sprite;
    }

    [Header("Task → Sprite Mapping")]
    [SerializeField] private TaskSpriteMapping[] mappings;

    private SpriteRenderer spriteRenderer;

    // Cache SpriteRenderer reference on awake
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Subscribe to task completion events on start
    void Start()
    {
        SubscribeToTasks();
    }

    /// <summary>
    /// Subscribes to all assigned task completion events.
    /// </summary>
    private void SubscribeToTasks()
    {
        foreach (TaskSpriteMapping mapping in mappings)
        {
            if (mapping.task == null)
                continue;

            mapping.task.OnTaskCompleted += (t) => ApplySprite(mapping);
        }
    }

    /// <summary>
    /// Applies the sprite associated with a completed task.
    /// </summary>
    private void ApplySprite(TaskSpriteMapping mapping)
    {
        if (mapping.sprite == null)
            return;

        spriteRenderer.sprite = mapping.sprite;

        Debug.Log($"{gameObject.name} → Sprite changed to: {mapping.sprite.name}");
    }
}