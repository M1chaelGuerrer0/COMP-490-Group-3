using System;
using UnityEngine;

/// <summary>
/// Represents a single task in the game.
/// 
/// Responsibilities:
/// - Track completion state
/// - Notify listeners when completed (event-driven)
/// 
/// Does NOT handle:
/// - Task order (TaskManager)
/// - Visual updates (SpriteOnTaskComplete)
/// </summary>
public class Task : MonoBehaviour
{
    /// <summary>
    /// Fired when this task is completed.
    /// </summary>
    public event Action<Task> OnTaskCompleted;

    private bool isCompleted = false;

    // =========================
    // TASK STATE
    // =========================

    /// <summary>
    /// Marks this task as completed and notifies listeners.
    /// </summary>
    public void CompleteTask()
    {
        if (isCompleted)
            return; // Prevent duplicate completion

        isCompleted = true;

        Debug.Log($"Task completed: {gameObject.name}");

        NotifyTaskCompleted();
    }

    /// <summary>
    /// Invokes the completion event safely.
    /// </summary>
    private void NotifyTaskCompleted()
    {
        OnTaskCompleted?.Invoke(this);
    }

    // =========================
    // OPTIONAL UTILITIES (FUTURE)
    // =========================

    /// <summary>
    /// Returns whether this task has already been completed.
    /// </summary>
    public bool IsCompleted()
    {
        return isCompleted;
    }

    /// <summary>
    /// Resets this task (useful for restarting levels).
    /// </summary>
    public void ResetTask()
    {
        isCompleted = false;
    }
}