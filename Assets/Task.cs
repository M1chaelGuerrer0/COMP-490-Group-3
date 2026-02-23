using System;
using UnityEngine;
public class Task : MonoBehaviour 
{
    public event Action<Task> OnTaskCompleted;

    public void CompleteTask() {
        Debug.Log($"Task completed: {gameObject.name}");
        OnTaskCompleted?.Invoke(this);
    }
}