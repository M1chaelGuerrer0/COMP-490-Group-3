using UnityEngine;

public class CrystalSourceCup : MonoBehaviour
{
    public string crystalColor;
    private bool alreadyTaken = false;

    public string TakeCrystal()
    {
        if (alreadyTaken)
        {
            Debug.Log(gameObject.name + " already used");
            return "";
        }

        alreadyTaken = true;
        Debug.Log("Taken from " + gameObject.name + ": " + crystalColor);
        return crystalColor;
    }
}