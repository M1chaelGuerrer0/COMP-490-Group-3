using UnityEngine;

public class DropperController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;

    public string dropColor;

    private Collider2D myCollider;

    private void Start()
    {
        startPosition = transform.position;
        myCollider = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        isDragging = true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        offset = transform.position - mouseWorldPos;
    }

    private void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        transform.position = mouseWorldPos + offset;
    }

    private void OnMouseUp()
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

            if (hit.CompareTag("Cups"))
            {
                CupColorController normalCup = hit.GetComponent<CupColorController>();
                if (normalCup != null)
                {
                    normalCup.AddColor(dropColor);
                    ReturnToStart();
                    return;
                }

                Cup4MixController mixCup = hit.GetComponent<Cup4MixController>();
                if (mixCup != null)
                {
                    mixCup.AddColor(dropColor);
                    ReturnToStart();
                    return;
                }
            }
        }

        ReturnToStart();
    }

    private void ReturnToStart()
    {
        transform.position = startPosition;
    }
}