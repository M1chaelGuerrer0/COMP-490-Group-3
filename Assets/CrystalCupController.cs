using UnityEngine;

public class CrystalCupController : MonoBehaviour
{
    public bool hasCrystalPowder = false;

    public Sprite normalCupSprite;
    public Sprite crystalCupSprite;

    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (sr != null && normalCupSprite != null)
        {
            sr.sprite = normalCupSprite;
        }
    }

    public void AddCrystalPowder()
    {
        if (hasCrystalPowder)
        {
            Debug.Log(gameObject.name + " already has crystal powder");
            return;
        }

        hasCrystalPowder = true;

        if (sr != null && crystalCupSprite != null)
        {
            sr.sprite = crystalCupSprite;
        }

        Debug.Log(gameObject.name + " now has crystal powder");
    }
}