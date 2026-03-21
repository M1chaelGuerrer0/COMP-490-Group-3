using System;
using UnityEngine;

// This class represents a task in the game. Each task can be completed, and it notifies the TaskManager when it is completed.
// Tasks can be used to represent specific actions the player must take (e.g., "Add Yeast to Cup", "Use Spoon on Flask").
public class Task : MonoBehaviour 
{
    public event Action<Task> OnTaskCompleted;

    private bool isCompleted = false;

    public void CompleteTask()
    {
        if (isCompleted) return; // 🔥 prevent repeat

        isCompleted = true;

        Debug.Log($"Task completed: {gameObject.name}");
        OnTaskCompleted?.Invoke(this);
    }
}