using UnityEngine;

public class CupColorController : MonoBehaviour
{
    public string acceptedColor;
    public int currentDrops = 0;
    public int neededDrops = 2;

    public Sprite coloredSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentDrops = 0;
    }

    public void AddColor(string incomingColor)
    {
        if (incomingColor != acceptedColor) return;
        if (currentDrops >= neededDrops) return;

        currentDrops++;

        if (currentDrops >= neededDrops && sr != null && coloredSprite != null)
        {
            sr.sprite = coloredSprite;
        }

        Debug.Log(gameObject.name + " got color " + incomingColor + " drops: " + currentDrops);
    }
}