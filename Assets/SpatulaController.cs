using UnityEngine;

public class SpatulaController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;

    private Collider2D myCollider;

    public bool hasCrystal = false;
    public string carriedCrystalColor = "";

    void Start()
    {
        startPosition = transform.position;
        myCollider = GetComponent<Collider2D>();
    }

    void OnMouseDown()
    {
        isDragging = true;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        offset = transform.position - mouse;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.z = 0f;
        transform.position = mouse + offset;
    }

    void OnMouseUp()
    {
        isDragging = false;

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            myCollider.bounds.center,
            myCollider.bounds.size,
            0f
        );

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            if (hit.CompareTag("Cups") && !hasCrystal)
            {
                CrystalSourceCup cup = hit.GetComponent<CrystalSourceCup>();

                if (cup != null)
                {
                    string pickedColor = cup.TakeCrystal();

                    if (pickedColor != "")
                    {
                        hasCrystal = true;
                        carriedCrystalColor = pickedColor;
                        Debug.Log("Picked up: " + carriedCrystalColor);
                        ReturnHome();
                        return;
                    }
                }
            }

            if (hit.CompareTag("Tube") && hasCrystal)
            {
                TestTubeController tube = hit.GetComponent<TestTubeController>();

                if (tube != null)
                {
                    bool added = tube.AddCrystal(carriedCrystalColor);

                    if (added)
                    {
                        hasCrystal = false;
                        carriedCrystalColor = "";
                        Debug.Log("Added to tube");
                    }

                    ReturnHome();
                    return;
                }
            }
        }

        ReturnHome();
    }

    void ReturnHome()
    {
        transform.position = startPosition;
    }
}