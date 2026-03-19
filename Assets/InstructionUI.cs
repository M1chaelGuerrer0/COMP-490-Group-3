using UnityEngine;

public class InstructionsUI : MonoBehaviour
{
    [SerializeField] private GameObject instructionPanel;

    public void ToggleInstructions()
    {
        instructionPanel.SetActive(!instructionPanel.activeSelf);
    }

    public void CloseInstructions()
    {
        instructionPanel.SetActive(false);
    }
}