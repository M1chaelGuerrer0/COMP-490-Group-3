using UnityEngine;

// toggles which panel to open in level selector
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private GameObject level1Panel;
    [SerializeField] private GameObject level2Panel;
    [SerializeField] private GameObject level3Panel;
    [SerializeField] private GameObject level4Panel;

    public void OpenLevel1()
    {
        Debug.Log("Level 1 opened");
        level1Panel.SetActive(true);
    }
    public void OpenLevel2()
    {
        Debug.Log("Level 2 opened");
        level2Panel.SetActive(true);
    }
    public void OpenLevel3()
    {
        Debug.Log("Level 3 opened");
        level3Panel.SetActive(true);
    }
    public void OpenLevel4()
    {
        Debug.Log("Level 4 opened");
        level4Panel.SetActive(true);
    }
}