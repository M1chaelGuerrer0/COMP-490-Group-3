using UnityEngine;

public class CupController : MonoBehaviour
{
    public int currentFill = 0;
    public int maxFill = 4;

    public Sprite emptyCupSprite;
    public Sprite filledCupSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentFill = 0;

        if (sr != null && emptyCupSprite != null)
        {
            sr.sprite = emptyCupSprite;
        }
    }

    public void AddWater()
    {
        if (currentFill >= maxFill) return;

        currentFill++;

        if (sr != null && filledCupSprite != null)
        {
            sr.sprite = filledCupSprite;
        }

        Debug.Log(gameObject.name + " water amount: " + currentFill);
    }
}