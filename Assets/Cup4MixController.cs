using UnityEngine;

public class Cup4MixController : MonoBehaviour
{
    public bool hasBlue = false;
    public bool hasRed = false;

    public Sprite blueSprite;
    public Sprite violetSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        hasBlue = false;
        hasRed = false;
    }

    public void AddColor(string incomingColor)
    {
        if (incomingColor == "Blue" && !hasBlue)
        {
            hasBlue = true;

            if (sr != null && blueSprite != null)
            {
                sr.sprite = blueSprite;
            }

            return;
        }

        if (incomingColor == "Red" && !hasRed)
        {
            hasRed = true;
        }

        if (hasBlue && hasRed && sr != null && violetSprite != null)
        {
            sr.sprite = violetSprite;
        }
    }
}