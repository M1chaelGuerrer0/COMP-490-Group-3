using UnityEngine;

public class SpoonController : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 offset;
    private bool isDragging = false;

    public bool hasWater = false;

    public Sprite emptySpoonSprite;
    public Sprite fullSpoonSprite;

    private SpriteRenderer sr;
    private Collider2D spoonCollider;

    private void Start()
    {
        startPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
        spoonCollider = GetComponent<Collider2D>();

        if (sr != null && emptySpoonSprite != null)
        {
            sr.sprite = emptySpoonSprite;
        }
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
            spoonCollider.bounds.center,
            spoonCollider.bounds.size,
            0f
        );

        Debug.Log("Dropped spoon. hasWater = " + hasWater);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            Debug.Log("Hit object: " + hit.name + " | Tag: " + hit.tag);

            if (hit.CompareTag("Sink"))
            {
                FillWithWater();
                ReturnToStart();
                return;
            }

            if (hit.CompareTag("Cups"))
            {
                Debug.Log("Cup detected");

                if (hasWater)
                {
                    CupController cup = hit.GetComponent<CupController>();

                    if (cup != null)
                    {
                        cup.AddWater();
                        EmptySpoon();
                        ReturnToStart();
                        return;
                    }
                    else
                    {
                        Debug.Log("CupController missing");
                    }
                }
                else
                {
                    Debug.Log("Spoon has no water");
                }
            }
        }

        ReturnToStart();
    }

    public void FillWithWater()
    {
        hasWater = true;

        if (sr != null && fullSpoonSprite != null)
        {
            sr.sprite = fullSpoonSprite;
        }

        Debug.Log("Spoon filled with water");
    }

    public void EmptySpoon()
    {
        hasWater = false;

        if (sr != null && emptySpoonSprite != null)
        {
            sr.sprite = emptySpoonSprite;
        }

        Debug.Log("Spoon emptied");
    }

    private void ReturnToStart()
    {
        transform.position = startPosition;
    }
}