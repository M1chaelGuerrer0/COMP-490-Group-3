using UnityEngine;
using TMPro;

/// <summary>
/// UI component for a single task item in the task list.
/// </summary>
public class TaskItemUI : MonoBehaviour
{
    public TMP_Text text;
    public GameObject checkmark;

    /// <summary>
    /// Shows the checkmark when task is completed.
    /// </summary>
    public void Complete()
    {
        checkmark.SetActive(true);
    }
}