using UnityEngine;

/// <summary>
/// Controls the instruction panel visibility in the UI.
/// </summary>
public class InstructionsUI : MonoBehaviour
{
    [SerializeField] private GameObject instructionPanel;

    /// <summary>
    /// Toggle the instruction panel open or closed.
    /// </summary>
    public void ToggleInstructions()
    {
        instructionPanel.SetActive(!instructionPanel.activeSelf);
    }

    /// <summary>
    /// Close the instruction panel.
    /// </summary>
    public void CloseInstructions()
    {
        instructionPanel.SetActive(false);
    }
}