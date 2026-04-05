using UnityEngine;

public class TestTubeController : MonoBehaviour
{
    public int currentStep = 0;

    public Sprite emptyTubeSprite;
    public Sprite redTubeSprite;
    public Sprite yellowTubeSprite;
    public Sprite blueTubeSprite;
    public Sprite violetTubeSprite;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null && emptyTubeSprite != null)
        {
            sr.sprite = emptyTubeSprite;
        }
    }

    public bool AddCrystal(string colorName)
    {
        Debug.Log("Tube got: " + colorName + " at step " + currentStep);

        if (currentStep == 0 && colorName == "Red")
        {
            currentStep = 1;
            UpdateTubeSprite();
            return true;
        }

        if (currentStep == 1 && colorName == "Yellow")
        {
            currentStep = 2;
            UpdateTubeSprite();
            return true;
        }

        if (currentStep == 2 && colorName == "Blue")
        {
            currentStep = 3;
            UpdateTubeSprite();
            return true;
        }

        if (currentStep == 3 && colorName == "Violet")
        {
            currentStep = 4;
            UpdateTubeSprite();
            return true;
        }

        Debug.Log("Wrong order or wrong color");
        return false;
    }

    void UpdateTubeSprite()
    {
        if (sr == null) return;

        if (currentStep == 0 && emptyTubeSprite != null)
            sr.sprite = emptyTubeSprite;
        else if (currentStep == 1 && redTubeSprite != null)
            sr.sprite = redTubeSprite;
        else if (currentStep == 2 && yellowTubeSprite != null)
            sr.sprite = yellowTubeSprite;
        else if (currentStep == 3 && blueTubeSprite != null)
            sr.sprite = blueTubeSprite;
        else if (currentStep == 4 && violetTubeSprite != null)
            sr.sprite = violetTubeSprite;
    }
}