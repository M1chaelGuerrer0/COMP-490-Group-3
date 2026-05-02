using UnityEngine;

/// <summary>
/// Controls the level selection UI by opening the correct level panel.
/// </summary>
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject level1Panel;
    [SerializeField] private GameObject level2Panel;
    [SerializeField] private GameObject level3Panel;
    [SerializeField] private GameObject level4Panel;

    /// <summary>Open the panel for level 1.</summary>
    public void OpenLevel1()
    {
        Debug.Log("Level 1 opened");
        level1Panel.SetActive(true);
    }

    /// <summary>Open the panel for level 2.</summary>
    public void OpenLevel2()
    {
        Debug.Log("Level 2 opened");
        level2Panel.SetActive(true);
    }

    /// <summary>Open the panel for level 3.</summary>
    public void OpenLevel3()
    {
        Debug.Log("Level 3 opened");
        level3Panel.SetActive(true);
    }

    /// <summary>Open the panel for level 4.</summary>
    public void OpenLevel4()
    {
        Debug.Log("Level 4 opened");
        level4Panel.SetActive(true);
    }
}