using System;
using UnityEngine;

// This class represents a task in the game, which can be completed by interacting with an ingredient.
// It has an event that is triggered when the task is completed, allowing the TaskManager to respond accordingly.
public class Task : MonoBehaviour 
{
    public event Action<Task> OnTaskCompleted;

    public void CompleteTask() {
        Debug.Log($"Task completed: {gameObject.name}");
        OnTaskCompleted?.Invoke(this);
    }
}